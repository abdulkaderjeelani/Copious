using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Foundation
{
    public class Context
    {
        public IDictionary<object, object> Items { get; set; }

        public Action Abort { get; set; }

        public Actor Actor { get; set; }
        
    }
}