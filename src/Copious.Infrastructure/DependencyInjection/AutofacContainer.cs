using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Copious.Infrastructure.DependencyInjection;
using Copious.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Infrastructure {
    /*
    References for multi tenancy support in autofac
    https://github.com/SaltyDH/AutofacMultitenancy1/blob/master/src/AutofacMultitenancy1/Startup.cs
    https://stackoverflow.com/questions/38940241/autofac-multitenant-in-an-aspnet-core-application-does-not-seem-to-resolve-tenan
    */

    public class AutofacContainer : DependancyContainer, Interface.IContainer {
        readonly ContainerBuilder _builder;

        public AutofacContainer (IServiceCollection services) {
            // Wire up autofac as DI container
            _builder = new ContainerBuilder ();
            _builder.Populate (services);
        }

        public bool HasAssemblyScanningSupport => true;
        IContainer AutofacIContainer { get; set; }

        public IServiceProvider GetServiceProvider () {
            Container = AutofacIContainer = _builder.Build ();
            return new AutofacServiceProvider (AutofacIContainer);
        }

        public void Register<TClass, TInterface> () where TClass : TInterface => _builder.RegisterType<TClass> ().As<TInterface> ();

        public void Register<TClass, TInterface, TExcplicitParameterType> (string explicitParameterName, TExcplicitParameterType explicitparameterValue) where TClass : TInterface => _builder.RegisterType<TClass> ().As<TInterface> ().WithParameter (explicitParameterName, explicitparameterValue);

        public void Register<TClass, TInterface, TExcplicitParameterType> (string explicitParameterName, Func<TExcplicitParameterType> explicitparameterValueRetriever) where TClass : TInterface => _builder.RegisterType<TClass> ().As<TInterface> ().WithParameter (
            (pi, ctx) => pi.Name.EqualsInsensitive (explicitParameterName),
            (pi, ctx) => pi.Name.EqualsInsensitive (explicitParameterName) ? explicitparameterValueRetriever.Invoke () : default (object));

        public void Register<TClass> () => _builder.RegisterType<TClass> ();

        public void RegisterAssemblyTypes (Assembly assembly, Func<Type, bool> typeFilter = null) {
            if (typeFilter != null)
                _builder.RegisterAssemblyTypes (assembly)
                .Where (typeFilter)
                .AsImplementedInterfaces ();
            else
                _builder.RegisterAssemblyTypes (assembly)
                .AsImplementedInterfaces ();
        }

        public void RegisterGeneric (Type implementationType, Type definitionType) => _builder.RegisterGeneric (implementationType).As (definitionType);

        public void RegisterInstance<TInterface> (TInterface instance) where TInterface : class => _builder.RegisterInstance (instance);
    }
}