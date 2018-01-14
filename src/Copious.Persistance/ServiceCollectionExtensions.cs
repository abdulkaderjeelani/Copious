using System;
using System.Linq;

using Copious.Infrastructure.Interface;
using Copious.SharedKernel;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Persistance
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the  database used by application.
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="dbOptions"></param>
        /// <remarks>
        /// Call this method from your boot strapper class before any service registrations
        /// </remarks>
        /// <example>
        ///
        ///  {
        ///    DbToUse = Copious.Infrastructure.Interface.Db.Postgres,
        ///    ConnectionStringKey = "PostgreSqlProvider",
        ///    MigrationsAssembly = "Module.Core.Persistance",
        ///    OnSeeding = Module.Core.Persistance.ModuleContextExtensions.EnsureSeedData,
        ///    IsIdentityDb = true,
        ///    UserRoles = new string[] { }
        ///
        ///  }
        /// </example>
        public static void ConfigureDb<TContext>(this IServiceCollection services, IConfigurationRoot configuration, DbOptions<TContext> dbOptions)
            where TContext : DbContext
        {
            if (dbOptions == null)
                throw new ArgumentNullException(nameof(dbOptions), "Database configuration options is mandatory.");

            var connectionString = configuration.GetConnectionString(dbOptions.ConnectionStringKey); // "PostgreSqlProvider"

            if (dbOptions.DbToUse == Db.Postgres)
            {
                //Add entity framework
                services.AddEntityFrameworkNpgsql();

                //Add the datacontext into service collection, so that we can resolve it
                services.AddDbContext<TContext>(options =>
                {
                    options.UseNpgsql(connectionString, b => b.MigrationsAssembly(dbOptions.MigrationsAssembly)); // e.g. "[ProjectName].Core.Persistance"
                    options.EnableSensitiveDataLogging();
                });
            }

            if (CopiousConfiguration.Config.IncludeAspNetIdentity && dbOptions.IsIdentityDb)
            {
                // https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?tabs=visual-studio%2Caspnetcore2x
                //Add asp net identity and use the existing datacontext
                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<TContext>()
                    .AddDefaultTokenProviders();
            }

            //Run the code first migrations, and make sure db is updated
            var serviceProvider = services.BuildServiceProvider();

            using (var context = serviceProvider.GetService<TContext>())
            {
                context.Database.Migrate();
                dbOptions.OnSeeding?.Invoke(context, true);

                //create the default roles provided if any
                if (!CopiousConfiguration.Config.IncludeAspNetIdentity || !dbOptions.IsIdentityDb || dbOptions.UserRoles == null || !dbOptions.UserRoles.Any()) return;

                var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
                var results = dbOptions.UserRoles.Select(role => roleManager.CreateAsync(new IdentityRole(role.ToString()))).Select(async t => await t);
            }
        }
    }
}