namespace Copious.Workflow.Stages {
    using Copious.Foundation.ComponentModel;

    public struct ComponentDetail {
        public ComponentDetail (string name, string context, ComponentType type) {
            ComponentName = name;
            BoundedContext = context;
            ComponentType = type;
        }

        public string ComponentName { get; }
        public string BoundedContext { get; }
        public ComponentType ComponentType { get; }
    }
}