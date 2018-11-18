using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Copious.Foundation;

namespace Copious.SharedKernel {
    public abstract class StatedAggregate<TAggregate, TState> : Aggregate<TAggregate>
        where TState : class, IEntity<Guid>, new () {
            readonly List<Event<TState>> _changes;

            /// <summary>
            /// Load with specifcation instances
            /// If a specification requires Reposiotry / External Access Abstract the spec
            /// e.g Identifiable<Guid>Specification and supply via DI, then set the needed by calling SetState in interface
            /// </summary>
            /// <param name="event"></param>
            /// <returns></returns>
            protected virtual IEnumerable<ISpecification<TAggregate, TState>> GetInvariants<TEvent> (TEvent @event)
            where TEvent : Event<TState>
                => default (IEnumerable<ISpecification<TAggregate, TState>>);

            protected StatedAggregate () {
                _changes = new List<Event<TState>> ();
            }

            protected StatedAggregate (TState state) {
                State = state;
            }

            TState _state;

            public TState State {
                get => _state;
                set {
                    _state = value;
                    if (_state == null) return;
                    Id = _state.Id;
                    Version = _state.Version;
                }
            }

            public override Guid Id {
                get => base.Id;

                set {
                    base.Id = value;
                    if (State != null)
                        State.Id = value;
                }
            }

            public override int Version {
                get => base.Version;

                set {
                    base.Version = value;
                    if (State != null) State.Version = value;
                }
            }

            public new IEnumerable<Event<TState>> GetUncommittedChanges () => _changes;

            public override void MarkChangesAsCommitted () {
                _changes.Clear ();
            }

            protected void Produce<TEvent> (TEvent @event) where TEvent : Event<TState> {
                EnforceInvariants (GetInvariants (@event), @event);
                Apply<TEvent> (@event, true);
            }

            void Apply<TEvent> (Event<TState> @event, bool isNewEvent) where TEvent : Event<TState> {
                (this as IApplyDomainEvent<TEvent>)?.Apply (@event as TEvent);

                if (isNewEvent)
                    _changes.Add (@event);
            }

            protected virtual void EnforceInvariants (IEnumerable<ISpecification<TAggregate, TState>> invariants, Event<TState> @event, InvariantEnforcementStyle enforcemntStyle = InvariantEnforcementStyle.Collect) {
                if (invariants == null || !invariants.Any ()) return;

                var exceptions = new ConcurrentBag<InvariantException> ();

                Parallel.ForEach (invariants, (iv, loopState) => {
                    if (!iv.IsSatisfiedBy (Self, @event, out string failedReason))
                        HandleInvariantFailure (enforcemntStyle, iv.GetType ().Name + failedReason ?? string.Empty, loopState, exceptions);
                });

                ThrowInvariantException (exceptions);
            }
        }
}