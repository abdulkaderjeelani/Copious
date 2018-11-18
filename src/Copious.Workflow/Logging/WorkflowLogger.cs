using System;

namespace Copious.Workflow.Logging {
    /// <summary>
    /// Logging class used by the command processors, to log the workflow error.
    /// When ever ther is a error in executing stage or mapping a component, we log it through this class only
    /// </summary>
    public interface IWorkflowLogger {
        void LogInfo (string info);
    }

    public class DefaultWorkflowLogger : IWorkflowLogger {
        public void LogInfo (string info) {
            throw new NotImplementedException ();
        }
    }
}