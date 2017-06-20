using System;

namespace Copious.Foundation
{
    [Serializable]
    public class Command : Message
    {
        public Command(Guid componentId, int componentVersion) : base(componentId, componentVersion)
        {
        }
    }

    public class Command<TPayload> : Command
    {
        public Command(Guid componentId, int componentVersion) : base(componentId, componentVersion)
        {
        }

        public Command(TPayload state, Guid componentId, int componentVersion) : base(componentId, componentVersion)
        {
            State = state;
        }

        public TPayload State { get; }
    }
}