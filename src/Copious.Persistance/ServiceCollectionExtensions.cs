using System;
using System.Linq;
using System.Collections.Generic;
using Copious.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Copious.Infrastructure.Interface;
using Copious.Persistance.Interface;

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
        public static void ConfigureDb<TContext>(this IServiceCollection services, IConfigurationRoot configuration, DbOptions<TContext> dbOptions)
            where TContext : DbContext
        {
            if (dbOptions == null)
                throw new ArgumentNullException(nameof(dbOptions), "Database configuration options is mandatory.");

            var connectionString = configuration.GetConnectionString(dbOptions.ConnectionStringKey); // "PostgreSqlProvider"

            //Add entity framework
            services.AddEntityFramework().AddEntityFrameworkNpgsql();

            if (dbOptions.DbToUse == Db.Postgres)
            {
                //Add the datacontext into service collection, so that we can resolve it
                services.AddDbContext<TContext>(options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly(dbOptions.MigrationsAssembly))); // "Codify.Core.Persistance"
            }

            if (CopiousConfiguration.Config.IncludeAspNetIdentity && dbOptions.IsIdentityDb)
            {
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
                if (CopiousConfiguration.Config.IncludeAspNetIdentity && dbOptions.IsIdentityDb && dbOptions.UserRoles != null && dbOptions.UserRoles.Any())
                {
                    var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
                    foreach (var role in dbOptions.UserRoles)
                        roleManager.CreateAsync(new IdentityRole(role.ToString())).Wait();
                }
            }
        }
    }
}