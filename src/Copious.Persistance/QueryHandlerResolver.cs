using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Copious.Infrastructure.Interface;
using Copious.Persistance.Interface;

namespace Copious.Persistance
{
    public static class QueryHandlerResolver
    {
        private static readonly Type[] CommonQueryTypes = { typeof(GetAllQuery<>), typeof(FindByIdQuery<>), typeof(SearchQuery<>) };

        private static readonly ConcurrentDictionary<string, List<Type>> QueryHandlersCache = new ConcurrentDictionary<string, List<Type>>();

        public static List<Type> GetQueryHandlerType(Type handlerType, Type moduleQueryHandlerType, Type qryType, Type qryResType)
        {
            var stateType = qryType.GetGenericArguments().FirstOrDefault();

            var key = $"{handlerType.Name}-{moduleQueryHandlerType.Name}-{qryType.Name}-{stateType?.Name}-{qryResType.Name}";

            if (!QueryHandlersCache.TryGetValue(key, out var handlers))
            {
                handlers = TypeLocator.GetGenericImplementor(handlerType, qryType, qryResType);

                if (handlers.Count == 0 && TypeLocator.CheckGenericParameterOfType(qryType, CommonQueryTypes))
                {
                    var qryHandlerType = moduleQueryHandlerType.MakeGenericType(stateType);
                    handlers.Add(qryHandlerType);
                }

                handlers = QueryHandlersCache.AddOrUpdate(key, handlers, (t, m) => m);
            }
            return handlers;
        }
    }
}