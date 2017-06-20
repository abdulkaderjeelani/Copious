using System.Collections.Generic;
using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.Infrastructure.Interface
{
    public interface IEventBus
    {
        /// <summary>
        /// Publish to internal registered handlers and external world (through MessageBus)
        /// </summary>
        /// <param name="event"></param>
        void Publish<TEvent>(Envelope<TEvent> @event) where TEvent : Event;

        Task PublishAsync<TEvent>(Envelope<TEvent> @event) where TEvent : Event;

        void Publish<TEvent>(IEnumerable<Envelope<TEvent>> events) where TEvent : Event;
    }
}