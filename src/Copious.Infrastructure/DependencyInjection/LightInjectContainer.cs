using System;
using System.Reflection;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Copious.Infrastructure.DependencyInjection;

namespace Copious.Infrastructure
{
    public class LightInjectContainer : DependancyContainer, Interface.IContainer
    {
        private readonly IServiceCollection _services;

        public LightInjectContainer(IServiceCollection services)
        {
            _services = services;
            Container = _LightInjectContainer = new ServiceContainer();
        }

        public bool HasAssemblyScanningSupport => true;

        private ServiceContainer _LightInjectContainer { get; set; }

        public IServiceProvider GetServiceProvider()
            => _LightInjectContainer.CreateServiceProvider(_services);

        public void Register<TClass>()
            => _LightInjectContainer.Register<TClass>();

        public void Register<TClass, TInterface>() where TClass : TInterface
            => _LightInjectContainer.Register<TInterface, TClass>();

        public void Register<TClass, TInterface, TExcplicitParameterType>(string explicitParameterName, TExcplicitParameterType explicitparameterValue) where TClass : TInterface
        => _LightInjectContainer.Register<TInterface, TClass>()
                .RegisterConstructorDependency(
                    (fac, paraminfo) =>
                    {
                        if (paraminfo.Name == explicitParameterName) /* If there are multiple parameter names for the same type then control here*/
                            return explicitparameterValue;

                        return default(TExcplicitParameterType);
                    });

        public void Register<TClass, TInterface, TExcplicitParameterType>(string explicitParameterName, Func<TExcplicitParameterType> explicitparameterValueRetriever) where TClass : TInterface
            => _LightInjectContainer.Register<TInterface, TClass>()
                .RegisterConstructorDependency(
                    (fac, paraminfo) =>
                    {
                        if (paraminfo.Name == explicitParameterName) /* If there are multiple parameter names for the same type then control here*/
                            return explicitparameterValueRetriever.Invoke();

                        return default(TExcplicitParameterType);
                    });

        public void RegisterAssemblyTypes(Assembly assembly, Func<Type, bool> typeFilter = null)
            => _LightInjectContainer.RegisterAssembly(assembly, (serviceType, implementingType) => typeFilter?.Invoke(implementingType) ?? true);

        public void RegisterGeneric(Type implementationType, Type definitionType)
            => _LightInjectContainer.Register(definitionType, implementationType);

        public void RegisterInstance<TInterface>(TInterface instance) where TInterface : class
            => _LightInjectContainer.RegisterInstance(instance);
    }
}