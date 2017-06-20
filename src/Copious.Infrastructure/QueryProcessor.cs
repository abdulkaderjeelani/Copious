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

namespace Copious.Infrastructure
{
    internal sealed class QueryProcessor : IQueryProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private static readonly ConcurrentDictionary<Type, MethodInfo> SyncMethodsCache = new ConcurrentDictionary<Type, MethodInfo>();

        [DebuggerStepThrough]
        public TQueryResult Process<TQueryResult>(Query<TQueryResult> query)
        {
            var qryType = query.GetType();
            var qryResType = typeof(TQueryResult);

            var syncHandlerType = typeof(IQueryHandler<,>).MakeGenericType(qryType, qryResType);

            var syncHandler = (IQueryHandler)_serviceProvider.GetService(syncHandlerType);

            if (syncHandler == null)
                syncHandler = GetHandlerFromFactory(qryType, qryResType, false);

            if (syncHandler == null)
                throw new KeyNotFoundException("Handler not found, If async query is used call process async method");

            var queryType = query.GetType();
            if (!SyncMethodsCache.TryGetValue(queryType, out var fetch))
            {
                var method = syncHandler.GetType().GetMethod("Fetch", new Type[] { query.GetType() });
                fetch = SyncMethodsCache.AddOrUpdate(queryType, method, (t, m) => m);
            }

            return (TQueryResult)fetch.Invoke(syncHandler, new object[] { query });
        }

        [DebuggerStepThrough]
        public TQueryResult Process<TQuery, TQueryResult>(TQuery query) where TQuery : Query<TQueryResult>
        {
            var syncHandler = _serviceProvider.GetService<IQueryHandler<TQuery, TQueryResult>>();
            if (syncHandler == null)
                throw new KeyNotFoundException("Handler not found");

            return syncHandler.Fetch(query);
        }

        private static readonly ConcurrentDictionary<Type, MethodInfo> AsyncMethodsCache = new ConcurrentDictionary<Type, MethodInfo>();

        [DebuggerStepThrough]
        public async Task<TQueryResult> ProcessAsync<TQueryResult>(Query<TQueryResult> query)
        {
            var qryType = query.GetType();
            var qryResType = typeof(TQueryResult);

            var asyncHandlerType = typeof(IQueryHandlerAsync<,>).MakeGenericType(qryType, qryResType);

            var asyncHandler = (IQueryHandler)_serviceProvider.GetService(asyncHandlerType);

            if (asyncHandler == null)
                asyncHandler = GetHandlerFromFactory(qryType, qryResType, true);

            if (asyncHandler == null)
                throw new KeyNotFoundException("Handler not found");

            var queryType = query.GetType();
            if (!AsyncMethodsCache.TryGetValue(queryType, out var fetchAsync))
            {
                var method = asyncHandler.GetType().GetMethod("FetchAsync", new Type[] { query.GetType() });
                fetchAsync = AsyncMethodsCache.AddOrUpdate(queryType, method, (t, m) => m);
            }

            return await (Task<TQueryResult>)fetchAsync.Invoke(asyncHandler, new object[] { query });
        }

        [DebuggerStepThrough]
        public async Task<TQueryResult> ProcessAsync<TQuery, TQueryResult>(TQuery query) where TQuery : Query<TQueryResult>
        {
            var asyncHandler = _serviceProvider.GetService<IQueryHandlerAsync<TQuery, TQueryResult>>();
            if (asyncHandler == null)
                throw new KeyNotFoundException("Handler not found");

            return await asyncHandler.FetchAsync(query);
        }

        /// <summary>
        /// From all factories try to resolve handler
        /// </summary>
        /// <param name="qryType"></param>
        /// <param name="qryResType"></param>
        /// <param name="isAsync"></param>
        /// <returns></returns>
        private IQueryHandler GetHandlerFromFactory(Type qryType, Type qryResType, bool isAsync)
        {
            // As there will me multiple module and 1 context per module.
            var moduleHandlerFactories = _serviceProvider.GetServices<IQueryHandlerFactory>();

            foreach (var handlerFactory in moduleHandlerFactories)
            {
                var handler = (isAsync ? handlerFactory.GetAsyncHandlers(qryType, qryResType) :
                                                 handlerFactory.GetHandlers(qryType, qryResType)).FirstOrDefault();
                if (handler != null)
                    return handler;
            }

            return null;
        }
    }
}