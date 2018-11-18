using Copious.Foundation;

namespace Copious.Application.Interface {
    public interface IEventHandler {
        void Handle (Event @event);
    }

    public interface IEventHandler<in TEvent> where TEvent : Event {
        void Handle (TEvent @event);
    }

    public interface IEnvelopedEventHandler<TEvent> : IEventHandler where TEvent : Event {
        void Handle (Envelope<TEvent> envelope);
    }
}