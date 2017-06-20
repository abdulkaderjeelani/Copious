namespace Copious.Workflow.Stages
{
    public class StageConditions
    {
        public StageConditions()
        {
            IsEmpty = false;
        }

        private StageConditions(bool isEmpty)
        {
            IsEmpty = isEmpty;
        }

        public static readonly StageConditions Empty = new StageConditions(true);

        public bool IsEmpty { get; }
    }
}