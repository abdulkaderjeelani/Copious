using System;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

using System;

namespace Copious.Infrastructure.Interface
{
    public class CopiousConfiguration
    {
        private static CopiousConfiguration _config;

        public static CopiousConfiguration Config => _config ?? throw new InvalidOperationException($"Configuration is not initialized. Pleas call {nameof(Initialize)}");

        /// <summary>
        /// Prefix of all application assemblies
        /// </summary>
        public IReadOnlyCollection<string> AppAssemblyPrefixes { get; private set; }

        public StorageProvider DefaultDocumentStorageProvider { get; private set; }

        public Dictionary<string, string> DefaultDocumentStorageProviderOptions { get; private set; }

        /// <summary>
        /// Tells which container to use
        /// </summary>
        public DIContainer DIContainer { get; private set; }

        public Db DocumentDb { get; private set; }

        public string DocumentDbConnection { get; private set; }

        public bool EnableAntiforgery { get; private set; }

        /// <summary>
        /// Tells wheter to enable cors
        /// </summary>
        public bool EnableCors { get; private set; }

        public bool EnableDocument { get; private set; }

        /// <summary>
        /// Wheter to add and use aspnet identity
        /// </summary>
        public bool IncludeAspNetIdentity { get; private set; }

        /// <summary>
        /// Tells the logger to use
        /// </summary>
        public LoggingProvider LoggingProvider { get; private set; }

        public bool EnableScheduler { get; private set; }

        public AuthenticationType AuthenticationType { get; private set; }

        /// <summary>
        /// Tells the scheduler to use
        /// </summary>
        public Scheduler Scheduler { get; private set; }

        public Mapper Mapper { get; private set; }

        /// <summary>
        /// Database of scheduler in case of Hangfire
        /// </summary>
        public string SchedulerPostgreSqlDb { get; private set; }

        public static IConfigurationRoot Initialize(IConfigurationBuilder builder, params (string path, bool optional, bool reloadOnChange)[] configFiles)
        {
            foreach (var configFile in configFiles)
                builder = builder.AddJsonFile(path: configFile.path, optional: configFile.optional, reloadOnChange: configFile.reloadOnChange);

            builder.AddJsonFile("copious.config.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            _config = new CopiousConfiguration
            {
                IncludeAspNetIdentity = configuration.GetValue<bool>(nameof(IncludeAspNetIdentity)),
                AppAssemblyPrefixes = configuration.GetValue<string>(nameof(AppAssemblyPrefixes)).Split(','),
                DIContainer = configuration.GetValue<DIContainer>(nameof(DIContainer)),
                LoggingProvider = configuration.GetValue<LoggingProvider>(nameof(LoggingProvider)),
                EnableScheduler = configuration.GetValue<bool>(nameof(EnableScheduler)),
                Scheduler = configuration.GetValue<Scheduler>(nameof(Scheduler)),
                Mapper = configuration.GetValue<Mapper>(nameof(Mapper)),
                SchedulerPostgreSqlDb = configuration.GetConnectionString(nameof(SchedulerPostgreSqlDb)),
                EnableCors = configuration.GetValue<bool>(nameof(EnableCors)),
                EnableAntiforgery = configuration.GetValue<bool>(nameof(EnableAntiforgery)),
                EnableDocument = configuration.GetValue<bool>(nameof(EnableDocument)),
                DocumentDb = configuration.GetValue<Db>(nameof(DocumentDb)),
                DocumentDbConnection = configuration.GetValue<string>(nameof(DocumentDbConnection)),
                DefaultDocumentStorageProvider = configuration.GetValue<StorageProvider>(nameof(DefaultDocumentStorageProvider)),
                AuthenticationType = configuration.GetValue<AuthenticationType>(nameof(AuthenticationType)),
                DefaultDocumentStorageProviderOptions = new Dictionary<string, string>()
            };
            FillDictionaryFromConfig(configuration, nameof(DefaultDocumentStorageProviderOptions), _config.DefaultDocumentStorageProviderOptions);

            return configuration;
        }

        private static void FillDictionaryFromConfig(IConfigurationRoot configuration, string keyName, Dictionary<string, string> dictionary)
        {
            var index = 0;
            while (true)
            {
                var key = configuration[$"{keyName}:{index}:Key"];
                var value = configuration[$"{keyName}:{index}:Value"];

                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    dictionary.Add(key, value);
                    index++;
                }
                else break;
            }
        }
    }
}