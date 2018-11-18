using System;
using System.Collections.Generic;
using System.Text;
using Copious.Foundation;

namespace Copious.Infrastructure.Interface {
    public interface IContextProvider {
        RequestContext Context { get; }
    }

    public static class ContextProviderExtensions {
        public static Func<RequestContext> Fn (this IContextProvider contextProvider) => () => contextProvider.Context;

    }
}