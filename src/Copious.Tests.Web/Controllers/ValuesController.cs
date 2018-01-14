using System.Runtime.CompilerServices;
using Copious.Infrastructure.AspNet.Controllers;
using Copious.Infrastructure.Interface;
using Copious.Infrastructure.Interface.Services;
using Copious.Persistance.Interface;
using Copious.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Copious.Tests.Web.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : CopiousController
    {
        public ValuesController(IRuleAssessor ruleAssessor, IContextProvider contextProvider, ISecurityProvider securityProvider,
            IScheduler sch, IQueryProcessor queryProcessor, ICommandBus commandBus,
            IExceptionHandler exceptionHandler, ILoggerFactory loggerFactory,
            IQueryHandler<GetAllQuery, List<Copious.Document.Interface.State.Index>> indexQuery) : base(contextProvider, exceptionHandler, loggerFactory)
        {
            
            var x = indexQuery.Fetch(new GetAllQuery(contextProvider.Fn()));


            queryProcessor.Process<GetAllQuery, List<Copious.Document.Interface.State.DocumentAccess>>(new GetAllQuery(contextProvider.Fn()));
            // sch.ScheduleAsyncJob<IEmailSender>(email => email.SendEmailAsync("test@test.com", "test", "test"));
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
            => new string[] { "value1", "value2" };
    }
}