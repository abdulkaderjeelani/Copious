using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Copious.Utilities {
    public static class TaskExtensions {
        public static Task ForEachAsync<T> (this IEnumerable<T> source, Func<T, Task> body) => Task.WhenAll (from item in source select Task.Run (() => body?.Invoke (item)));
    }
}