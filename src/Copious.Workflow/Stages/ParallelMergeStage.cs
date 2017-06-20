namespace Copious.Workflow.Stages
{
    using System;
    using System.Threading.Tasks;
    using Core;

    public class ParallelMergeStage : WorkflowStage
    {
        /// <summary>
        /// Stores the stage data that is merged, Checks whether all stages are merged (true when last stage of parllel fork ran to completion)
        /// Executes the next stage (only if all the incoming stages are completed and merged as mentioned before)
        /// </summary>
        /// <param name="wfCommand"></param>
        public override Task Execute(WorkflowCommand wfCommand)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Contains the stages that are merged at this node,
        /// </summary>
        public class ParallelMergeStageData : WorkflowStageData
        {
        }
    }
}