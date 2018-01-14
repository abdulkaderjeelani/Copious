using System;
using Copious.Infrastructure.Interface.Services;
using Copious.Document.Interface;
using Copious.Foundation;
using Copious.Document.Interface.State;

namespace Copious.Document
{
    public class DocumentGuard : IDocumentGuard
    {
        readonly ISecurityProvider _securityProvider;

        public DocumentGuard(ISecurityProvider securityProvider)
        {
            _securityProvider = securityProvider;
        }

        public void Protect(RequestContext context, Guid documentId)
        {
            throw new NotImplementedException();
        }

        public void Verify(RequestContext context, VersionedDocument document)
        {
            throw new NotImplementedException();
        }
        
    }
}