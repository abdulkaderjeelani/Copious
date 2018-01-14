using System;
using AutoMapper;
using Copious.Foundation;
using Copious.Foundation.ComponentModel;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Copious.SharedKernel
{
    public abstract class Aggregate<TAggregate> : Entity<Guid>, IInteractiveBusinessComponent
    {
        readonly List<Event> _changes;

        /// <summary>
        /// Load with specifcation instances
        /// If a specification requires Reposiotry / External Access Abstract the spec
        /// e.g Identifiable<Guid>Specification and supply via DI, then set the needed by calling SetState in interface
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        protected virtual IEnumerable<ISpecification<TAggregate>> GetInvariants(Event @event) => null;

        //http://stackoverflow.com/questions/22672775/stackoverflowexception-when-accessing-member-of-generic-type-via-dynamic-net-c

        public virtual IMapper Mapper { get; set; }

        protected Aggregate()
        {
            _changes = new List<Event>();
        }

        public IEnumerable<Event> GetUncommittedChanges() => _changes;

        public virtual void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public virtual void LoadsFromHistory(IEnumerable<Event> history)
        {
            Version = -1;
            if (history != null)
            {
                foreach (var e in history) Produce(e, false);
                //After loading events set the version of Agg. to last event version (Agg. versionis incremented per event)
                Version = history.Last().ComponentVersion;
            }
        }

        protected virtual void Produce(Event @event)
        {
            EnforceInvariants(GetInvariants(@event));
            Produce(@event, true);
        }

        /// <summary>
        /// Apply the change event,
        /// </summary>
        /// <param name="event"></param>
        /// <param name="raiseNew"></param>
        /// <remarks>
        /// Self.Handle(Converter.ChangeTo(@event, @event.GetType())); has problem with generic, so used reflection.
        /// https://stackoverflow.com/questions/22672775/stackoverflowexception-when-accessing-member-of-generic-type-via-dynamic-net-c
        /// </remarks>
        void Produce(Event @event, bool raiseNew)
        {
            @event.IsNewEvent = raiseNew;

            GetType().GetTypeInfo()
             .GetMethod("Handle", new[] { @event.GetType() })
             .Invoke(this, new[] { @event });

            if (raiseNew)
                _changes.Add(@event);
        }

        protected virtual void EnforceInvariants(IEnumerable<ISpecification<TAggregate>> invariants, InvariantEnforcementStyle enforcemntStyle = InvariantEnforcementStyle.Collect)
        {
            if (invariants == null || !invariants.Any()) return;

            var exceptions = new ConcurrentBag<InvariantException>();

            Parallel.ForEach(invariants, (iv, loopState) =>
            {
                if (!iv.IsSatisfiedBy(Self))
                    HandleInvariantFailure(enforcemntStyle, iv.GetType().Name, loopState, exceptions);
            });

            ThrowInvariantException(exceptions);
        }

        protected virtual void HandleInvariantFailure(InvariantEnforcementStyle enforcemntStyle, string exMessage, ParallelLoopState loopState, ConcurrentBag<InvariantException> exceptions)
        {
            //Any valid string here based on specification
            exceptions.Add(new InvariantException(exMessage));
            if (enforcemntStyle == InvariantEnforcementStyle.FailFast)
                loopState.Stop();
        }

        protected virtual void ThrowInvariantException(ConcurrentBag<InvariantException> exceptions)
        {
            if (exceptions.Count == 1)
                throw exceptions.First();

            if (exceptions.Count > 1)
                throw new InvariantException(exceptions.AsEnumerable());
        }
    }
}