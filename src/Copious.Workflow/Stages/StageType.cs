namespace Copious.Workflow.Stages {
    /// <summary>
    /// Denotes stage type, helps to avoid reflection in workflow engine and to represent the meta data of a stage.
    /// </summary>
    public enum StageType {
        ComponentStage,
        WorkflowStage
    }
}