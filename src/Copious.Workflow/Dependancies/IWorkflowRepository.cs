namespace Copious.Workflow.Dependancies {
    using System;
    using Core;

    public interface IWorkflowRepository {
        WorkflowInstance GetWorkflowInstance (Guid id);
    }
}