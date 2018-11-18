using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Copious.Foundation;
using Copious.Infrastructure.Interface;
using Copious.Persistance.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Infrastructure {
    internal sealed class QueryProcessor : IQueryProcessor {
        readonly IServiceProvider _serviceProvider;

        public QueryProcessor (IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        static readonly ConcurrentDictionary<Type, MethodInfo> SyncMethodsCache = new ConcurrentDictionary<Type, MethodInfo> ();

        public TQueryResult Process<TQuery, TQueryResult> (TQuery query) where TQuery : Query => Process<TQuery, TQueryResult> (query, default (string));

        static readonly Func<object, string, bool> handlerIdentityPredicate = (h, handlerIdentity) => !string.IsNullOrEmpty (handlerIdentity) && (h is Identifiable<string> i) && i.Match (handlerIdentity);

        [DebuggerStepThrough]
        public TQueryResult Process<TQuery, TQueryResult> (TQuery query, string handlerIdentity) where TQuery : Query {
            var qryType = typeof (TQuery);
            var qryResType = typeof (TQueryResult);

            var syncHandlerType = typeof (IQueryHandler<,>).MakeGenericType (qryType, qryResType);

            var syncHandler = (IQueryHandler<TQuery, TQueryResult>) _serviceProvider.GetService (syncHandlerType);

            if (syncHandler == null)
                syncHandler = _serviceProvider.GetService<IQueryHandlerFactory> ()
                .GetHandlers<TQuery, TQueryResult> () ?
                .Where (h => handlerIdentityPredicate (h, handlerIdentity)) ?
                .FirstOrDefault ();

            if (syncHandler == null)
                throw new KeyNotFoundException ("Handler not found, If async query is used call process async method");

            var queryType = query.GetType ();
            if (!SyncMethodsCache.TryGetValue (queryType, out var fetch)) {
                var method = syncHandler.GetType ().GetMethod ("Fetch", new Type[] { query.GetType () });
                fetch = SyncMethodsCache.AddOrUpdate (queryType, method, (t, m) => m);
            }

            return (TQueryResult) fetch.Invoke (syncHandler, new object[] { query });
        }

        static readonly ConcurrentDictionary<Type, MethodInfo> AsyncMethodsCache = new ConcurrentDictionary<Type, MethodInfo> ();

        public async Task<TQueryResult> ProcessAsync<TQuery, TQueryResult> (TQuery query) where TQuery : Query => await ProcessAsync<TQuery, TQueryResult> (query, default (string));

        [DebuggerStepThrough]
        public async Task<TQueryResult> ProcessAsync<TQuery, TQueryResult> (TQuery query, string handlerIdentity) where TQuery : Query {
            var qryType = typeof (TQuery);
            var qryResType = typeof (TQueryResult);

            var asyncHandlerType = typeof (IQueryHandlerAsync<,>).MakeGenericType (qryType, qryResType);

            var asyncHandler = (IQueryHandlerAsync<TQuery, TQueryResult>) _serviceProvider.GetService (asyncHandlerType);

            if (asyncHandler == null)
                asyncHandler = _serviceProvider.GetService<IQueryHandlerFactory> ()
                .GetAsyncHandlers<TQuery, TQueryResult> () ?
                .Where (h => handlerIdentityPredicate (h, handlerIdentity)) ?
                .FirstOrDefault ();

            if (asyncHandler == null)
                throw new KeyNotFoundException ("Handler not found");

            var queryType = query.GetType ();
            if (!AsyncMethodsCache.TryGetValue (queryType, out var fetchAsync)) {
                var method = asyncHandler.GetType ().GetMethod ("FetchAsync", new Type[] { query.GetType () });
                fetchAsync = AsyncMethodsCache.AddOrUpdate (queryType, method, (t, m) => m);
            }

            return await (Task<TQueryResult>) fetchAsync.Invoke (asyncHandler, new object[] { query });
        }

    }
}