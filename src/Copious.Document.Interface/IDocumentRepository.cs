using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Copious.Document.Interface.State;
using Copious.Foundation;

namespace Copious.Document.Interface {
    public interface IDocumentRepository {
        /// <summary>
        /// Save a document to storage and its meta data to a document persistance
        /// </summary>
        /// <param name="document"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        Index Save (RequestContext context, VersionedDocument document, Stream file);

        /// <summary>
        /// Save a draft
        /// If a default draft already exists for the document then its overwritten.
        /// Drafts are saved based on draft name and user name, means a User can have THE SAME document saved as multiple drafts but in different names.
        /// </summary>        
        void SaveDraft (RequestContext context, Guid documentId, string draftName, Stream file);

        /// <summary>
        /// Searches the document index based on search criteria provided.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="index"></param>
        /// <returns>Ids of documents those matach to the search criteria</returns>
        List<Guid> Search (RequestContext context, Index index);

        /// <summary>
        /// Gets the index of the document id provided
        /// </summary>
        /// <param name="context"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        Index GetIndex (RequestContext context, Guid documentId);

        /// <summary>
        /// Gets the document object from the document id and version
        /// </summary>
        /// <param name="context"></param>
        /// <param name="documentId"></param>
        /// /// <param name="version"></param>
        /// <returns></returns>
        VersionedDocument GetDocument (RequestContext context, Guid documentId, int version);

        /// <summary>
        /// Gets the document objects from the document id and version
        /// </summary>
        /// <param name="context"></param>
        /// <param name="documentId"></param>       
        /// <returns></returns>
        List<VersionedDocument> GetDocuments (RequestContext context, Guid documentId);

        /// <summary>
        /// Return the document based on its Id
        /// </summary>
        Stream Get (RequestContext context, Guid documentId, out VersionedDocument metaData);

        /// <summary>
        /// Returns the draft corresponding to user
        /// </summary>
        Stream GetDraft (RequestContext context, string draftName);

        Stream GetDraft (RequestContext context, Guid documentId, string draftName);

        /// <summary>
        /// Logical delete
        /// </summary>        
        void Remove (RequestContext context, Guid documentId);

        /// <summary>
        /// Physical delete
        /// </summary>        
        void Destroy (RequestContext context, Guid documentId);
    }
}