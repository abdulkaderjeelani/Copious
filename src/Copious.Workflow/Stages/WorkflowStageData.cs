using Copious.Workflow.Core;

namespace Copious.Workflow.Stages
{
    /// <summary>
    /// <para>
    /// Workflow instances need some internal data,
    /// This internal data is used when they wakeup and Executes.
    /// </para>
    /// <para>
    /// E.g. <see cref="ParallelMergeStage"/> must maintain the data what are the stages that are merged,
    /// During execution we check this and proceed to next when all the incoming stages are merged
    /// </para>
    /// <para>
    /// Improvement:
    /// For now the workflow stages are executed in context, using stage data we can also run them out of process, How we maintain ???
    /// </para>
    /// </summary>
    public abstract class WorkflowStageData
    {
        public WorkflowCommand WorkflowCommand { get; set; }
    }
}