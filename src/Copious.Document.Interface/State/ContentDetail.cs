using System;
using System.Collections.Generic;
using System.Text;
using Copious.Foundation;

namespace Copious.Document.Interface.State {
    /// <summary>
    /// Contains general / open dcument info of a doc if kind is general
    /// </summary>
    public class ContentDetail : DocumentDetail {
        public string Content { get; set; }

        public List<File> ResourceFiles { get; set; }

        public string ContentType { get; set; }
    }
}