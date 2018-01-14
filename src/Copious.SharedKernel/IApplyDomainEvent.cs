using Copious.Foundation;

namespace Copious.SharedKernel
{
    /// <summary>
    /// Interface applicable only on aggregates, to apply the domain change event
    /// </summary>
    public interface IApplyDomainEvent
    {
        void Apply(Event @event);
    }

    public interface IApplyDomainEvent<in TEvent> where TEvent : Event
    {
        void Apply(TEvent @event);
    }
}