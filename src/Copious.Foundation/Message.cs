using System;
using Copious.Foundation.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Copious.Foundation
{
    public class Message : Operation
    {
        public Message(Guid componentId, int componentVersion)
        {
            Id = Guid.NewGuid();
            ComponentId = componentId;
            ComponentVersion = componentVersion;
        }

        /// <summary>
        /// Agg/ Entity Id
        /// </summary>
        public Guid ComponentId { get; protected set; }

        /// <summary>
        /// Name of the component associated with page
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// Aggregate / Entity version for this command,
        /// The version must also be saved with every entity / agg.
        /// </summary>
        public int ComponentVersion { get; protected set; }

        public Guid? WorkflowInstanceId { get; set; }

        protected dynamic Self => this;

     
    }
}