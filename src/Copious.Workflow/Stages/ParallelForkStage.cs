namespace Copious.Workflow.Stages {
    using System.Threading.Tasks;
    using System;
    using Core;

    public class ParallelForkStage : WorkflowStage {
        public override Task Execute (WorkflowCommand wfCommand) {
            throw new NotImplementedException ();
        }

        public class ParallelForkStageData : WorkflowStageData { }
    }
}