using System;
using Copious.Application.Interface;
using Copious.Infrastructure.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Application
{
    public sealed class Registrator : IRegistrator
    {
        public void RegisterDependancies(IConfigurationRoot configuration, IServiceCollection services)
        {
            services.AddSingleton<ICommandHandlerFactory, CommandHandlerFactory>();
            services.AddSingleton<IEventHandlerFactory, EventHandlerFactory>();
            services.AddScoped<ICommandGuard, CommandGuard>();
        }

        public void RegisterDependancies(IConfigurationRoot configuration, IContainer container, IServiceProvider serviceProvider)
        {
            // intentionally left empty
        }
    }
}