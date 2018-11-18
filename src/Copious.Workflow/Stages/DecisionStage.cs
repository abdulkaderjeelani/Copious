namespace Copious.Workflow.Stages {
    using System.Threading.Tasks;
    using System;
    using Core;

    /// <summary>
    /// Stage that branches the workflow based on the given condition
    /// </summary>
    public class DecisionStage : WorkflowStage {
        /// <summary>
        /// Condition is represented as valid string that can be parsed by LamdaParser,
        /// The varContext for the parser will be the event of the previous stage
        /// </summary>
        public string Condition { get; set; }

        public override Task Execute (WorkflowCommand wfCommand) {
            throw new InvalidOperationException ();
        }

        public class DecisionStageData : WorkflowStageData { }
    }
}