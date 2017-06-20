namespace Copious.Workflow.Mapping
{
    using System;
    using Copious.Foundation;

    /// <summary>
    /// Used in case if a stage has <see cref="Stages.Stage.ExecutionConditions"/>  set,
    /// and the condition gets failed, In such cases it raises an NoEvent to the engine.
    /// <see cref="Engine.IWorkflowEngine.ProcessEventAsync(Guid, Event, Core.WorkflowInvoker, Guid)"/>
    /// </summary>
    public class NoEvent : Event
    {
        public NoEvent(Guid componentId, int componentVersion) : base(componentId, componentVersion)
        {
        }

        public override string CompName => nameof(Mapping.NoEvent);
    }
}