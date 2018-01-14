using System;
using System.Reflection;

using Copious.Document.Interface;
using Copious.Document.Interface.State;
using Copious.Document.Persistance;
using Copious.Infrastructure.Interface;
using Copious.Persistance;

using System;

using Copious.Persistance.Interface;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        public void RegisterDependancies(IConfigurationRoot configuration, IContainer container, IServiceProvider serviceProvider)
        {
            DocumentContext ContextProvider() => serviceProvider.GetService<DocumentContext>();

            RegistrationHelper.RegisterGeneralQueryHandlers<Index, DocumentContext>(container, ContextProvider);
            RegistrationHelper.RegisterGeneralQueryHandlers<VersionedDocument, DocumentContext>(container, ContextProvider);
            RegistrationHelper.RegisterGeneralQueryHandlers<Draft, DocumentContext>(container, ContextProvider);
            //RegistrationHelper.RegisterGeneralQueryHandlers<DocumentAccess, DocumentContext>(container, ContextProvider);
        }

        public class testhandler : Copious.Persistance.Interface.IQueryHandler<Copious.Persistance.Interface.GetAllQuery, System.Collections.Generic.List<DocumentAccess>>
        {
            System.Collections.Generic.List<DocumentAccess> IQueryHandler<GetAllQuery, System.Collections.Generic.List<DocumentAccess>>.Fetch(GetAllQuery query)
            {
                return null;
            }
        }
    }
}