using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Copious.Document.Interface;
using Copious.Document.Interface.State;
using Copious.Foundation;
using Copious.Infrastructure.AspNet.Model;
using Copious.Infrastructure.AspNet.Results;
using Copious.Infrastructure.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Copious.Infrastructure.AspNet.Controllers {
    [Route ("api/[controller]")]
    public abstract class DocumentController : CopiousController {
        private readonly IDocumentRepository _documentRepository;

        protected DocumentController (IContextProvider contextProvider, ILoggerFactory loggerFactory, IExceptionHandler exceptionHandler, IDocumentRepository documentRepository) : base (contextProvider, exceptionHandler, loggerFactory) {
            _documentRepository = documentRepository;
        }

        [HttpPost]
        [Route ("[action]")]
        //[Authorize]
        public virtual ObjectResult Store (DocumentModel document) {
            ObjectResult result;

            try {

                if (document != null && document.FormFile != null) {
                    var versionedDocument = new VersionedDocument {
                    Metadata = document.Metadata,
                    Security = document.Security,
                    Detail = document.Detail,
                    File = document.File
                    };

                    using (var stream = document.FormFile.OpenReadStream ()) {
                        var index = _documentRepository.Save (_contextProvider.Context, versionedDocument, stream);
                        result = new DocumentResult (new ServiceResult<Index> (index)).SetHttpStatusCode (HttpStatusCode.OK);
                    }

                } else
                    result = new DocumentResult (new ServiceResult (DocumentApiErrorCodes.UploadFileMissing))
                    .SetHttpStatusCode (HttpStatusCode.BadRequest);

            } catch (DocumentException dex) {
                result = new DocumentResult (new ServiceResult<DocumentExceptionInfo> (dex.DocumentExceptionInfo, DocumentApiErrorCodes.FromException (dex)))
                    .SetHttpStatusCode (HttpStatusCode.ExpectationFailed);

                HandleException (dex);
            } catch (Exception ex) {
                result = new DocumentResult (new ServiceResult (CopiousErrorCodes.Exception))
                    .SetHttpStatusCode (HttpStatusCode.InternalServerError);
                HandleException (ex);
            }
            return result;
        }

    }

}