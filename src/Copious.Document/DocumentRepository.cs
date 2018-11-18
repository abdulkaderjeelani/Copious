using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Copious.Document.Interface;
using Copious.Document.Interface.State;
using Copious.Document.Persistance;
using Copious.Foundation;
using Copious.Infrastructure.Documents.Storage;
using Copious.Infrastructure.Documents.Storage.Factory;
using Copious.Infrastructure.Interface;

namespace Copious.Document {
    public sealed class DocumentRepository : IDocumentRepository {
        readonly IDocumentGuard _guard;
        readonly DocumentContext _persistance;

        public DocumentRepository (IDocumentGuard guard, DocumentContext persistance) {
            _guard = guard;
            _persistance = persistance;
        }

        public void Destroy (RequestContext context, Guid documentId) {
            _guard.Protect (context, documentId);
        }

        public Index GetIndex (RequestContext context, Guid documentId) {
            _guard.Protect (context, documentId);

            return _persistance.Index.SingleOrDefault (i => i.Id == documentId);
        }

        public VersionedDocument GetDocument (RequestContext context, Guid documentId, int version) {
            _guard.Protect (context, documentId);

            return _persistance.VersionedDocuments
                .SingleOrDefault (v => v.DocumentId == documentId && v.VersionNo == version);
        }

        public List<VersionedDocument> GetDocuments (RequestContext context, Guid documentId) {
            _guard.Protect (context, documentId);

            return _persistance.VersionedDocuments
                .Where (v => v.DocumentId == documentId).ToList ();
        }

        public Stream Get (RequestContext context, Guid documentId, out VersionedDocument document) {
            _guard.Protect (context, documentId);

            document = null;
            return null;
        }

        public Stream GetDraft (RequestContext context, string draftName) {
            //retrieve the doc id from persistance based on draft name and user
            Guid documentId;
            _guard.Protect (context, documentId);

            return null;
        }

        public Stream GetDraft (RequestContext context, Guid documentId, string draftName) {
            _guard.Protect (context, documentId);
            return null;
        }

        public void Remove (RequestContext context, Guid documentId) {
            _guard.Protect (context, documentId);

        }

        public Index Save (RequestContext context, VersionedDocument document, Stream source) {
            if (document.DocumentKind == DocumentKind.Content)
                throw new DocumentException (context, DocumentExceptionTypes.InvalidDocumentKind, "Invalid document kind specified during Save. If you intend to save content then Use \"SaveContent\" Method");

            _guard.Verify (context, document);

            //save the index first then the first version of document in a transaction an return the index object.

            var storageProvider = PrepareForStorage (context, document);
            storageProvider.SaveBlobStreamAsync (document.File.Container, document.File.Blob, source);

            return null;
        }

        public void SaveDraft (RequestContext context, Guid documentId, string draftName, Stream file) {
            var index = GetIndex (context, documentId);
            var document = GetDocument (context, index.Id, index.VersionNo);

            _guard.Verify (context, document);

            throw new NotImplementedException ();
        }

        public List<Guid> Search (RequestContext context, Index index) {
            throw new NotImplementedException ();
        }

        /// <summary>
        /// Based on context / settings decide the storage to use and fill up the File on document object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="document"></param>
        static IStorageProvider PrepareForStorage (RequestContext context, VersionedDocument document) {
            // fill the file object here

            document.File = new Interface.State.File {
                Container = "Uploads",
                Blob = document.Metadata.Name,
                Provider = CopiousConfiguration.Config.DefaultDocumentStorageProvider,
                ProviderOptions = CopiousConfiguration.Config.DefaultDocumentStorageProviderOptions
            };

            return GetStorageProvider (context, document.File);
        }

        /// <summary>
        /// Detect the provider from the file object and create an instance of it
        /// </summary>
        /// <param name="context"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        static IStorageProvider GetStorageProvider (RequestContext context, Interface.State.File file) {
            IStorageProvider storageProvider = null;

            try {
                storageProvider = StorageFactory.CreateProvider (file.Provider, file.ProviderOptions);
            } catch (NotSupportedException) {

                throw new DocumentException (context, DocumentExceptionTypes.NotSupportedStorageProvider, $"Sorry the provider {CopiousConfiguration.Config.DefaultDocumentStorageProvider.ToString()} is not supported now");
            }

            return storageProvider;
        }
    }
}