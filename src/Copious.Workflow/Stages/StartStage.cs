﻿namespace Copious.Workflow.Stages
{
    using System;
    using System.Threading.Tasks;
    using Core;

    /// <summary>
    /// IMPROVEMENT: Any custom logic that needs to be executed after workflow has started, is
    /// done by this class.
    /// </summary>
    public class StartStage : WorkflowStage
    {
        /// <summary>
        /// Mark the workflow as complete.
        /// </summary>
        /// <param name="wfCommand"></param>
        public override Task Execute(WorkflowCommand wfCommand)
        {
            throw new NotImplementedException();
        }
    }
}