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
        where TCommand : CrudCommand<TState>
        where TAggregate : StatedAggregate<TAggregate, TState>, new()
        where TState : class, IEntity<Guid>, new()
    {
        public CrudCommandHandler(ILoggerFactory loggerFactory, IExceptionHandler exceptionHandler, IRepository<TState> repository, ICommandGuard guard, IEventBus eventBus, IMapper mapper)
            : base(loggerFactory, exceptionHandler, repository, guard, eventBus, mapper)
        {
        }
        
    }
}