using Copious.Infrastructure.Interface;
using Copious.Infrastructure.Interface.Services;
using Copious.Infrastructure.Scheduler;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Copious.Infrastructure
{
    public static class CrossConcerns
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            if (CopiousConfiguration.Config.EnableScheduler)
                switch (CopiousConfiguration.Config.Scheduler)
                {
                    case Interface.Scheduler.Hangfire:
                    default:
                        services.AddHangfire(config =>
                        {
                            config.UsePostgreSqlStorage(CopiousConfiguration.Config.SchedulerPostgreSqlDb);
                            config.UseColouredConsoleLogProvider();
                        });
                        services.AddSingleton<IScheduler, HangfireScheduler>();
                        break;
                }

        }

        public static void Configure(IConfigurationRoot configuration, IApplicationBuilder app, IServiceProvider serviceProvider, ILoggerFactory loggerFactory, bool isDevelopmentEnv)
        {
            UseLogging(configuration, loggerFactory, isDevelopmentEnv);
            UseScheduling(app, serviceProvider);
        }

        private static void UseLogging(IConfigurationRoot configuration, ILoggerFactory loggerFactory, bool isDevelopmentEnv)
        {
            if (isDevelopmentEnv)
            {
                loggerFactory.AddConsole(configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }

            switch (CopiousConfiguration.Config.LoggingProvider)
            {
                case LoggingProvider.Log4Net:

                default:
                    AddLog4Net();
                    break;
            }

            void AddLog4Net()
            {
                var log4netConfig = new XmlDocument();
                using (var reader = new StreamReader(new FileStream("log4net.config", FileMode.Open, FileAccess.Read)))
                {
                    log4netConfig.Load(reader);
                }

                var rep = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                log4net.Config.XmlConfigurator.Configure(rep, log4netConfig["log4net"]);
                loggerFactory.AddProvider(new Log4NetProvider());
            }
        }

        private static void UseScheduling(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            if (CopiousConfiguration.Config.EnableScheduler)
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