using Copious.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Document.Interface.State
{
    public class DocumentAccess : Entity
    {
        public Guid DocumentId { get; set; }        
        public ActorKind ActorKind { get; set; }
        public Guid ActorId { get; set; }
        public DateTimeOffset AccessedOn { get; set; }
        public AccessType AccessType { get; set; }
        public string IpAddress { get; set; }
        public string Location { get; set; }        
        public string Reason { get; set; }

        /* If a user changes a document on request by other user, then we save other user as RequestedBy */
        public ActorKind RequestedByActorKind { get; set; }
        public Guid RequestedByActorId { get; set; }

        public Guid SystemId { get; set; }
        public Guid SubSystemId { get; set; }
    }
}