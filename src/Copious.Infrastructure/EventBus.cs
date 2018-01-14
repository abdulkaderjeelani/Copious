using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Copious.Application.Interface;
using Copious.Foundation;
using Copious.Infrastructure.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Infrastructure
{
    public class EventBus : IEventBus
    {
        readonly IServiceProvider _serviceProvider;
        readonly IEventHandlerFactory _handlerFactory;

        public EventBus(IServiceProvider serviceProvider, IEventHandlerFactory handlerFactory)
        {
            _serviceProvider = serviceProvider;
            _handlerFactory = handlerFactory;
        }

        [DebuggerStepThrough]
        public void Publish<TEvent>(Envelope<TEvent> @event) where TEvent : Event
        {
            var syncHandlers = _serviceProvider.GetServices(typeof(IEventHandler<TEvent>)).Select(eh => (IEventHandler<TEvent>)eh);

            if (syncHandlers == null)
                syncHandlers = _handlerFactory.GetHandlers<TEvent>();

            if (syncHandlers != null)
                Parallel.ForEach(syncHandlers, syncHandler => syncHandler.Handle(@event.Body));
        }

        [DebuggerStepThrough]
        public async Task PublishAsync<TEvent>(Envelope<TEvent> @event) where TEvent : Event
        {
            var asyncHandlers = _serviceProvider.GetServices(typeof(IEventHandlerAsync<TEvent>)).Select(eh => (IEventHandlerAsync<TEvent>)eh);

            if (asyncHandlers == null)
                asyncHandlers = _handlerFactory.GetAsyncHandlers<TEvent>();

            if (asyncHandlers != null)
            {
                var asyncTasks = new List<Task>();
                Parallel.ForEach(asyncHandlers, asyncHandler => asyncTasks.Add(asyncHandler.HandleAsync(@event.Body)));
                await Task.WhenAll(asyncTasks.ToArray());
            }
        }

        [DebuggerStepThrough]
        public void Publish<TEvent>(IEnumerable<Envelope<TEvent>> events) where TEvent : Event
        {
            Parallel.ForEach(events, evt => Publish(evt));
        }

        [DebuggerStepThrough]
        public async Task PublishAsync<TEvent>(IEnumerable<Envelope<TEvent>> events) where TEvent : Event
        {
            await Task.Run(() => Publish(events));
        }
    }
}