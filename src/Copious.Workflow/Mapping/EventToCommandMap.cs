namespace Copious.Workflow.Mapping {
    using System;
    using Foundation.ComponentModel;

    public class EventToCommandMap : Component {
        public EventToCommandMap (string eventName, string commandName) {
            EventName = eventName;
            CommandName = commandName;
            Id = Guid.Empty;
            Version = 1;
        }

        public string EventName { get; }
        public string CommandName { get; }
    }
}