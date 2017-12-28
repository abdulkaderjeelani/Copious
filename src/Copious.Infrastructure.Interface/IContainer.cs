using System;
using System.Reflection;

namespace Copious.Infrastructure.Interface
{
    /// <summary>
    /// Abstraction layer for Container registrations
    /// </summary>
    public interface IContainer : IDisposable
    {
        IServiceProvider GetServiceProvider();

        bool HasAssemblyScanningSupport { get; }

        void Register<TClass, TInterface>() where TClass : TInterface;

        /// <summary>
        /// The explicit parameter must be first parameter for the implementing class
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TExcplicitParameterType"></typeparam>
        /// <param name="explicitParameterName"></param>
        /// <param name="explicitparameterValue"></param>
        void Register<TClass, TInterface, TExcplicitParameterType>(string explicitParameterName, TExcplicitParameterType explicitparameterValue) where TClass : TInterface;

        /// <summary>
        /// The explicit parameter must be first parameter for the implementing class,
        /// The provided delegate is lazily invoked by the implementing DI provider (currently supported: Autofac, LightInject, DryIOC)
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TExcplicitParameterType"></typeparam>
        /// <param name="explicitParameterName"></param>
        /// <param name="explicitparameterValueRetriever"></param>
        void Register<TClass, TInterface, TExcplicitParameterType>(string explicitParameterName, Func<TExcplicitParameterType> explicitparameterValueRetriever) where TClass : TInterface;

        void Register<TClass>();

        void RegisterGeneric(Type implementationType, Type definitionType);

        /// <summary>
        /// Any implementd types with <see cref="HasAssemblyScanningSupport"/>
        /// Must implmenent this method, if not throws NotSupportedException
        /// Caller should check for  <see cref="HasAssemblyScanningSupport"/>
        /// before calling this sub routine
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="typeFilter"></param>
        void RegisterAssemblyTypes(Assembly assembly, Func<Type, bool> typeFilter = null);

        void RegisterInstance<TInterface>(TInterface instance) where TInterface : class;
    }
}