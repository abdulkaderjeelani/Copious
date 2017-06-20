using System;
using Copious.Infrastructure.Interface;
using Copious.Persistance.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Persistance
{
    public class Registrator : IRegistrator
    {
        public void RegisterDependancies(IConfigurationRoot configuration, IServiceCollection services)
        {
            //Looking for Query Hanldr Factory registration? Its in modules persistannce registrator as it depenends on repository
            services.AddScoped<IQueryGuard, QueryGuard>();
        }

        public void RegisterDependancies(IConfigurationRoot configuration, IContainer container)
        {
            // Method intentionally left empty. Remove this comment when adding registrations.
        }
    }
}