namespace Copious.Workflow.Mapping
{
    using System;
    using Copious.Foundation;

    /// <summary>
    /// If a event-command is not found and <see cref="MapNotFoundBehavior.WrapCommand"/> behavior is set in workflow,
    /// then we wrap the event in a command and send it for execution
    /// </summary>
    public class WrapCommand : Command
    {
        public WrapCommand(Guid componentId, int componentVersion) : base(componentId, componentVersion)
        {
        }

        public Event SourceEvent { get; set; }

        public override string CompName => nameof(WrapCommand);
    }
}