namespace Copious.Workflow.Exceptions
{
    using System;

    public class WorkflowException : Exception
    {
        public WorkflowException(string message) : base(message)
        {
        }
    }
}