using Copious.Infrastructure;
using Copious.Infrastructure.AspNet;
using Copious.Infrastructure.DependencyInjection;
using Copious.Infrastructure.Interface;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Copious.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;

namespace Copious.Main
{
    public abstract class Bootstrapper
    {
        private readonly IConfigurationRoot _configuration;
        private IContainer ApplicationContainer;


        protected Bootstrapper(IConfigurationBuilder builder, params (string path, bool optional, bool reloadOnChange)[] configFiles)
        {
            _configuration = CopiousConfiguration.Initialize(builder, configFiles);
        }
        
     
        public virtual IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //add IConfigurationRoot to the di container
            services.Add(new ServiceDescriptor(typeof(IConfigurationRoot), provider => _configuration, ServiceLifetime.Singleton));

            CrossConcerns.ConfigureServices(services);
            Features.ConfigureServices(services);

            return RegisterServices();

            IServiceProvider RegisterServices()
            {
                var registrators = GetFrameworkRegistrators(_configuration);
                registrators.AddRange(GetAppRegistrators(_configuration));

                registrators.ForEach(registrator => registrator.RegisterDependancies(_configuration, services));
                ApplicationContainer = ContainerProvider.Create(services);
                registrators.ForEach(registrator => registrator.RegisterDependancies(_configuration, ApplicationContainer, services.BuildServiceProvider()));
                return ApplicationContainer.GetServiceProvider();

            }
        }

        public virtual void Configure(IApplicationBuilder app, IApplicationLifetime appLifetime, IAntiforgery antiforgery, IHostingEnvironment env, ILoggerFactory loggerFactory,
                                    IServiceProvider serviceProvider, Func<string, string, Task<ClaimsIdentity>> identityResolver)
        {
            CrossConcerns.Configure(_configuration, app, serviceProvider, loggerFactory, env.EnvironmentName == EnvironmentName.Development);
            Features.Configure(app, antiforgery, identityResolver);
            // dispose resources that have been resolved in the application container, by registering for the "ApplicationStopped" event.
            appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }

        /// <summary>
        /// Registers a db context in service collection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbOptions"></param>
        /// <typeparam name="TContext"></typeparam>
        /// <example>
        ///  _bootstrapper.RegisterDatabase(services, new Copious.Persistance.DbOptions{Core.Persistance.ModuleContext}
        ///  {
        ///    DbToUse = Copious.Infrastructure.Interface.Db.Postgres,
        ///    ConnectionStringKey = "PostgreSqlProvider",
        ///    MigrationsAssembly = "Module.Core.Persistance",
        ///    OnSeeding = Module.Core.Persistance.ModuleContextExtensions.EnsureSeedData,
        ///    IsIdentityDb = true,
        ///    UserRoles = new string[] { }});
        /// </example>
        public virtual Bootstrapper RegisterDatabase<TContext>(IServiceCollection services, DbOptions<TContext> dbOptions)
             where TContext : DbContext
        {
            services.ConfigureDb(_configuration, dbOptions);
            return this;
        }

        protected abstract IEnumerable<IRegistrator> GetAppRegistrators(IConfigurationRoot configuration);

        protected virtual List<IRegistrator> GetFrameworkRegistrators(IConfigurationRoot configuration)
        {
            var frameworkRegisrators = new List<IRegistrator>{
                new Infrastructure.Registrator(),
                new Infrastructure.AspNet.Registrator(),
                new Persistance.Registrator(),
                new Application.Registrator(),
                new SharedKernel.Registrator()};

            if (CopiousConfiguration.Config.EnableDocument)
                frameworkRegisrators.Add(new Document.Registrator());

            return frameworkRegisrators;
        }
    }
}