namespace Copious.Workflow.Engine {
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System;
    using Copious.Foundation;
    using Core;
    using Stages;

    /// <summary>
    /// The engine acts as a process manager, API controllers use the workflow engine to communitcate with workflow,
    /// Engine should be injected to the base controller so that its available to all controllers
    /// </summary>
    public interface IWorkflowEngine {
        WorkflowInstance RunningInstance { get; }

        /// <summary>
        /// Checks whether the given event is a start event in any of available workflows,
        /// Returns all the matching workflow ids
        /// </summary>
        /// <param name="startEvent"></param>
        /// <returns>Workflow ids in which the given command is the start command</returns>
        IList<Guid> GetWorkflowIds (string startEvent);

        //TODO: Design WF instance persistance model, with stages, commands, and its data

        /// <summary>
        /// <para>
        /// By default the workflow can only be started  (by sending the start event to the workflow engine i.e This Method)
        /// by the users who has the start command rights (only a start command can raise a start event) or
        /// by the system
        /// </para>
        /// <para>
        /// In GUI we can start the workflow when a UI component is saved.
        /// (E.g. wf is started when a quote is saved) raising QuoteCreatedEvent
        /// Start the workflow by doing steps below
        /// 1. Create a workflow instance, and persist it and return, set currentstageid = start stage id (0)
        /// 2. Forwards thw workflow to first stage by calling <see cref="ProcessEventAsync(Guid, Event, WorkflowInvoker, Guid)"/> method with params
        /// created instance id , and passed in start event
        /// </para>
        /// <para>
        /// Improvement:
        /// 1. we can provide a GUI to start workflow e.g Click a StartWorkflow Button
        /// 2. This method shall also check whether wf exist for the current instance of component (command.ComponentId exists in any of running workflowInstances)
        /// Note: Workflow instances are persisted in its own database,
        /// </para>
        /// <para>
        /// Validations:
        /// Whether the given event is a start event for any of workflow
        /// Checks the extended workflow rights of the wfInvoker using invokerId
        /// </para>
        /// </summary>
        /// <param name="startEvent"></param>
        /// <param name="workflowId">If passed  the engine will not compute the workflow from command.
        /// Must be passed if the start command matches multiple workflows.</param>
        /// <returns></returns>
        Task<WorkflowInstance> StartWorkflowAsync (Event startEvent, WorkflowInvoker wfInvoker, Guid invokerId, Guid? workflowId = null);

        Task<WorkflowInstance> GetWorkflowAsync (Guid wfInstanceId);

        /// <summary>
        /// <para>
        /// Checks whether <paramref name="workflowInstanceId"/>  == <paramref name="wfEvent"/> if both has values .WorkflowInstanceId or throws EX
        /// IF wfEvent.WorkflowInstanceId is Guid.Empty then only we use workflowInstanceId, if both are empty throws Ex.
        /// Checks whether the event is a workflow event (event that moves workflow from one stage to another). If no then does nothing
        /// </para>
        /// <para>
        /// IF <paramref name="wfEvent"/> is the <see cref="Workflow.StartEvents"/>  of workflow, then we use the zero stage out connectors
        /// to identify the Target Stage.
        /// </para>
        /// <para>
        /// ELSE
        /// </para>
        /// <para>
        /// Computes the current stage of workflow
        /// Finds the out connector <see cref="Stage.OutConnectors"/>   of stage with from event <see cref="StageConnector.FromEvent"/> == <paramref name="wfEvent"/>
        /// From out connector we know the TargetStage <see cref="StageConnector.ToStageId"/>  and TargetCommand <see cref="StageConnector.ToCommand"/>
        /// </para>
        /// <para>
        /// Creates the command from the event of the completed stage by calling<see cref="CreateCommandFromEvent(StageConnector, Event)"/>
        /// Executes the stage, by calling <see cref="ExecuteComponentStageAsync(ComponentStage, Command)"/> where
        /// param <see cref="Stage"/>  =  <see cref="StageConnector.ToStageId"/> (Computed)
        /// param <see cref="Command"/> =  <see cref="StageConnector.ToCommand"/> (Computed)
        /// </para>
        /// <para>
        /// Workflow is completed when the current stage has value <see cref="Stage.IsOneOfLastStage"/> ) = true  and it dont have any out connectors.
        /// the event occured is end event  <see cref="Workflow.EndEvents"/>
        /// Called from programs (api / services) where CommandProcessor is present, Why? only a command processor can execute a command so obviously
        /// only those programs can produce events, when a program produces a event, it does two things,
        /// 1. Publish the event to workflow engine using this method for wf execution
        /// 2. Publish the event to external event bus for other third party or non workflow logic
        /// </para>
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="wfEvent">Any event of the current stage, NULL for forwarding the workflow to first stage from start command</param>
        /// <param name="wfInvoker"></param>
        /// <param name="invokerId"></param>
        /// <returns></returns>
        /// <exception cref=""></exception>
        void ProcessEventAsync (Guid workflowInstanceId, Event wfEvent, WorkflowInvoker wfInvoker, Guid invokerId);

        /// <summary>
        /// Based on type of stage Executes the stage by calling
        /// <see cref="WorkflowStage.Execute(WorkflowCommand)"/> For workflow stage
        /// [<see cref="WorkflowStage.WorkflowEngine"/> will be set as current instance (this) of <see cref="WorkflowEngine"/>  ],
        /// <see cref="ComponentStage.Execute(Command, Infrastructure.Interface.ICommandBus, Infrastructure.Interface.ICommandBus)"/>  For component stage,
        /// <para>
        /// Called From:
        /// </para>
        /// <see cref="ProcessEventAsync(Guid, Event, WorkflowInvoker, Guid)/>
        /// <see cref="ParallelForkStage.Execute(WorkflowCommand)"/>
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        Task ExecuteStageAsync (Stage stage, Command command);

        /// <summary>
        /// Using mapping components creates the command.
        /// <para>
        /// Called From:
        /// </para>
        /// <see cref="ProcessEventAsync(Guid, Event, WorkflowInvoker, Guid)"/>
        /// <see cref="ParallelForkStage.Execute(WorkflowCommand)"/>
        /// </summary>
        /// <param name="stageConnector"></param>
        /// <param name="event"></param>
        /// <returns></returns>
        Task<Command> CreateCommandFromEvent (StageConnector stageConnector, Event @event);
    }
}