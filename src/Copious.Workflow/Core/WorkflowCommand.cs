namespace Copious.Workflow.Core {
    using System;
    using Copious.Foundation;

    /// <summary>
    /// A wrapper that contains the result event of previous stage,
    /// This command is used by the workflow stages for their processing
    /// (e.g. decision stage takes the event data matches rules and process it)
    /// </summary>
    public class WorkflowCommand : Command {
        public WorkflowCommand (Guid componentId, int componentVersion) : base (componentId, componentVersion) { }

        /// <summary>
        /// Pay load of the event occured on Previous stage,
        /// </summary>
        public dynamic Event { get; set; } //TODO: change to dynamic

        /// <summary>
        /// Name of event occured on previous stage,
        /// TODO: (Used by parallel fork stage) to create multiple commands and execute
        /// </summary>
        public string EventName { get; set; }

        public override string CompName => nameof (WorkflowCommand);
    }
}