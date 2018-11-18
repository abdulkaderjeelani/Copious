using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Identity {
    public static class IdentityExtensions {
        public static string ToReadable (this IEnumerable<IdentityError> errors) {
            var errorBuilder = new StringBuilder ();
            foreach (IdentityError error in errors)
                errorBuilder.AppendLine ($"{error.Code}, {error.Description}");

            return errorBuilder.ToString ();
        }
    }
}