using Copious.Foundation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Copious.SharedKernel
{
    public abstract class StatedAggregate<TAggregate, TState> : Aggregate<TAggregate>
     where TState : class, IEntity, new()
    {
        private readonly List<Event<TState>> _changes;

        /// <summary>
        /// Load with specifcation instances
        /// If a specification requires Reposiotry / External Access Abstract the spec
        /// e.g IUniqueSpecification and supply via DI, then set the needed by calling SetState in interface
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        protected virtual IEnumerable<ISpecification<TAggregate, TState>> GetInvariants<TEvent>(TEvent @event)
            where TEvent : Event<TState>
            => default(IEnumerable<ISpecification<TAggregate, TState>>);

        protected StatedAggregate()
        {
            _changes = new List<Event<TState>>();
        }

        protected StatedAggregate(TState state)
        {
            State = state;
        }

        private TState _state;

        public TState State
        {
            get { return _state; }
            set
            {
                _state = value;
                if (_state != null)
                {
                    Id = _state.Id;
                    Version = _state.Version;
                }
            }
        }

        public override Guid Id
        {
            get
            {
                return base.Id;
            }

            set
            {
                base.Id = value;
                if (State != null)
                    State.Id = value;
            }
        }

        public override int Version
        {
            get
            {
                return base.Version;
            }

            set
            {
                base.Version = value;
                if (State != null)
                    State.Version = value;
            }
        }

        public new IEnumerable<Event<TState>> GetUncommittedChanges() => _changes;

        public override void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        protected void Produce<TEvent>(TEvent @event) where TEvent : Event<TState>
        {
            EnforceInvariants(GetInvariants(@event), @event);
            Produce<TEvent>(@event, true);
        }

        private void Produce<TEvent>(Event<TState> @event, bool raiseEvent) where TEvent : Event<TState>
        {
            (this as IApplyEvent<TEvent>).Apply(@event as TEvent);

            if (raiseEvent)
                _changes.Add(@event);
        }

        protected virtual void EnforceInvariants(IEnumerable<ISpecification<TAggregate, TState>> invariants, Event<TState> @event, InvariantEnforcementStyle enforcemntStyle = InvariantEnforcementStyle.Collect)
        {
            if (invariants == null || !invariants.Any()) return;

            var exceptions = new ConcurrentBag<InvariantException>();

            Parallel.ForEach(invariants, (iv, loopState) =>
            {
                if (!iv.IsSatisfiedBy(Self, @event, out string failedReason))
                    HandleInvariantFailure(enforcemntStyle, iv.GetType().Name + failedReason ?? string.Empty, loopState, exceptions);
            });

            ThrowInvariantException(exceptions);
        }
    }
}