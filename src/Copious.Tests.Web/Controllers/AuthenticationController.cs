using Copious.Infrastructure.Interface;
using Copious.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Copious.Tests.Web.Controllers {
    public class AuthenticationController : Infrastructure.AspNet.Controllers.AuthenticationController {
        public AuthenticationController (IContextProvider contextProvider, ILoggerFactory loggerFactory, IExceptionHandler exceptionHandler,
            IQueryProcessor queryProcessor, ICommandBus commandBus,
            //UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender) : base (contextProvider, loggerFactory, exceptionHandler,
            queryProcessor, commandBus,
            //userManager, signInManager, roleManager,
            null, null, null,
            emailSender) { }
    }
}