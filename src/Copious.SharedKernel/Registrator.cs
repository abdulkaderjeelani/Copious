using System;
using Copious.Infrastructure.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.SharedKernel
{
    public sealed class Registrator : IRegistrator
    {
        public void RegisterDependancies(IConfigurationRoot configuration, IServiceCollection services)
        {
            services.AddSingleton<IRuleAssessor, RuleAssessor>();
        }

        public void RegisterDependancies(IConfigurationRoot configuration, IContainer container, IServiceProvider serviceProvider)
        {
            // Method intentionally left empty. Remove this comment when adding registrations.
        }
    }
}