namespace Copious.Workflow.Stages {
    using System.Threading.Tasks;
    using System;
    using Core;

    public class ApprovalStage : DecisionStage {
        public override Task Execute (WorkflowCommand wfCommand) {
            throw new NotImplementedException ();
        }
    }
}