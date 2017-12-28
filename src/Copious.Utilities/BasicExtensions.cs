using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Utilities
{
    public static class BasicExtensions
    {
        public static bool EqualsInsensitive(this string actual, string compare) => actual.Equals(compare, StringComparison.OrdinalIgnoreCase);

        public static bool AllMatch<T>(this T[] source, T[] compare) => source.Length == compare.Length && source.All(s => compare.Contains(s));
    }
}
