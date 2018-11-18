namespace Copious.Workflow.Stages {
    public enum StageState {
        NotStarted,

        Running,

        Parked,
        Completed,
        Failed,
        Stopped,
        Paused,
    }
}