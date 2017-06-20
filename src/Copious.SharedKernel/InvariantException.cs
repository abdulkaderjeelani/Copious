using System;
using System.Collections.Generic;

namespace Copious.SharedKernel
{
    [Serializable]
    public class InvariantException : Exception
    {
        public bool IsAggregated { get; set; } = false;
        public IEnumerable<InvariantException> InnerExceptions { get; set; } = null;

        public InvariantException(string message) : base(message)
        {
        }

        public InvariantException(IEnumerable<InvariantException> ivExs)
        {
            IsAggregated = true;
            InnerExceptions = ivExs;
        }
    }

    public enum InvariantEnforcementStyle
    {
        FailFast,
        Collect,
    }
}