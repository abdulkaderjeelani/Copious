namespace Copious.Workflow.Mapping
{
    using System.Threading.Tasks;
    using Core;
    using Copious.Foundation;

    public class EventToWorkflowCommandMapper : IEventToCommandMapper
    {
        public EventToCommandMap MappingDetail => new EventToCommandMap("*", "WorkflowCommand");

        public Task<Command> Map(Event workflowEvent)
            => Task.FromResult((Command)new WorkflowCommand(workflowEvent.ComponentId, workflowEvent.ComponentVersion)
            {
                Event = workflowEvent,
            });
    }
}