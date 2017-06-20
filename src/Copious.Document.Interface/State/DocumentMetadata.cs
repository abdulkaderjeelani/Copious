using Copious.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Document.Interface.State
{
    public class DocumentMetadata : ValueObject<DocumentMetadata>
    {        
        public Guid SystemId { get; set; }
        public Guid SubSystemId { get; set; }        
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string RevisionCode { get; set; }
        public string Tags { get; set; }
        public string Author { get; set; }
    }
}
