using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Infrastructure.Interface {
    public interface IRegistrator {
        /// <summary>
        /// Called first to add registrations into servicecollection, use this method if needed to use default di features of aspnet
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="services"></param>
        void RegisterDependancies (IConfigurationRoot configuration, IServiceCollection services);

        /// <summary>
        /// Called next, to add the registrations into container, use this method to use features provided by the framework using various di containers
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="container"></param>
        /// <param name="serviceProvider">Provider built with seviceCollections</param>
        void RegisterDependancies (IConfigurationRoot configuration, IContainer container, IServiceProvider serviceProvider);
    }
}