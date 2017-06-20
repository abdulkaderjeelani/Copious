using Copious.Infrastructure.Interface;
using Copious.Document.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Copious.Persistance;
using Copious.Document.Persistance;
using System.Reflection;

namespace Copious.Document
{
    public class Registrator : IRegistrator
    {
        public void RegisterDependancies(IConfigurationRoot configuration, IServiceCollection services)
        {
            services.ConfigureDb(configuration, new DbOptions<DocumentContext>
            {
                DbToUse = CopiousConfiguration.Config.DocumentDb,
                ConnectionStringKey = CopiousConfiguration.Config.DocumentDbConnection,                
                MigrationsAssembly = typeof(DocumentContext).GetTypeInfo().Assembly.GetName().Name
            });

            services.AddScoped<IDocumentGuard, DocumentGuard>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
        }

        public void RegisterDependancies(IConfigurationRoot configuration, IContainer container)
        {
            // intentionally left empty
        }
    }
}