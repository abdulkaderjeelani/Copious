using System;

namespace Copious.Foundation
{
    [Serializable]
    public class Event : Message
    {
        public Event(Guid componentId, int componentVersion) : base(componentId, componentVersion)
        {
        }

        public Event(Guid componentId, int componentVersion, bool isPublishableToExternal) : this(componentId, componentVersion)
        {
            IsPublishableToExternal = isPublishableToExternal;
        }

        /// <summary>
        /// By default we will publish all the events, If an event is not to be published then we set this to false
        /// </summary>
        public bool IsPublishableToExternal { get; protected set; } = true;

        public bool IsNewEvent { get; set; } = false;

        public dynamic Payload { get; set; }
    }

    public class Event<TPayload> : Event
    {
        public Event(Guid componentId, int componentVersion) : base(componentId, componentVersion)
        {
        }

        public Event(TPayload state, Guid componentId, int componentVersion, bool isPublishable) : base(componentId, componentVersion, isPublishable)
        {
            State = state;
        }

        public TPayload State { get; }
    }
}