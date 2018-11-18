using System;
using System.Collections.Generic;
using System.Text;
using Copious.Foundation;

namespace Copious.Document.Interface.State {
    public class DocumentSecurity : ValueObject<DocumentSecurity> {
        public bool IsEncrypted { get; set; }
    }
}