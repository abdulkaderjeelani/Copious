using Copious.Application.Interface;
using Copious.Foundation;
using Copious.Infrastructure.Interface;
using Copious.Persistance.Interface;
using Copious.SharedKernel;
using System;

namespace Copious.Application
{
    public static class RegistrationHelper
    {
        private const string paramRepository = "repository";

        public static void RegisterCrudHandlers<TState, TAggregate>(IContainer container, IRepository<TState> moduleRepository)
            where TState : CopiousEntity, new()
            where TAggregate : StatedAggregate<TAggregate, TState>, new()
        {
            container.Register<CrudCommandHandler<Create<TState>, TAggregate, TState>, ICommandHandlerAsync<Create<TState>>, IRepository<TState>>(paramRepository, moduleRepository);
            container.Register<CrudCommandHandler<Update<TState>, TAggregate, TState>, ICommandHandlerAsync<Update<TState>>, IRepository<TState>>(paramRepository, moduleRepository);
            container.Register<CrudCommandHandler<Delete<TState>, TAggregate, TState>, ICommandHandlerAsync<Delete<TState>>, IRepository<TState>>(paramRepository, moduleRepository);
        }

        public static void RegisterCrudHandlers<TState>(IContainer container, IRepository<TState> moduleRepository)
            where TState : CopiousEntity, new()
        {
            container.Register<CrudCommandHandler<Create<TState>, CrudAggregate<TState>, TState>, ICommandHandlerAsync<Create<TState>>, IRepository<TState>>(paramRepository, moduleRepository);
            container.Register<CrudCommandHandler<Update<TState>, CrudAggregate<TState>, TState>, ICommandHandlerAsync<Update<TState>>, IRepository<TState>>(paramRepository, moduleRepository);
            container.Register<CrudCommandHandler<Delete<TState>, CrudAggregate<TState>, TState>, ICommandHandlerAsync<Delete<TState>>, IRepository<TState>>(paramRepository, moduleRepository);
        }

        public static void RegisterCrudHandlers<TState, TAggregate>(IContainer container, Func<IRepository<TState>> moduleRepositoryGetter)
            where TState : CopiousEntity, new()
            where TAggregate : StatedAggregate<TAggregate, TState>, new()
        {
            container.Register<CrudCommandHandler<Create<TState>, TAggregate, TState>, ICommandHandlerAsync<Create<TState>>, IRepository<TState>>(paramRepository, moduleRepositoryGetter);
            container.Register<CrudCommandHandler<Update<TState>, TAggregate, TState>, ICommandHandlerAsync<Update<TState>>, IRepository<TState>>(paramRepository, moduleRepositoryGetter);
            container.Register<CrudCommandHandler<Delete<TState>, TAggregate, TState>, ICommandHandlerAsync<Delete<TState>>, IRepository<TState>>(paramRepository, moduleRepositoryGetter);
        }

        public static void RegisterCrudHandlers<TState>(IContainer container, Func<IRepository<TState>> moduleRepositoryGetter)
            where TState : CopiousEntity, new()
        {
            container.Register<CrudCommandHandler<Create<TState>, CrudAggregate<TState>, TState>, ICommandHandlerAsync<Create<TState>>, IRepository<TState>>(paramRepository, moduleRepositoryGetter);
            container.Register<CrudCommandHandler<Update<TState>, CrudAggregate<TState>, TState>, ICommandHandlerAsync<Update<TState>>, IRepository<TState>>(paramRepository, moduleRepositoryGetter);
            container.Register<CrudCommandHandler<Delete<TState>, CrudAggregate<TState>, TState>, ICommandHandlerAsync<Delete<TState>>, IRepository<TState>>(paramRepository, moduleRepositoryGetter);
        }
    }
}