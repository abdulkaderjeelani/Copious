namespace Copious.Workflow.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core;
    using Dependancies;
    using Exceptions;
    using Foundation.ComponentModel;
    using Infrastructure.Interface;
    using Copious.Foundation;
    using Logging;
    using Mapping;
    using Stages;

    public class WorkflowEngine : Component, IWorkflowEngine
    {
        readonly IWorkflowLogger _wfLogger;
        readonly IWorkflowRepository _wfRepository;
        readonly ICommandBus _inProcBus;
        readonly ICommandBus _outProcBus;
        readonly IWorkflowSecurityProvider _wfSecurity;

        public WorkflowEngine(IWorkflowLogger logger, IWorkflowRepository wfRepository,
                              ICommandBus inProcBus, ICommandBus outProcBus,
                              IWorkflowSecurityProvider wfSecurity = null)
        {
            _wfLogger = logger;
            _wfRepository = wfRepository;
            _inProcBus = inProcBus;
            _outProcBus = outProcBus;
            _wfSecurity = wfSecurity;
        }

        public WorkflowInstance RunningInstance { get; private set; }

        public Task<Command> CreateCommandFromEvent(StageConnector stageConnector, Event @event)
            => WorkflowEvtCmdMapFinder.FindMapper(new EventToCommandMap(stageConnector.FromEvent, stageConnector.ToCommand),
                                            RunningInstance.Workflow.MapNotFoundBehavior).Map(@event);

        public Task ExecuteStageAsync(Stage stage, Command command)
        {
            switch (stage.StageType)
            {
                case StageType.ComponentStage:
                    return ((WorkflowStage)stage).Execute(command as WorkflowCommand);

                case StageType.WorkflowStage:
                    return ((ComponentStage)stage).Execute(command, _inProcBus, _outProcBus);
            }

            return null;
        }

        public Task ExecuteWorkflowStageAsync(WorkflowStage stage, WorkflowCommand command)
        {
            return stage.Execute(command);
        }

        public Task<WorkflowInstance> GetWorkflowAsync(Guid wfInstanceId)
        {
            throw new NotImplementedException();
        }

        public IList<Guid> GetWorkflowIds(string startEvent)
        {
            throw new NotImplementedException();
        }

        public void ProcessEventAsync(Guid workflowInstanceId, Event wfEvent, WorkflowInvoker wfInvoker, Guid invokerId)
        {
            if (workflowInstanceId == Guid.Empty && wfEvent.WorkflowInstanceId == Guid.Empty)
                throw new WorkflowException("WorkflowInstanceId is missing.");

            if (workflowInstanceId != Guid.Empty && wfEvent.WorkflowInstanceId != Guid.Empty && (workflowInstanceId != wfEvent.WorkflowInstanceId))
                throw new WorkflowException("WorkflowInstanceId conflict.");

            var wfInstanceId = wfEvent.WorkflowInstanceId.HasValue ? wfEvent.WorkflowInstanceId.Value : workflowInstanceId;

            RunningInstance = _wfRepository.GetWorkflowInstance(wfInstanceId);
            EnsureWorkflowEvent(RunningInstance.Workflow, wfEvent);
        }

        public Task<WorkflowInstance> StartWorkflowAsync(Event startEvent, WorkflowInvoker wfInvoker, Guid invokerId, Guid? workflowId = default(Guid?))
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if event belongs to the workflow else throws the exception
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="event"></param>
        void EnsureWorkflowEvent(Workflow workflow, Event @event)
        {
            if (!workflow.Stages.SelectMany(s => s.OutConnectors).Any(c => c.FromEvent.Equals(@event.ComponentName, StringComparison.OrdinalIgnoreCase)))
                throw new WorkflowException($"Event is not a part of workflow. Id : {workflow.Id} , Name : {workflow.Name}.");
        }

        /// <summary>
        /// Any failure in engine must be logged using <see cref="Logging.IWorkflowLogger"/>  and notified to users
        /// </summary>
        /// <param name="info">info to log</param>
        void LogAndNotify(string info)
        {
            _wfLogger.LogInfo(info);
        }
    }
}