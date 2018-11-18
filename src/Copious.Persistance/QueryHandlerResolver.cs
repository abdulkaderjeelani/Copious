using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Copious.Foundation;
using Copious.Infrastructure.Interface;

namespace Copious.Persistance {
    public static class QueryHandlerResolver {
        static readonly ConcurrentDictionary<string, List<Type>> QueryHandlersCache = new ConcurrentDictionary<string, List<Type>> ();

        public static List<Type> GetQueryHandlerType<TQuery, TQueryResult> (Type handlerType) {
            var queryType = typeof (TQuery);
            var queryResultType = typeof (TQueryResult);
            // list <x> resove to x, only y means y
            var queryResultStateType = queryResultType.IsGenericType ? queryResultType.GetGenericArguments ().FirstOrDefault () : queryResultType;

            var key = $"{handlerType.Name}-{queryType.Name}- {queryResultType.Name}-{queryResultStateType?.Name}";

            if (!QueryHandlersCache.TryGetValue (key, out List<Type> handlers))
                handlers = QueryHandlersCache.AddOrUpdate (key, TypeLocator.GetGenericImplementor<TQuery, TQueryResult> (handlerType), (t, m) => m);

            return handlers;
        }
    }
}