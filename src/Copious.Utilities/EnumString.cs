using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Copious.Utilities
{
    public class EnumString<T> where T : struct, IConvertible
    {
        public string StringValue { get; set; }
        public T EnumValue { get; set; }

        public static List<EnumString<T>> CreateEnumList()
        {
            var tType = typeof(T);
            if (!tType.GetTypeInfo().IsEnum) throw new ArgumentException("T must be an enum");

            return Enum.GetNames(tType).Select(gc =>
                new EnumString<T>
                {
                    StringValue = gc,
                    EnumValue = (T)System.Enum.Parse(tType, gc)
                }).ToList();
        }
    }
}