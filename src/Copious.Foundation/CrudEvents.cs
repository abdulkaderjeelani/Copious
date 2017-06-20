using System.Collections.Generic;

namespace Copious.Foundation
{
    public class EntityEvent<TState> : Event<TState>
        where TState : class, IEntity, new()
    {
        public EntityEvent(TState state, bool isPublishable) : base(state, state.Id, state.Version, isPublishable)
        {
        }
    }

    public class Created<TState> : EntityEvent<TState>
            where TState : class, IEntity, new()
    {
        public Created(TState state, bool isPublishable) : base(state, isPublishable)
        {
        }
    }

    public class Updated<TState> : EntityEvent<TState>
           where TState : class, IEntity, new()
    {
        public Updated(TState state, bool isPublishable) : base(state, isPublishable)
        {
        }

        public Updated(TState state, IEnumerable<string> propertiesToBeUpdated, bool isPublishable) : this(state, isPublishable)
        {
            PropertiesToBeUpdated = propertiesToBeUpdated;
        }

        public IEnumerable<string> PropertiesToBeUpdated { get; set; }
    }

    public class Deleted<TState> : EntityEvent<TState>
         where TState : class, IEntity, new()
    {
        public Deleted(TState state, bool isPublishable) : base(state, isPublishable)
        {
        }
    }
}