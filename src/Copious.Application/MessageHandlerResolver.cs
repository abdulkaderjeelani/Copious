using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Copious.Foundation;
using Copious.Infrastructure.Interface;

namespace Copious.Application {
    public static class MessageHandlerResolver {
        private static readonly ConcurrentDictionary<string, List<Type>> CommandHandlersCache = new ConcurrentDictionary<string, List<Type>> ();
        private static readonly ConcurrentDictionary<string, List<Type>> EventHandlersCache = new ConcurrentDictionary<string, List<Type>> ();

        /// <summary>
        /// returns the implmenting handlers of the provided inteface
        /// </summary>
        /// <typeparam name="TMessage">Command or Event</typeparam>
        /// <param name="handlerType">ICommandHandler or IEventHandler</param>
        /// <returns></returns>
        public static List<Type> GetMessageHandlerType<TMessage> (Type handlerType) {
            var messageType = typeof (TMessage);
            var stateType = messageType.GetGenericArguments ().FirstOrDefault ();
            var key = $"{handlerType.Name}-{messageType.Name}-{stateType?.Name}";
            var cache = messageType.Name.Contains (nameof (Command)) ? CommandHandlersCache : EventHandlersCache;

            if (!cache.TryGetValue (key, out List<Type> handlers))
                handlers = cache.AddOrUpdate (key, TypeLocator.GetGenericImplementor<TMessage> (handlerType), (t, m) => m);

            return handlers;
        }
    }
}