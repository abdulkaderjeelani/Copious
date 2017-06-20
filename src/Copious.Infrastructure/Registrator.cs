using System;
using Copious.Infrastructure.Interface;
using Copious.Infrastructure.Interface.Services;
using Copious.Infrastructure.Scheduler;
using Copious.Infrastructure.Security;
using Copious.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Infrastructure
{
    public class Registrator : IRegistrator
    {
        public void RegisterDependancies(IConfigurationRoot configuration, IServiceCollection services)
        {
            services.AddTransient<IExceptionHandler, ExceptionHandler>();
            services.AddSingleton<ITextSerializer, JsonTextSerializer>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddTransient<IEventBus, EventBus>();
            services.AddTransient<ICommandBus, CommandBus>();
            services.AddTransient<IQueryProcessor, QueryProcessor>();
            services.AddScoped<ISecurityProvider, SecurityProvider>();
            
        }

        public void RegisterDependancies(IConfigurationRoot configuration, IContainer container)
        {
            // intentionally left empty
        }
        
    }
}