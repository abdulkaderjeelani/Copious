using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Copious.Foundation;
using Copious.Application.Interface;
using Copious.SharedKernel;
using Copious.Infrastructure.Interface;

namespace Copious.Application
{
    public static class MessageHandlerResolver
    {
        private static readonly Type[] CrudCommandTypes = { typeof(Create<>), typeof(Update<>), typeof(Delete<>) };

        private static readonly ConcurrentDictionary<string, List<Type>> CommandHandlersCache = new ConcurrentDictionary<string, List<Type>>();

        public static List<Type> GetCommandHandlerType<T>(Type handlerType)
        {
            var cmdType = typeof(T);
            var stateType = cmdType.GetGenericArguments().FirstOrDefault();

            var key = $"{handlerType.Name}-{cmdType.Name}-{stateType?.Name}";

            if (!CommandHandlersCache.TryGetValue(key, out List<Type> handlers))
            {
                handlers = TypeLocator.GetGenericImplementor<Type>(handlerType);

                //make sure that the command is a generic CrudCommand
                if (handlers.Count == 0 && TypeLocator.CheckGenericParameterOfType(cmdType, CrudCommandTypes))
                {
                    var crudAggType = typeof(CrudAggregate<>);
                    var aggType = crudAggType.MakeGenericType(stateType);
                    var crudHandlerType = typeof(CrudCommandHandler<,,>).MakeGenericType(cmdType, aggType, stateType);
                    handlers.Add(crudHandlerType);
                }

                handlers = CommandHandlersCache.AddOrUpdate(key, handlers, (t, m) => m);
            }

            return handlers;
        }

        private static readonly ConcurrentDictionary<string, List<Type>> EventHandlersCache = new ConcurrentDictionary<string, List<Type>>();

        public static List<Type> GetEventHandlerType<T>(Type handlerType)
        {
            var evtType = typeof(T);
            var stateType = evtType.GetGenericArguments().FirstOrDefault();

            var key = $"{handlerType.Name}-{evtType.Name}-{stateType?.Name}";

            if (!EventHandlersCache.TryGetValue(key, out List<Type> handlers))
            {
                handlers = TypeLocator.GetGenericImplementor<Type>(handlerType);
                handlers = EventHandlersCache.AddOrUpdate(key, handlers, (t, m) => m);
            }

            return handlers;
        }
    }
}