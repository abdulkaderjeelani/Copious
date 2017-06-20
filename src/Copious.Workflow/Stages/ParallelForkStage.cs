namespace Copious.Workflow.Stages
{
    using System;
    using System.Threading.Tasks;
    using Core;

    public class ParallelForkStage : WorkflowStage
    {
        public override Task Execute(WorkflowCommand wfCommand)
        {
            throw new NotImplementedException();
        }

        public class ParallelForkStageData : WorkflowStageData
        {
        }
    }
}