using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Copious.Document.Interface;
using Copious.Document.Interface.State;
using Copious.Foundation;
using Microsoft.AspNetCore.Mvc;

namespace Copious.Infrastructure.AspNet.Results {
    public class DocumentResult : ObjectResult {
        private enum DocumentResultType { None, Index, Indices, Document, Documents }

        private readonly DocumentResultType _documentResultType;

        public DocumentResult (ServiceResult result) : base (result) {
            _documentResultType = DocumentResultType.None;
        }

        public DocumentResult (ServiceResult<Index> index) : base (index) {
            _documentResultType = DocumentResultType.Index;
        }

        public DocumentResult (ServiceResult<List<Index>> indices) : base (indices) {
            _documentResultType = DocumentResultType.Indices;
        }

        public DocumentResult (ServiceResult<VersionedDocument> document) : base (document) {
            _documentResultType = DocumentResultType.Document;
        }

        public DocumentResult (ServiceResult<List<VersionedDocument>> documents) : base (documents) {
            _documentResultType = DocumentResultType.Documents;
        }

        public List<Index> Indices {
            get {
                GuardDefault (DocumentResultType.Indices);
                return Value as List<Index>;
            }
            set {
                GuardDefault (DocumentResultType.Indices);
                Value = value;
            }
        }

        public Index Index {
            get {
                GuardDefault (DocumentResultType.Index);
                return Value as Index;
            }
            set {
                GuardDefault (DocumentResultType.Index);
                Value = value;
            }
        }

        public List<VersionedDocument> Documents {
            get {
                GuardDefault (DocumentResultType.Documents);
                return Value as List<VersionedDocument>;
            }
            set {
                GuardDefault (DocumentResultType.Documents);
                Value = value;
            }
        }

        public VersionedDocument Document {
            get {
                GuardDefault (DocumentResultType.Document);
                return Value as VersionedDocument;
            }
            set {
                GuardDefault (DocumentResultType.Document);
                Value = value;
            }
        }

        public DocumentResult SetHttpStatusCode (HttpStatusCode value) {
            StatusCode = (int) value;
            return this;
        }

        private void GuardDefault (DocumentResultType documentResultType) {
            if (documentResultType != _documentResultType)
                throw new InvalidOperationException ($"Invalid {nameof(DocumentResultType)}");
        }

    }

    public static class DocumentApiErrorCodes {
        public static readonly ErrorCode UploadFileMissing = new ErrorCode (100, "File not found in the request.");
        private const int DocumentExceptionErrorCodeBase = 1000;
        public static ErrorCode FromException (DocumentException ex) => new ErrorCode (DocumentExceptionErrorCodeBase + (int) ex.ExceptionType, ex.Message);
    }

}