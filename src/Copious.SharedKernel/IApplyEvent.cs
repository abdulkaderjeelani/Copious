using Copious.Foundation;

namespace Copious.SharedKernel
{
    public interface IApplyEvent
    {
        void Apply(Event @event);
    }

    public interface IApplyEvent<in TEvent> where TEvent : Event
    {
        void Apply(TEvent @event);
    }
}