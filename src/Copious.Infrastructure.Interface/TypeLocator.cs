using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.DependencyModel;

namespace Copious.Infrastructure.Interface
{
    public static class TypeLocator
    {
        public static List<Type> GetGenericImplementor<T>(Type interfaceType)
            => GetAssemblies().SelectMany(ass => ass.GetExportedTypes()
                                .Where(x => !x.GetTypeInfo().IsAbstract && x.GetInterfaces()
                                    .Any(a => a.GetTypeInfo().IsGenericType && a.GetGenericTypeDefinition() == interfaceType))
                                .Where(h => h.GetInterfaces()
                                    .Any(ii => ii.GetGenericArguments()
                                        .Any(aa => aa == typeof(T))))).ToList();

        public static List<Type> GetGenericImplementor(Type interfaceType, Type t1Type, Type t2Type)
            => GetAssemblies().SelectMany(ass => ass.GetExportedTypes()
                                .Where(x => !x.GetTypeInfo().IsAbstract && x.GetInterfaces()
                                    .Any(a => a.GetTypeInfo().IsGenericType && a.GetGenericTypeDefinition() == interfaceType))
                                .Where(h => h.GetInterfaces()
                                    .Any(ii =>
                                    {
                                        var args = ii.GetGenericArguments();
                                        return args.Length >= 2 && args[0] == t1Type && args[1] == t2Type;
                                    }))).ToList();

        public static bool CheckGenericParameterOfType(Type tType, Type[] checkTypes)
            => tType.GetTypeInfo().IsGenericType && checkTypes.Any(t => t == tType.GetGenericTypeDefinition());

        private static List<Assembly> _assemblies;

        private static IEnumerable<Assembly> GetAssemblies()
        {
            if (_assemblies == null)
            {
                var runtimeId = RuntimeEnvironment.GetRuntimeIdentifier();
                var runtimeAssemblies = DependencyContext.Default.GetRuntimeAssemblyNames(runtimeId);
                foreach (var appAss in CopiousConfiguration.Config.AppAssemblyPrefixes)
                    runtimeAssemblies = runtimeAssemblies.Where(a => a.Name.StartsWith(appAss, StringComparison.OrdinalIgnoreCase));

                _assemblies = runtimeAssemblies.Select(a => Assembly.Load(a)).ToList();
            }
            return _assemblies;
        }
    }
}