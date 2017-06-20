using System;
using Copious.Infrastructure.Interface.Services;
using Copious.Document.Interface;
using Copious.Foundation;
using Copious.Document.Interface.State;

namespace Copious.Document
{
    public class DocumentGuard : IDocumentGuard
    {
        private readonly ISecurityProvider _securityProvider;

        public DocumentGuard(ISecurityProvider securityProvider)
        {
            _securityProvider = securityProvider;
        }

        public void Protect(Context context, Guid documentId)
        {
            throw new NotImplementedException();
        }

        public void Verify(Context context, VersionedDocument document)
        {
            throw new NotImplementedException();
        }
        
    }
}