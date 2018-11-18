using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Copious.Utilities {
    public static class EnumExtensions {
        public static T Include<T> (this Enum type, T value) where T : struct => (T) (ValueType) (((int) (ValueType) type | (int) (ValueType) value));

        public static T Remove<T> (this Enum type, T value) where T : struct => (T) (ValueType) (((int) (ValueType) type & ~(int) (ValueType) value));

        public static bool Has<T> (this Enum type, T value) where T : struct => (((int) (ValueType) type & (int) (ValueType) value) == (int) (ValueType) value);

        public static T ToEnum<T> (this long val) => val.ToString ().ToEnum<T> ();

        public static T ToEnum<T> (this int val) => val.ToString ().ToEnum<T> ();

        public static T ToEnum<T> (this string val) => (T) Enum.Parse (typeof (T), val, true);

        public static string ToDescriptionString<T> (this T val) where T : struct {
            var attributes = (DescriptionAttribute[]) val.GetType ().GetTypeInfo ().GetField (val.ToString ()).GetCustomAttributes (typeof (DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        /// <summary>
        /// Get the collection of values of Enum
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<T> GetValues<T> () => Enum.GetValues (typeof (T)).Cast<T> ();
    }
}