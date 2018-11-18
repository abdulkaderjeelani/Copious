using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Foundation {
    public class RequestContext {
        public IDictionary<object, object> Items { get; set; }

        public Action AbortRequest { get; set; }

        public Actor Actor { get; set; }

    }
}