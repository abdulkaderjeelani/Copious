using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Copious.Foundation {
    public class ErrorCode {
        public ErrorCode (int code, string description) {
            Code = code;
            Description = description;
        }

        public int Code { get; set; }
        public string Description { get; set; }

        public ErrorCode AddInfo (string info) {
            Description = $"{Description} {info}";
            return this;
        }

        public EventId ToEventId () => new EventId (Code, Description);
    }
}