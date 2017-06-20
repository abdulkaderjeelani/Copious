namespace Copious.Workflow.Dependancies
{
    using System;
    using Core;

    public interface IWorkflowSecurityProvider
    {
        /// <summary>
        /// Verfies whether the invoker has the access rights for the workflow,
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="invoker"></param>
        /// <param name="invokerId"></param>
        /// <returns></returns>
        bool CheckAccess(Guid workflowId, WorkflowInvoker invoker, Guid invokerId);
    }
}