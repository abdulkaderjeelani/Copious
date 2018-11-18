namespace Copious.Workflow.Stages {
    using System.Collections.Generic;
    using System;
    using Copious.Foundation.ComponentModel;
    using Core;

    /// <summary>
    /// Workflow step is represented as a Stage,
    /// <list type="">
    /// <listheader>Rules</listheader>
    /// <item> 1. The first stage of the workflow must have only one inConnector </item>
    /// <item> 2. Any workflowstage  must have only one inConnector </item>
    /// </list>
    /// </summary>
    public class Stage : Component {
        public int StageId { get; set; }

        public StageType StageType { get; set; }

        public Guid WorkflowId { get; set; }

        public bool IsOneOfFirstStage { get; set; }

        public bool IsOneOfLastStage { get; set; }

        public StageState State { get; set; }

        /// <summary>
        /// Used to get the stages connected to this stage at the input end
        /// </summary>
        public IList<StageConnector> InConnectors { get; set; }

        /// <summary>
        /// Used to get the stages connected to this stage at the output end
        /// </summary>
        public IList<StageConnector> OutConnectors { get; set; }

        /// <summary>
        /// Conditions on which this stage can be performed,
        /// If null then the stage is executed, If not null then the stage will only be
        /// executed if the condition is met.
        /// If not met then proceed to next stage without execute.
        /// Applicable only for stages with system components,
        /// If condition is set for one stage, next stages must contain a map from NoEvent. i.e NoEvent to Command Map.
        /// As the stage may skip the execution due to condition and there by it produces NoEvent.
        /// </summary>
        public StageConditions ExecutionConditions { get; set; }

        /// <summary>
        /// Conditions on which this stage can be skipped, To skip with out conditions
        /// Set this to StageConditions.Empty
        /// </summary>
        public StageConditions SkipConditions { get; set; }
    }
}