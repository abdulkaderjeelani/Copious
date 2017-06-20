using Copious.Document.Interface.State;
using Copious.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Document.Interface
{
    public interface IDocumentGuard
    {
        /// <summary>
        /// Checks the security and throws exceptions if any violation
        /// </summary>
        /// <param name="context"></param>
        /// <param name="documentId"></param>
        void Protect(Context context, Guid documentId);

        /// <summary>
        /// Verify security during new document / draft
        /// </summary>
        /// <param name="context"></param>
        /// <param name="document"></param>
        void Verify(Context context, VersionedDocument document);
    }
}