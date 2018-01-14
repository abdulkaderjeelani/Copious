using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using Copious.Infrastructure.AspNet.Filters;
using Copious.Infrastructure.AspNet.Model;
using Copious.Infrastructure.AspNet.Results;
using Copious.Infrastructure.Interface;
using Copious.Infrastructure.Interface.Services;
using Copious.SharedKernel;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using System;

namespace Copious.Infrastructure.AspNet.Controllers
{
    [Route("api/[controller]")]
    public abstract class AuthenticationController : CopiousController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        protected AuthenticationController(IContextProvider contextProvider, ILoggerFactory loggerFactory, IExceptionHandler exceptionHandler,
                                           IQueryProcessor queryProcessor, ICommandBus commandBus,
                                           UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
                                           IEmailSender emailSender)
            : base(contextProvider, exceptionHandler, queryProcessor, commandBus, loggerFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        //https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api

        /// <summary>
        /// Login is now done using a middleware for the client application.
        /// This Login method does not generate token, It only checks whether user is valid.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpPost]
        [AllowAnonymous]
        [AntiForgeryFilter]
        public virtual async Task<IActionResult> Login([FromBody]Credentials credentials)
        {
            var identity = await GetIdentity(credentials.Username, credentials.Password);
            return new OkObjectResult(identity);
        }

        /// <summary>
        /// Authenticates the users and generate a claim identity.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <remarks>Called from token provider, wired in Startup </remarks>
        public virtual async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var user = new ApplicationUser(username);
            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (signInResult.Succeeded)
            {
                var genericId = new GenericIdentity(username, "Token");
                var userRoles = await _userManager.GetRolesAsync(user);
                var identity = new ClaimsIdentity(genericId, userRoles.Select(role => new Claim("role", role, ClaimValueTypes.String)));
                return identity;
            }

            return null;
        }

        public virtual async Task<Func<Task>> CreateUser(ApplicationUser newUser, string password, params Func<Task>[] failureHandler)
        {
            try
            {
                var existUser = await _userManager.FindByNameAsync(newUser.UserName);
                if (existUser != null)
                    throw new ControllerException(new NotOkResult(new ServiceResult(ErrorCodes.UserAlreadyExists)));

                _logger.LogInformation(nameof(CreateUser));

                var userResult = await _userManager.CreateAsync(newUser, password);

                if (!userResult.Succeeded)
                    throw new ControllerException(new NotOkResult(new ServiceResult(ErrorCodes.UserCreationFailed.AddInfo(userResult.Errors.ToReadable()))));

                //Check user exists,
                newUser = await _userManager.FindByIdAsync(newUser.Id);
            }
            catch
            {
                await InvokeFailureHandlersAsync(failureHandler);
                throw;
            }
            // return the failure handler to caller
            return async () => await _userManager.DeleteAsync(newUser);
        }

        public virtual async Task<Func<Task>> AddToRole(ApplicationUser user, string role, params Func<Task>[] failureHandler)
        {
            try
            {
                _logger.LogInformation(nameof(AddToRole));

                var roleResult = await _userManager.AddToRoleAsync(user, role);

                if (!roleResult.Succeeded)
                    throw new ControllerException(new NotOkResult(new ServiceResult(ErrorCodes.RoleCreationFailed.AddInfo(roleResult.Errors.ToReadable()))));
            }
            catch
            {
                await InvokeFailureHandlersAsync(failureHandler);
                throw;
            }

            // return the failure handler to the caller, this is executed on further failure
            return async () => await _userManager.RemoveFromRoleAsync(user, role);
        }
    }
}