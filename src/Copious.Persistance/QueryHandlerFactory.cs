using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Copious.Persistance.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Persistance
{
    public class QueryHandlerFactory : IQueryHandlerFactory
    {
        public const string ParameterName = "moduleQueryHandlerType";

        private readonly IServiceProvider _serviceProvider;
        private readonly Type _moduleQueryHandlerType;

        public QueryHandlerFactory(Type moduleQueryHandlerType, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            if (!moduleQueryHandlerType.GetTypeInfo().IsGenericType || moduleQueryHandlerType.GetTypeInfo().GetGenericArguments().Length != 1)
                throw new InvalidOperationException("Module Query handler must be a generic type with 1 arg.");

            if (!moduleQueryHandlerType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IQueryHandler)))
                throw new InvalidOperationException($"Module Query handler must implement {nameof(IQueryHandler)}.");

            _moduleQueryHandlerType = moduleQueryHandlerType;
        }

        public IEnumerable<IQueryHandler> GetAsyncHandlers(Type qryType, Type qryResType)
            => QueryHandlerResolver.GetQueryHandlerType(typeof(IQueryHandlerAsync<,>), _moduleQueryHandlerType, qryType, qryResType)
                .Select(h => ((IQueryHandler)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, h)));

        public IEnumerable<IQueryHandler> GetHandlers(Type qryType, Type qryResType)
            => QueryHandlerResolver.GetQueryHandlerType(typeof(IQueryHandler<,>), _moduleQueryHandlerType, qryType, qryResType)
                .Select(h => ((IQueryHandler)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, h)));
    }
}