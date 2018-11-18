using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Copious.Utilities;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.DependencyModel;

namespace Copious.Infrastructure.Interface {
    public static class TypeLocator {
        public static List<Type> GetGenericImplementor<T> (Type interfaceType) => GetGenericImplementor (interfaceType, typeof (T));

        public static List<Type> GetGenericImplementor<T1, T2> (Type interfaceType) => GetGenericImplementor (interfaceType, typeof (T1), typeof (T2));

        static List<Type> GetGenericImplementor (Type interfaceType, params Type[] intrfaceTypeArguments) => GetAssemblies ().SelectMany (ass => ass.GetExportedTypes ()
            .Where (x => !x.GetTypeInfo ().IsAbstract && x.GetInterfaces ()
                .Any (a => a.GetTypeInfo ().IsGenericType && a.GetGenericTypeDefinition () == interfaceType))
            .Where (h => h.GetInterfaces ()
                .Any (ii => intrfaceTypeArguments.AllMatch (ii.GetGenericArguments ()))
            )).ToList ();

        public static bool CheckGenericParameterOfType (Type tType, Type[] checkTypes) => tType.GetTypeInfo ().IsGenericType && checkTypes.Any (t => t == tType.GetGenericTypeDefinition ());

        static List<Assembly> _assemblies;

        static IEnumerable<Assembly> GetAssemblies () {
            if (_assemblies == null) {
                var runtimeId = RuntimeEnvironment.GetRuntimeIdentifier ();
                var runtimeAssemblies = DependencyContext.Default.GetRuntimeAssemblyNames (runtimeId);
                foreach (var appAss in CopiousConfiguration.Config.AppAssemblyPrefixes)
                    runtimeAssemblies = runtimeAssemblies.Where (a => a.Name.StartsWith (appAss, StringComparison.OrdinalIgnoreCase));

                _assemblies = runtimeAssemblies.Select (a => Assembly.Load (a)).ToList ();
            }
            return _assemblies;
        }
    }
}