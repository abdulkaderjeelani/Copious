using Copious.Foundation;
using Copious.Persistance.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Copious.Persistance
{
    public class QueryHandlerFactory : IQueryHandlerFactory
    {
        readonly IServiceProvider _serviceProvider;

        public QueryHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<IQueryHandlerAsync<TQuery, TQueryResult>> GetAsyncHandlers<TQuery, TQueryResult>() where TQuery : Query
          => QueryHandlerResolver.GetQueryHandlerType<TQuery, TQueryResult>(typeof(IQueryHandlerAsync<,>))
              .Select(h => (IQueryHandlerAsync<TQuery, TQueryResult>)GetQueryHandlerInstance(h));

        public IEnumerable<IQueryHandler<TQuery, TQueryResult>> GetHandlers<TQuery, TQueryResult>() where TQuery : Query
        => QueryHandlerResolver.GetQueryHandlerType<TQuery, TQueryResult>(typeof(IQueryHandler<,>))
              .Select(h => (IQueryHandler<TQuery, TQueryResult>)GetQueryHandlerInstance(h));

        object GetQueryHandlerInstance(Type h)
            => ActivatorUtilities.CreateInstance(_serviceProvider, h);
    }
}