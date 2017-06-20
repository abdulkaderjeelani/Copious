using Copious.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Document.Interface.State
{
    public class DocumentSecurity : ValueObject<DocumentSecurity>
    {
        public bool IsEncrypted { get; set; }
    }
}