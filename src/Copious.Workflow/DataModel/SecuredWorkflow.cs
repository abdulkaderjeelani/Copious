namespace Copious.Workflow.DataModel {
    using System;

    /// <summary>
    /// By default the workflow can only be started
    /// by the users who has the start command rights or
    /// by the system by sending the start command to the workflow
    ///
    /// In GUI we can start the workflow when a UI component in First Interactive Component Stage is saved
    /// (check whether wf exist for the current instance of component e.g Quote, If does not exist start the workflow)
    /// or we can provide a GUI to start workflow e.g Click a StartWorkflow Button
    ///
    /// In case if we need to secure the workflow further, we map workflow to users / user groups,
    /// If a workflow is secured (wf id present in this table) then we allow only the mentioned users
    /// to start the workflow, can be controlled in the application level
    /// </summary>
    public class SecuredWorkflow {
        public Guid? UserId { get; set; }
        public Guid? UserGroupId { get; set; }
        public Guid? SystemId { get; set; }
        public Guid WorkflowId { get; set; }

        public bool IsValid => UserId.HasValue || UserGroupId.HasValue || SystemId.HasValue;
    }
}