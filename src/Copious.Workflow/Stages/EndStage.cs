namespace Copious.Workflow.Stages {
    using System.Threading.Tasks;
    using System;
    using Core;

    /// <summary>
    /// IMPROVEMENT: Any custom logic that needs to be executed after workflow has completed all the stages, is
    /// done by this class.
    /// </summary>
    public class EndStage : WorkflowStage {
        /// <summary>
        /// Mark the workflow as complete.
        /// </summary>
        /// <param name="wfCommand"></param>
        public override Task Execute (WorkflowCommand wfCommand) {
            throw new NotImplementedException ();
        }
    }
}