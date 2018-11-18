using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.Infrastructure.Interface {
    /// <summary>
    /// Provides usability overloads for <see cref="IEventBus"/>
    /// </summary>
    public static class EventBusExtensions {
        public static void Publish<TEvent> (this IEventBus bus, TEvent @event) where TEvent : Event {
            bus.Publish (new Envelope<TEvent> (@event));
        }

        public static void Publish<TEvent> (this IEventBus bus, IEnumerable<TEvent> events) where TEvent : Event {
            bus.Publish (events.Select (x => new Envelope<TEvent> (x)));
        }

        public static async Task PublishAsync<TEvent> (this IEventBus bus, TEvent @event) where TEvent : Event {
            await bus.PublishAsync (new Envelope<TEvent> (@event));
        }
    }
}