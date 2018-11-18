using System;
using System.Threading.Tasks;
using Copious.Application.Interface;
using Copious.Foundation;
using Copious.Infrastructure.Interface;
using Copious.Persistance.Interface;
using Copious.SharedKernel;
using Copious.SharedKernel.Exceptions;
using Microsoft.Extensions.Logging;
using Polly;

namespace Copious.Application {
    public class CommandHandler<TCommand, TAggregate, TState> : ICommandHandler<TCommand>, ICommandHandlerAsync<TCommand> where TCommand : Command where TAggregate : StatedAggregate<TAggregate, TState>, new () where TState : class, IEntity<Guid>, new () {
        protected readonly IEventBus _eventBus;
        protected readonly IExceptionHandler _exceptionHandler;
        protected readonly ICommandGuard _guard;
        protected readonly IMapper _mapper;
        protected readonly IRepository<TState> _repository;
        private readonly ILogger _logger;
        private readonly Func<int, TimeSpan> exponentialWaiter = (attempt) => TimeSpan.FromSeconds (Math.Pow (2, attempt));

        protected CommandHandler (ILoggerFactory loggerFactory, IExceptionHandler exceptionHandler, IRepository<TState> repository, ICommandGuard guard, IEventBus eventBus) {
            _exceptionHandler = exceptionHandler;
            _repository = repository;
            _eventBus = eventBus;
            _logger = loggerFactory.CreateLogger<CommandHandler<TCommand, TAggregate, TState>> ();
            _guard = guard;
        }

        protected CommandHandler (ILoggerFactory loggerFactory, IExceptionHandler exceptionHandler, IRepository<TState> repository, ICommandGuard guard, IEventBus eventBus, IMapper mapper) : this (loggerFactory, exceptionHandler, repository, guard, eventBus) {
            _mapper = mapper;
        }

        public virtual void Execute (TCommand command) =>
            GetDefaultSyncPolicy (2).Execute (() => {
                var agg = ExecuteCommand (command);
                _repository.Save (agg.Id, command.ComponentVersion, agg.State, agg.GetUncommittedChanges ());
                Parallel.ForEach (agg.GetUncommittedChanges (), evt => _eventBus.Publish (evt));
            });

        public virtual async Task ExecuteAsync (TCommand command) =>
            await GetDefaultAsyncPolicy (2).ExecuteAsync (async () => {
                var agg = ExecuteCommand (command);
                await _repository.SaveAsync (agg.Id, command.ComponentVersion, agg.State, agg.GetUncommittedChanges ());
                Parallel.ForEach (agg.GetUncommittedChanges (), evt => _eventBus.Publish (evt));
            });

        protected TAggregate ExecuteCommand (TCommand command) {
            var state = _repository.Get (command.ComponentId);

            var agg = new TAggregate {
                Mapper = _mapper
            };

            if (state == null) agg.Id = command.ComponentId;
            else agg.State = state;

            //Version Check 1 - check whether there is any modification between read and command execution
            if (command.ComponentVersion > 0 && agg.Version != command.ComponentVersion)
                throw new VersionConflictException (command.ComponentVersion, agg.Version);

            (agg as IHandleCommand<TCommand>)?.Handle (command);
            return agg;
        }

        private Policy GetDefaultAsyncPolicy (int retryCount) =>
            Policy.Handle<Exception> ().WaitAndRetryAsync (
                retryCount: retryCount,
                sleepDurationProvider: exponentialWaiter,
                onRetry: onRetry);

        private Policy GetDefaultSyncPolicy (int retryCount) =>
            Policy.Handle<Exception> (ex => !(ex is VersionConflictException)).WaitAndRetry (
                retryCount: retryCount,
                sleepDurationProvider: exponentialWaiter,
                onRetry: onRetry);

        //todo: take policy region it to utils

        #region Policy

        private void onRetry (Exception exception, TimeSpan calculatedWaitDuration) => _exceptionHandler.HandleException (exception);

        #endregion Policy
    }
}