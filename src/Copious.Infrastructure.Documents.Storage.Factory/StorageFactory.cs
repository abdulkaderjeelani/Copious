using System;
using System.Collections.Generic;
using System.Text;
using Copious.Infrastructure.Documents.Storage.Amazon;
using Copious.Infrastructure.Documents.Storage.Azure;
using Copious.Infrastructure.Documents.Storage.Google;
using Copious.Infrastructure.Documents.Storage.Local;
using Copious.Infrastructure.Interface;

namespace Copious.Infrastructure.Documents.Storage.Factory {
    public class StorageFactory {
        public static IStorageProvider CreateProvider (StorageProvider provider, object options) {
            IStorageProvider storageProvider;
            switch (provider) {
                case StorageProvider.FileSystem:
                    storageProvider = new LocalStorageProvider (options as LocalOptions);
                    break;
                case StorageProvider.Azure:
                    storageProvider = new AzureStorageProvider (options as AzureProviderOptions);
                    break;
                case StorageProvider.Amazon:
                    storageProvider = new AmazonStorageProvider (options as AmazonProviderOptions);
                    break;
                case StorageProvider.Google:
                    storageProvider = new GoogleStorageProvider (options as GoogleProviderOptions);
                    break;
                case StorageProvider.Rethink:
                default:
                    throw new NotSupportedException ();

            }

            return storageProvider;
        }
    }
}