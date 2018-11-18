namespace Copious.Workflow.Core {
    using System.Collections.Generic;
    using System;
    using Mapping;
    using Stages;

    public class Workflow {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<Stage> Stages { get; set; }

        public IList<string> StartEvents { get; set; }

        /// <summary>
        /// Workflow can have multiple end events at various stages,
        /// Workflow is completed when the current stage doent have any output connector and
        /// the event occured is end event (This is always true, because we popuplate end events from the final workflow stage)
        /// </summary>
        public IList<string> EndEvents { get; set; }

        public MapNotFoundBehavior MapNotFoundBehavior { get; set; }

        /// <summary>
        ///Once we identified the start event, we check the stage 0 (by default) and identify the
        ///stage from its outconnector,
        ///if its not found then, if this prop. is set to true, Engine perform smart scan logic,
        ///search for all stage's outconnectors having fromevent = startevent, then the workflow
        ///is started from the stage in which the out connector is pointing to,
        ///e.g. assume a outconnecor with event=startwf, tostage=4,
        ///if startwf is one of start event of our workflow, then we find this connector,
        ///we know that to stage is 4, so the workflow starts from stage 4.
        ///this is similar logic in processevent function,
        ///<para>
        ///Condition,
        ///start event of workflow must occur in only one out connectors of a workflow, else we endup with more than one matches
        ///</para>
        /// </summary>
        public bool SmartScanEnabled { get; set; } = false; // Recommended to be false, as this is a overhead for engine, Use only when absolutely necessary.
    }
}