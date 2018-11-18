using System;
using Copious.Document.Interface;
using Copious.Document.Interface.State;
using Copious.Foundation;
using Copious.Infrastructure.Interface.Services;

namespace Copious.Document {
    public class DocumentGuard : IDocumentGuard {
        readonly ISecurityProvider _securityProvider;

        public DocumentGuard (ISecurityProvider securityProvider) {
            _securityProvider = securityProvider;
        }

        public void Protect (RequestContext context, Guid documentId) {
            throw new NotImplementedException ();
        }

        public void Verify (RequestContext context, VersionedDocument document) {
            throw new NotImplementedException ();
        }

    }
}