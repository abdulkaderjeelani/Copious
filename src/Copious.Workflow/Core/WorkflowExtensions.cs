namespace Copious.Workflow.Core {
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using Copious.Foundation.ComponentModel;
    using Stages;

    public static class WorkflowExtensions {
        /// <summary>
        /// Gets the next stage of a decision stage
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="stage"></param>
        /// <param name="result">Result of the decision stage execution,</param>
        /// <returns></returns>
        public static Stage GetNextStage (this Workflow workflow, DecisionStage stage, bool result) =>
            GetStages (workflow, stage.OutConnectors.Where (oc => Convert.ToBoolean (oc.Name) == result)).First ();

        public static IList<Stage> GetNextStages (this Workflow workflow, ParallelForkStage stage) =>
            GetStages (workflow, stage.OutConnectors).ToList ();

        public static void PopulateSample (this Workflow workflow) {
            workflow = workflow ?? new Workflow ();

            workflow.StartEvents = new List<string> { "QuoteCreated" };
            workflow.EndEvents = new List<string> { "DCCreated" };
            workflow.Id = Guid.NewGuid ();
            workflow.Name = "Sample Workflow for Demonstration";

            workflow.Stages = new List<Stage> {
                new ComponentStage {
                StageId = 0,
                OutConnectors = new List<StageConnector> {
                new StageConnector { FromEvent = "QuoteCreated", ToStageId = 1, ToCommand = "WaitForApproval" } //No ToStageId for in connector always this stageId
                },
                },

                new ComponentStage {
                OutConnectors = new List<StageConnector> {
                new StageConnector { FromEvent = "QuoteApproved", ToStageId = 2, ToCommand = "CreateSalesOrder" } //No FromStageId for out connector always this stageId
                },
                ComponentDetail = new ComponentDetail ("Quote", "CRM", ComponentType.InteractiveBusinessComponent),
                StageType = StageType.ComponentStage, //set manually to avoid reflection usage by the run time,
                IsOneOfFirstStage = true,
                ProcessMode = StageProcessMode.InProc, //since the component involved in this stage is InteractiveBusinessComponent, this does not have any impact, as the commands are not sent over bus
                StageId = 1,
                },

                new ComponentStage {
                OutConnectors = new List<StageConnector> {
                new StageConnector { FromEvent = "SalesOrderCreated", ToStageId = 3, ToCommand = "CreateInvoice" }
                },
                ComponentDetail = new ComponentDetail ("Invoice", "Sales", ComponentType.InteractiveBusinessComponent),
                StageType = StageType.ComponentStage,
                ProcessMode = StageProcessMode.InProc,
                StageId = 2,
                },

                new ComponentStage {
                OutConnectors = new List<StageConnector> {
                new StageConnector { FromEvent = "InvoiceApproved", ToStageId = 4, ToCommand = "CreateDC" }
                },
                ComponentDetail = new ComponentDetail ("Invoice", "Sales", ComponentType.InteractiveBusinessComponent),
                StageType = StageType.ComponentStage,
                ProcessMode = StageProcessMode.InProc,
                StageId = 3,
                },

                new ComponentStage {
                //No Outconnector for last stage
                ComponentDetail = new ComponentDetail ("DC", "Shipment", ComponentType.InteractiveBusinessComponent),
                StageType = StageType.ComponentStage,
                ProcessMode = StageProcessMode.InProc,
                IsOneOfLastStage = true,
                StageId = 4,
                }
            };
        }

        static IEnumerable<Stage> GetStages (Workflow workflow, IEnumerable<StageConnector> outConnector) =>
            workflow.Stages.Where (s => outConnector.Select (oc => oc.ToStageId).Contains (s.StageId));
    }
}