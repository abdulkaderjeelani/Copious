using System;
using System.Collections.Generic;
using System.Linq;
using Copious.Application.Interface;
using Copious.Foundation;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Application {
    public sealed class EventHandlerFactory : IEventHandlerFactory {
        private readonly IServiceProvider _serviceProvider;

        public EventHandlerFactory (IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<IEventHandlerAsync<T>> GetAsyncHandlers<T> () where T : Event => MessageHandlerResolver.GetMessageHandlerType<T> (typeof (IEventHandlerAsync<>))
            .Select (h => ((IEventHandlerAsync<T>) ActivatorUtilities.GetServiceOrCreateInstance (_serviceProvider, h)));

        public IEnumerable<IEventHandler<T>> GetHandlers<T> () where T : Event => MessageHandlerResolver.GetMessageHandlerType<T> (typeof (IEventHandler<>))
            .Select (h => ((IEventHandler<T>) ActivatorUtilities.GetServiceOrCreateInstance (_serviceProvider, h)));
    }
}