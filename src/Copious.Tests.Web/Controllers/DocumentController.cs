using Copious.Document.Interface;
using Copious.Infrastructure.Interface;
using Copious.Infrastructure.Interface.Services;
using Copious.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Copious.Tests.Web.Controllers
{
    [Route("api/[controller]")]
    public class DocumentController : Infrastructure.AspNet.Controllers.DocumentController
    {
        public DocumentController(IContextProvider contextProvider, ILoggerFactory loggerFactory, IExceptionHandler exceptionHandler, IDocumentRepository documentRepository)
            : base(contextProvider, loggerFactory, exceptionHandler, documentRepository)            
        {
        }
    }
}