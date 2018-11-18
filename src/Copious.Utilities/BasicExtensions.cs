using System;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Copious.Utilities {
    public static class BasicExtensions {
        public static bool EqualsInsensitive (this string actual, string compare) => actual.Equals (compare, StringComparison.OrdinalIgnoreCase);

        public static bool AllMatch<T> (this T[] source, T[] compare) => source.Length == compare.Length && source.All (s => compare.Contains (s));

        /// <summary>
        /// Gets the given CamelCaseString as space separated words
        /// </summary>
        /// <param name="camelCaseString">String to be converted without space between words but uppercased first letters</param>
        /// <returns>Converted string with spaces between works</returns>
        public static string GetSpacedString (this string camelCaseString) => Regex.Replace (Regex.Replace (camelCaseString, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
    }
}