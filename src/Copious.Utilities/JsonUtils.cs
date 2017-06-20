using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Utilities
{
    public static class JsonUtils
    {
        public static string From<T>(T val) where T:class
        {
            if (val == null) return null;

            return Newtonsoft.Json.JsonConvert.SerializeObject(val);
        }

        public static T To<T>(string json) where T : class
        {
            if (string.IsNullOrWhiteSpace(json)) return null;

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}