namespace Copious.Workflow.Stages {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using Core;
    using Engine;

    /// <summary>
    /// Stages that corresponds to workflow, the items in BPMN
    /// Any workflow stage will have only one in connector.
    /// </summary>
    public abstract class WorkflowStage : Stage {
        protected WorkflowStage () {
            StageType = StageType.WorkflowStage;
        }

        public WorkflowEngine WorkflowEngine { get; internal set; }

        /// <summary>
        /// Executes the workflow command syncronously.
        /// Note: All the workflow stages are executed as is, In the context.
        /// </summary>
        /// <param name="wfCommand"></param>
        public abstract Task Execute (WorkflowCommand wfCommand);

        public IDictionary<string, object> CreateVarContextFromEvent (WorkflowCommand wfCommand) {
            throw new NotImplementedException ();
        }
    }
}