using Copious.Foundation.ComponentModel;
using Copious.Infrastructure.Interface;
using Copious.Persistance.EF;
using Copious.Persistance.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Copious.Foundation;

namespace Copious.Persistance
{
    public static class RegistrationHelper
    {
        const string paramContext = "dbContext";

        public static void RegisterGeneralQueryHandlers<TState, TContext>(IContainer container, TContext efContext)
            where TState : class, Identifiable<Guid>, new()
            where TContext : DbContext
        {
            container.Register<GeneralQueryHandler<TState>, IQueryHandler<GetAllQuery, List<TState>>, TContext>(paramContext, efContext);
            container.Register<GeneralQueryHandler<TState>, IQueryHandler<FindByIdQuery, TState>, TContext>(paramContext, efContext);
            container.Register<GeneralQueryHandler<TState>, IQueryHandler<SearchQuery<TState>, List<TState>>, TContext>(paramContext, efContext);

            container.Register<GeneralQueryHandler<TState>, IQueryHandlerAsync<GetAllQuery, List<TState>>, TContext>(paramContext, efContext);
            container.Register<GeneralQueryHandler<TState>, IQueryHandlerAsync<FindByIdQuery, TState>, TContext>(paramContext, efContext);
            container.Register<GeneralQueryHandler<TState>, IQueryHandlerAsync<SearchQuery<TState>, List<TState>>, TContext>(paramContext, efContext);
        }

        public static void RegisterGeneralQueryHandlers<TState, TContext>(IContainer container, Func<TContext> efContextGetter)
            where TState : class, Identifiable<Guid>, new()
            where TContext : DbContext
        {
            container.Register<GeneralQueryHandler<TState>, IQueryHandler<GetAllQuery, List<TState>>, TContext>(paramContext, efContextGetter);
            container.Register<GeneralQueryHandler<TState>, IQueryHandler<FindByIdQuery, TState>, TContext>(paramContext, efContextGetter);
            container.Register<GeneralQueryHandler<TState>, IQueryHandler<SearchQuery<TState>, List<TState>>, TContext>(paramContext, efContextGetter);

            container.Register<GeneralQueryHandler<TState>, IQueryHandlerAsync<GetAllQuery, List<TState>>, TContext>(paramContext, efContextGetter);
            container.Register<GeneralQueryHandler<TState>, IQueryHandlerAsync<FindByIdQuery, TState>, TContext>(paramContext, efContextGetter);
            container.Register<GeneralQueryHandler<TState>, IQueryHandlerAsync<SearchQuery<TState>, List<TState>>, TContext>(paramContext, efContextGetter);
        }
    }
}