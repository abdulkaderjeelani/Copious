using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Copious.Foundation {
    public interface Identifiable<T> {
        /// <summary>
        /// Case insensitive identifier
        /// </summary>
        T Id { get; set; }
    }

    public static class IdentifiableExtensions {
        public static bool Match (this Identifiable<string> identifiable, string identity) => string.Equals (identity, identifiable.Id, StringComparison.OrdinalIgnoreCase);

    }
}