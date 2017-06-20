using Copious.Document.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Copious.Infrastructure.Interface;
using Copious.Document.Interface.State;
using Copious.Foundation;
using Copious.Document.Persistance;
using Copious.Infrastructure.Documents.Storage;
using Copious.Infrastructure.Documents.Storage.Factory;

namespace Copious.Document
{
    public sealed class DocumentRepository : IDocumentRepository
    {
        private readonly IDocumentGuard _guard;
        private readonly DocumentContext _persistance;

        public DocumentRepository(IDocumentGuard guard, DocumentContext persistance)
        {
            _guard = guard;
            _persistance = persistance;
        }

        public void Destroy(Context context, Guid documentId)
        {
            _guard.Protect(context, documentId);
        }

        public Index GetIndex(Context context, Guid documentId)
        {
            _guard.Protect(context, documentId);

            return _persistance.Index.SingleOrDefault(i => i.Id == documentId);
        }

        public VersionedDocument GetDocument(Context context, Guid documentId, int version)
        {
            _guard.Protect(context, documentId);

            return _persistance.VersionedDocuments
                   .SingleOrDefault(v => v.DocumentId == documentId && v.VersionNo == version);
        }

        public List<VersionedDocument> GetDocuments(Context context, Guid documentId)
        {
            _guard.Protect(context, documentId);

            return _persistance.VersionedDocuments
                   .Where(v => v.DocumentId == documentId).ToList();
        }

        public Stream Get(Context context, Guid documentId, out VersionedDocument document)
        {
            _guard.Protect(context, documentId);

            document = null;
            return null;
        }

        public Stream GetDraft(Context context, string draftName)
        {
            //retrieve the doc id from persistance based on draft name and user
            Guid documentId;
            _guard.Protect(context, documentId);

            return null;
        }

        public Stream GetDraft(Context context, Guid documentId, string draftName)
        {
            _guard.Protect(context, documentId);
            return null;
        }

        public void Remove(Context context, Guid documentId)
        {
            _guard.Protect(context, documentId);

        }

        public Index Save(Context context, VersionedDocument document, Stream source)
        {
            if (document.DocumentKind == DocumentKind.Content)
                throw new DocumentException(context, DocumentExceptionTypes.InvalidDocumentKind, "Invalid document kind specified during Save. If you intend to save content then Use \"SaveContent\" Method");

            _guard.Verify(context, document);

            //save the index first then the first version of document in a transaction an return the index object.

            var storageProvider = PrepareForStorage(context, document);
            storageProvider.SaveBlobStreamAsync(document.File.Container, document.File.Blob, source);

            return null;
        }

        public void SaveDraft(Context context, Guid documentId, string draftName, Stream file)
        {
            var index = GetIndex(context, documentId);
            var document = GetDocument(context, index.Id, index.VersionNo);

            _guard.Verify(context, document);

            throw new NotImplementedException();
        }

        public List<Guid> Search(Context context, Index index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Based on context / settings decide the storage to use and fill up the File on document object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="document"></param>
        private static IStorageProvider PrepareForStorage(Context context, VersionedDocument document)
        {
            // fill the file object here

            document.File = new Interface.State.File
            {
                Container = "Uploads",
                Blob = document.Metadata.Name,
                Provider = CopiousConfiguration.Config.DefaultDocumentStorageProvider,
                ProviderOptions = CopiousConfiguration.Config.DefaultDocumentStorageProviderOptions
            };

            return GetStorageProvider(context, document.File);
        }

        /// <summary>
        /// Detect the provider from the file object and create an instance of it
        /// </summary>
        /// <param name="context"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private static IStorageProvider GetStorageProvider(Context context, Interface.State.File file)
        {
            IStorageProvider storageProvider = null;

            try
            {
                storageProvider = StorageFactory.CreateProvider(file.Provider, file.ProviderOptions);
            }
            catch (NotSupportedException)
            {

                throw new DocumentException(context, DocumentExceptionTypes.NotSupportedStorageProvider, $"Sorry the provider {CopiousConfiguration.Config.DefaultDocumentStorageProvider.ToString()} is not supported now");
            }

            return storageProvider;
        }
    }
}