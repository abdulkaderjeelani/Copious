namespace Copious.Workflow.Stages
{
    public class StageConnector
    {
        public int FromStageId { get; set; }
        public int ToStageId { get; set; }

        /// <summary>
        /// If a stage has multiple connectors each should be identified by unique name,
        /// E.g. for decision stage there will be 2 connectors with True, False
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// True if this connector is used to connect from any one of next stage to any one of previous stage
        /// If there is no mapping (evt-cmd) specified and pointing stage is already executed (Find from db)
        /// in workflow, then use the same command that is already used to execute this stage in past
        /// </summary>
        public bool IsLoopBack { get; set; }

        public string FromEvent { get; set; }

        public string ToCommand { get; set; }
    }
}