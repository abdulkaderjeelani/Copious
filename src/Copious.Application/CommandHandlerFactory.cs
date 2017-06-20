using Copious.Application.Interface;
using Copious.Foundation;
using Copious.Infrastructure.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Copious.Application
{
    public sealed class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IEnumerable<ICommandHandlerAsync<T>> GetAsyncHandlers<T>() where T : Command
           => MessageHandlerResolver.GetCommandHandlerType<T>(typeof(ICommandHandlerAsync<>))
              .Select(h => (ICommandHandlerAsync<T>)GetCommandHandlerInstance<T>(h));

        public IEnumerable<ICommandHandler<T>> GetHandlers<T>() where T : Command
            => MessageHandlerResolver.GetCommandHandlerType<T>(typeof(ICommandHandler<>))
              .Select(h => (ICommandHandler<T>)GetCommandHandlerInstance<T>(h));

        private object GetCommandHandlerInstance<T>(Type h) where T : Command
        => (typeof(CrudCommandHandler<,,>).Name == h.Name) ? GetCrudCommandHandlerInstance(h) : ActivatorUtilities.CreateInstance(_serviceProvider, h);

        private static readonly ConcurrentDictionary<string, Type> GenericRepositoriesCache = new ConcurrentDictionary<string, Type>();

        /// <summary>
        /// Finds the module of the commmand
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="h"></param>
        /// <returns></returns>
        private object GetCrudCommandHandlerInstance(Type h)
        {
            var stateType = h.GetGenericArguments().Last();
            var commandAssembly = stateType.GetTypeInfo().Assembly.ManifestModule.Name.Split('.');
            var prefix = commandAssembly[0];
            var moduleName = commandAssembly[1];
            var repoTypeName = $"I{moduleName}Repository";
            var key = $"{repoTypeName}-{stateType}";

            if (!GenericRepositoriesCache.TryGetValue(key, out Type genericRepositoryType))
            {
                var repoAssembly = Assembly.Load(new AssemblyName($"{prefix}.{moduleName}.Persistance.Interface"));
                var repoType = repoAssembly.GetExportedTypes().FirstOrDefault(t => t.Name.StartsWith(repoTypeName, StringComparison.OrdinalIgnoreCase));

                if (repoType == null || !repoType.GetTypeInfo().IsGenericType)
                    throw new KeyNotFoundException($"Default repository with name {repoTypeName} not found for module {moduleName} in assembly {repoAssembly.FullName}.");

                genericRepositoryType = GenericRepositoriesCache.AddOrUpdate(key, repoType.MakeGenericType(stateType), (t, m) => m);
            }

            return ActivatorUtilities.CreateInstance(_serviceProvider, h, _serviceProvider.GetService(genericRepositoryType));
        }
    }
}