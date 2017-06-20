using Copious.Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Copious.Document.Interface.State
{
    /// <summary>
    /// Serves as an index for document storage, For a given instance values correspond to recent version of document,
    /// Contains properties / fields that are only needed for document search /  retrieval.
    /// </summary>
    public class Index : Entity
    {
        /*Basic Index*/        
        public Guid SystemId { get; set; }
        public Guid SubSystemId { get; set; }
        public DocumentKind DocumentKind { get; set; }
        public int VersionNo { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Code { get; set; }
        public string RevisionCode { get; set; }
        public string Tags { get; set; }
        public string Author { get; set; }

        /*Access Index*/
        public DateTimeOffset AccessedOn { get; set; }
        public DateTimeOffset VersionedOn { get; set; }
        public Guid ActorId { get; set; }
        public ActorKind ActorKind { get; set; }


        /*Business Details Index*/
        public string ComponentName { get; set; }
        public Guid ComponentId { get; set; }

    }
}