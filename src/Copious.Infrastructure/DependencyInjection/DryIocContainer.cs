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
        readonly IServiceCollection _services;

        public DryIocContainer(IServiceCollection services)
        {
            _services = services;
            Container = DryIocIContainer = new Container(Rules.Default.WithImplicitRootOpenScope()
                .With(FactoryMethod.ConstructorWithResolvableArgumentsIncludingNonPublic));
        }

        public IContainer DryIocIContainer { get; set; }

        public bool HasAssemblyScanningSupport => true;

        public IServiceProvider GetServiceProvider()
        {
            DryIocIContainer = DryIocIContainer.WithDependencyInjectionAdapter(_services);
            return DryIocIContainer.Resolve<IServiceProvider>();
        }

        public void Register<TClass>()
            => DryIocIContainer.Register<TClass>();

        public void Register<TClass, TInterface>() where TClass : TInterface
            => DryIocIContainer.Register<TInterface, TClass>(); // made: FactoryMethod.ConstructorWithResolvableArgumentsIncludingNonPublic

        public void Register<TClass, TInterface, TExcplicitParameterType>(string explicitParameterName, TExcplicitParameterType explicitparameterValue) where TClass : TInterface
            => DryIocIContainer.Register<TInterface, TClass>(made: Parameters.Of.Name(explicitParameterName, req => explicitparameterValue));

        public void Register<TClass, TInterface, TExcplicitParameterType>(string explicitParameterName, Func<TExcplicitParameterType> explicitparameterValueRetriever) where TClass : TInterface
            => DryIocIContainer.Register<TInterface, TClass>(made: Parameters.Of.Name(explicitParameterName, req => explicitparameterValueRetriever.Invoke()));

        public void RegisterAssemblyTypes(Assembly assembly, Func<Type, bool> typeFilter = null)
            => DryIocIContainer.RegisterMany(new Assembly[] { assembly }, typeFilter);

        public void RegisterGeneric(Type implementationType, Type definitionType)
            => DryIocIContainer.Register(definitionType, implementationType);

        public void RegisterInstance<TInterface>(TInterface instance) where TInterface : class
            => DryIocIContainer.RegisterInstance<TInterface>(instance);
    }
}