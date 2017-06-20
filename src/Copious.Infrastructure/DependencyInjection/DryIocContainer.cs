using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Infrastructure.DependencyInjection
{
    // https://bitbucket.org/dadhi/dryioc/wiki/Home#markdown-header-usage-guide
    public class DryIocContainer : DependancyContainer, Interface.IContainer
    {
        private readonly IServiceCollection _services;

        public DryIocContainer(IServiceCollection services)
        {
            _services = services;
            Container = _DryIocContainer = new Container(Rules.Default.WithImplicitRootOpenScope()
                .With(FactoryMethod.ConstructorWithResolvableArgumentsIncludingNonPublic));
        }

        public IContainer _DryIocContainer { get; set; }

        public bool HasAssemblyScanningSupport => true;

        public IServiceProvider GetServiceProvider()
        {
            _DryIocContainer = _DryIocContainer.WithDependencyInjectionAdapter(_services);
            return _DryIocContainer.Resolve<IServiceProvider>();
        }

        public void Register<TClass>()
            => _DryIocContainer.Register<TClass>();

        public void Register<TClass, TInterface>() where TClass : TInterface
            => _DryIocContainer.Register<TInterface, TClass>(); // made: FactoryMethod.ConstructorWithResolvableArgumentsIncludingNonPublic

        public void Register<TClass, TInterface, TExcplicitParameterType>(string explicitParameterName, TExcplicitParameterType explicitparameterValue) where TClass : TInterface
            => _DryIocContainer.Register<TInterface, TClass>(made: Parameters.Of.Name(explicitParameterName, req => explicitparameterValue));

        public void RegisterAssemblyTypes(Assembly assembly, Func<Type, bool> typeFilter = null)
            => _DryIocContainer.RegisterMany(new Assembly[] { assembly }, typeFilter);

        public void RegisterGeneric(Type implementationType, Type definitionType)
            => _DryIocContainer.Register(definitionType, implementationType);

        public void RegisterInstance<TInterface>(TInterface instance) where TInterface : class
            => _DryIocContainer.RegisterInstance<TInterface>(instance);
    }
}