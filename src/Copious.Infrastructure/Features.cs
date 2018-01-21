using System;
using System.IO;
using System.Reflection;
using System.Xml;

using AutoMapper;

using Copious.Infrastructure.Interface;
using Copious.Infrastructure.Interface.Services;
using Copious.Infrastructure.Scheduler;

using Hangfire;
using Hangfire.PostgreSql;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

namespace Copious.Infrastructure
{
    /// <summary>
    /// Provides helper methods to add infrastructure services,
    /// </summary>
    public static class Features
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            AddScheduler(services);
        }

        public static void Configure(IConfigurationRoot configuration, IApplicationBuilder app, IServiceProvider serviceProvider, ILoggerFactory loggerFactory, bool isDevelopmentEnv)
        {
            UseLogging(configuration, loggerFactory, isDevelopmentEnv);
            UseScheduling(app, serviceProvider);
        }

        public static void Init(IServiceCollection services, IConfigurationRoot configuration)
        {
            //add IConfigurationRoot to the di container
            services.Add(new ServiceDescriptor(typeof(IConfigurationRoot), provider => configuration, ServiceLifetime.Singleton));
            services.AddSingleton<IConfiguration>(configuration);

            switch (CopiousConfiguration.Config.Mapper)
            {
                case Interface.Mapper.Mapster:
                    services.AddScoped<Interface.IMapper, Mappers.Mapster>();
                    break;
                case Interface.Mapper.Automapper:
                default:
                    // https://stackoverflow.com/questions/40275195/how-to-setup-automapper-in-asp-net-core
                    // https://lostechies.com/jimmybogard/2016/07/20/integrating-automapper-with-asp-net-core-di/
                    services.AddAutoMapper();
                    services.AddScoped<Interface.IMapper, Mappers.AutoMapper>();
                    break;
            }


        }

        public static void AddCache(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
        }

        public static void AddScheduler(IServiceCollection services)
        {
            if (!CopiousConfiguration.Config.EnableScheduler) return;

            switch (CopiousConfiguration.Config.Scheduler)
            {
                case Interface.Scheduler.Hangfire:
                default:
                    AddHangFireScheduler();
                    break;
            }

            void AddHangFireScheduler()
            {
                services.AddHangfire(config =>
                {
                    config.UsePostgreSqlStorage(CopiousConfiguration.Config.SchedulerPostgreSqlDb);
                    config.UseColouredConsoleLogProvider();
                });
                services.AddSingleton<IScheduler, HangfireScheduler>();
            }
        }

        public static void UseLogging(IConfigurationRoot configuration, ILoggerFactory loggerFactory, bool isDevelopmentEnv)
        {
            if (isDevelopmentEnv)
            {
                loggerFactory.AddConsole(configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }

            switch (CopiousConfiguration.Config.LoggingProvider)
            {
                case LoggingProvider.Serilog:
                    loggerFactory.AddSerilog();
                    break;

                case LoggingProvider.Log4Net:
                default:
                    AddLog4Net();
                    break;
            }

            void AddLog4Net()
            {
                var log4NetConfig = new XmlDocument();
                using (var reader = new StreamReader(new FileStream("log4net.config", FileMode.Open, FileAccess.Read)))
                {
                    log4NetConfig.Load(reader);
                }

                var rep = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                log4net.Config.XmlConfigurator.Configure(rep, log4NetConfig["log4net"]);
                loggerFactory.AddProvider(new Log4NetProvider());
            }
        }

        public static void UseScheduling(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            if (!CopiousConfiguration.Config.EnableScheduler) return;
            switch (CopiousConfiguration.Config.Scheduler)
            {
                case Interface.Scheduler.Hangfire:
                default:
                    // add our activator to the GlobalConfiguration
                    GlobalConfiguration.Configuration.UseActivator(new ServiceProviderActivator(serviceProvider));
                    app.UseHangfireServer();
                    app.UseHangfireDashboard();
                    break;
            }
        }
    }
}