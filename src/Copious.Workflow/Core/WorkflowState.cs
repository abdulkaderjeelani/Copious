namespace Copious.Workflow.Core
{
    public enum WorkflowState
    {
        NotStarted,

        Running,

        Completed,
        Failed,
        Stopped,
        Paused,
    }
}