using System;

namespace Copious.Utilities {
    [Serializable]
    public class EntityMapperException : Exception {
        public EntityMapperException () : base () { }

        public EntityMapperException (string message) : base (message) { }

        public EntityMapperException (string message, Exception innerException) : base (message, innerException) { }
    }
}