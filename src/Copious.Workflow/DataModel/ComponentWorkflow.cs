namespace Copious.Workflow.DataModel
{
    using System;

    public class ComponentWorkflow
    {
        /// <summary>
        /// Component Id is the Unique Id of the item associated, E.g. QuoteId, InvoiceId, EmailId, SmsId, etc.
        /// </summary>
        public Guid ComponentId { get; set; }

        public Guid WorkflowInstanceId { get; set; }
    }
}