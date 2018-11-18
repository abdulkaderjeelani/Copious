namespace Copious.Workflow.External {
    /// <summary>
    /// This class is called from command bus (memory - in the send method / mq - in the receiver side, receives command and sends to processor)
    /// Load the component, Find a command handler for the command to execute, send the command to the command handler.
    /// </summary>
    public class ComponentCommandProcessor { }
}