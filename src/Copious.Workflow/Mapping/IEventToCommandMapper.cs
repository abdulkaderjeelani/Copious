namespace Copious.Workflow.Mapping {
    using System.Threading.Tasks;
    using Copious.Foundation;

    public interface IEventToCommandMapper {
        EventToCommandMap MappingDetail { get; }

        Task<Command> Map (Event workflowEvent);
    }
}