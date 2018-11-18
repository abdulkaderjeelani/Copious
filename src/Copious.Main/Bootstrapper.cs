using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Copious.Infrastructure;
using Copious.Infrastructure.AspNet;
using Copious.Infrastructure.DependencyInjection;
using Copious.Infrastructure.Interface;
using Copious.Persistance;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Copious.Main {
    public abstract class Bootstrapper {
        private readonly IConfigurationRoot _configuration;
        private IContainer _applicationContainer;

        protected Bootstrapper (IConfigurationBuilder builder, params (string path, bool optional, bool reloadOnChange) [] configFiles) {
            _configuration = CopiousConfiguration.Initialize (builder, configFiles);
        }

        public virtual IServiceProvider BootstrapServices (IServiceCollection services) {
            //add IConfigurationRoot to the di container
            services.Add (new ServiceDescriptor (typeof (IConfigurationRoot), provider => _configuration, ServiceLifetime.Singleton));

            Infrastructure.Features.ConfigureServices (services);
            Infrastructure.AspNet.Features.ConfigureServices (services, default);

            return RegisterServices ();

            IServiceProvider RegisterServices () {
                var registrators = BootstrapperHelper.GetFrameworkRegistrators ();
                registrators.AddRange (GetAppRegistrators (_configuration));

                registrators.ForEach (registrator => registrator.RegisterDependancies (_configuration, services));
                _applicationContainer = ContainerProvider.Create (services);
                registrators.ForEach (registrator => registrator.RegisterDependancies (_configuration, _applicationContainer, services.BuildServiceProvider ()));
                return _applicationContainer.GetServiceProvider ();

            }
        }

        public virtual void Bootstrap (IApplicationBuilder app, IApplicationLifetime appLifetime, IAntiforgery antiforgery, IHostingEnvironment env, ILoggerFactory loggerFactory,
            IServiceProvider serviceProvider, Func<string, string, Task<ClaimsIdentity>> identityResolver) {
            Infrastructure.Features.Configure (_configuration, app, serviceProvider, loggerFactory, env.EnvironmentName == EnvironmentName.Development);
            Infrastructure.AspNet.Features.Configure (app, antiforgery, identityResolver);
            // dispose resources that have been resolved in the application container, by registering for the "ApplicationStopped" event.
            appLifetime.ApplicationStopped.Register (() => _applicationContainer.Dispose ());
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
        public virtual Bootstrapper RegisterDatabase<TContext> (IServiceCollection services, DbOptions<TContext> dbOptions)
        where TContext : DbContext {
            services.ConfigureDb (_configuration, dbOptions);
            return this;
        }

        protected abstract IEnumerable<IRegistrator> GetAppRegistrators (IConfigurationRoot configuration);

    }
}