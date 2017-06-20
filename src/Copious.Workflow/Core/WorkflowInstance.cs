namespace Copious.Workflow.Core
{
    using System;
    using System.Collections.Generic;
    using Stages;

    public class WorkflowInstance
    {
        public WorkflowInstance(Guid workflowId, int workflowVersion, Guid startedBy)
        {
            WorkflowId = workflowId;
            WorkflowVersion = workflowVersion;
            StartedBy = startedBy;
        }

        public Guid Id { get; set; }

        /// <summary>
        /// Captured snapshot of workflow when an instance is created,
        /// !!! Use lazy loading for this based on id and version, Do not load the workflow into instance when loading up
        /// </summary>
        public Workflow Workflow { get; }

        public Guid WorkflowId { get; }

        public int WorkflowVersion { get; }

        public Guid StartedBy { get; }

        public IList<Stage> CurrentStages { get; set; }

        public IDictionary<int, StageState> CurrentStageStates { get; set; }

        /// <summary>
        /// If <see cref="CurrentStageStates"/> is <see cref="StageState.Parked"/>
        /// Then the name of command parked will be held in this property
        /// Key: StageId, Value: CommandName
        /// </summary>
        public IDictionary<int, string> ParkedCommandNames { get; set; }
    }
}