using Copious.Foundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Copious.Document.Interface
{
    public class DocumentException : Exception
    {
        public DocumentException(RequestContext context, DocumentExceptionTypes exType, string usefulMessage) : base(usefulMessage)
        {
            DocumentExceptionInfo = new DocumentExceptionInfo
            {
                ContextOfException = context
            };
            ExceptionType = exType;
        }

        public DocumentException(RequestContext context, DocumentExceptionTypes exType, string usefulMessage, Exception innerException) : base(usefulMessage, innerException)
        {
            DocumentExceptionInfo = new DocumentExceptionInfo
            {
                ContextOfException = context
            };
            ExceptionType = exType;
        }

        public DocumentExceptionTypes ExceptionType { get; set; }

        public DocumentExceptionInfo DocumentExceptionInfo { get; set; }

    }

    public class DocumentExceptionInfo
    {
        public Guid DocumentId { get; set; }

        public int VersionNo { get; set; }

        public RequestContext ContextOfException { get; set; }
    }

    /// <summary>
    /// Instead of creating unique exception for every case, we create a single aggregate exception
    /// <see cref="Copious.Document.Interface.DocumentException"/> and identify it over the kind
    /// </summary>
    public enum DocumentExceptionTypes
    {
        InvalidDocumentKind = 1,
        NotSupportedStorageProvider,
    }
}
