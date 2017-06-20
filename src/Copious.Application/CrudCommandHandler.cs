using AutoMapper;
using Copious.Application.Interface;
using Copious.Foundation;
using Copious.Infrastructure.Interface;
using Copious.Persistance.Interface;
using Copious.SharedKernel;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Copious.Application
{
    public class CrudCommandHandler<TCommand, TAggregate, TState> : CommandHandler<TCommand, TAggregate, TState>,
        ICommandHandler<TCommand>, ICommandHandlerAsync<TCommand>
        where TCommand : Command
        where TAggregate : StatedAggregate<TAggregate, TState>, new()
        where TState : class, IEntity, new()
    {
        private const string InvalidCrudCommandErrorMessage = "Command is not a CRUD";

        public CrudCommandHandler(ILoggerFactory loggerFactory, IExceptionHandler exceptionHandler, IRepository<TState> repository, ICommandGuard guard, IEventBus eventBus, IMapper mapper)
            : base(loggerFactory, exceptionHandler, repository, guard, eventBus, mapper)
        {
        }

        public override void Execute(TCommand command)
        {
            if (!IsCommandACrud(command as Command))
                throw new InvalidOperationException(InvalidCrudCommandErrorMessage);

            base.Execute(command);
        }

        public override async Task ExecuteAsync(TCommand command)
        {
            if (!IsCommandACrud(command as Command))
                throw new InvalidOperationException(InvalidCrudCommandErrorMessage);

            await base.ExecuteAsync(command);
        }

        private static bool IsCommandACrud(Command command)
        {
            switch (command)
            {
                case Create<TState> _:
                case Update<TState> _:
                case Delete<TState> _:
                    return true;

                default:
                    return false;
            }
        }
    }
}