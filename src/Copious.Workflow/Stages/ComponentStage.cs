namespace Copious.Workflow.Stages {
    using System.Threading.Tasks;
    using Copious.Foundation;
    using Infrastructure.Interface;

    public class ComponentStage : Stage {
        public ComponentStage () {
            StageType = StageType.ComponentStage;
        }

        /// <summary>
        /// <para>
        /// Determines which command bus to be used, to send the command
        /// in proc uses in memory bus, out proc message queue.
        /// </para>
        /// <para>
        /// RECOMMENDATION FOR STAGE DESIGN: (Not Applicable For <see cref="IInteractiveBusinessComponent"/> )
        /// If the previous component stage (ignoring previous workflow stages, i.e. and current stage involves different bounded context
        /// ComponentDetail.BoundedContext varies) then use the OutProc mode, if InProc is used then it will be run in side the same
        /// context (app/externalprocess), Means no potential use for separation of apps
        /// </para>
        /// </summary>
        public StageProcessMode ProcessMode { get; set; }

        public ComponentDetail ComponentDetail { get; set; }

        /// <summary>
        /// <para>
        /// If the current stage involves a system component, then we dont save the command
        /// If the current stage involves a interactive component, then we parks command in the stage,
        /// The command wont be sent over bus
        /// and set <see cref="StageState.Parked"/>.
        /// </para>
        /// <para>
        /// In GUI when user selects to executes the command from workcenter, we use this command to prefill the data.
        /// </para>
        /// <para>
        /// When user clicks we take them to relevant page and pre-fill the data based on selected workcenter item's JSON
        /// </para>
        /// <para>
        /// Note: In interactive stages after a parked command is executed by user from work center, then the program will raise
        /// a event, so there must be a out connector in the parked stage with this Event as from event.
        /// </para>
        /// </summary>
        public Command CommandInHold { get; set; }

        public Task Execute (Command command, ICommandBus inProcBus, ICommandBus outProcBus) => Task.Run (() => (ProcessMode == StageProcessMode.InProc ? inProcBus : outProcBus).Send (command));
    }
}