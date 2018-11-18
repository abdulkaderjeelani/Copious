using System;
using Copious.Foundation;

namespace Copious.SharedKernel {
    public interface ISpecification<in TAggregate> {
        bool IsSatisfiedBy (TAggregate subject);
    }

    public interface ISpecification<in TAggregate, TState> where TState : class, IEntity<Guid>, new () {
        bool IsSatisfiedBy (TAggregate subject, Event<TState> @event, out string failedReason);
    }
}