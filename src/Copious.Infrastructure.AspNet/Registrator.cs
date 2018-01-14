using System;

using Copious.Infrastructure.Interface;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Infrastructure.AspNet
{
    public class Registrator : IRegistrator
    {
        public void RegisterDependancies(IConfigurationRoot configuration, IServiceCollection services)
        {
            services.AddScoped<IContextProvider, AspNetContextProvider>();
            services.AddScoped<Interface.AspNet.IAntiForgeryValidator, AntiForgeryValidator>();
        }

        public void RegisterDependancies(IConfigurationRoot configuration, IContainer container, IServiceProvider serviceProvider)
        {
            // intentionally left empty
        }
    }
}