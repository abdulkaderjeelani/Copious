using System;
using System.Threading.Tasks;
using Copious.Infrastructure.Interface.AspNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Copious.Infrastructure.AspNet.Filters {
    public sealed class AntiForgeryFilterAttribute : TypeFilterAttribute {
        public AntiForgeryFilterAttribute () : base (typeof (AntiForgeryFilter)) { }

        // Sub class to use dependancy injection seamlessly
        private class AntiForgeryFilter : IAsyncActionFilter {
            private readonly IAntiForgeryValidator _antiforgery;

            public AntiForgeryFilter (IAntiForgeryValidator antiforgery) {
                _antiforgery = antiforgery;
            }

            public async Task OnActionExecutionAsync (ActionExecutingContext context, ActionExecutionDelegate next) {
                try {
                    // This will throw if the token is invalid.
                    await _antiforgery.ValidateRequestAsync (context.HttpContext);
                } catch (Exception ex) {
                    Console.WriteLine (ex.Message);
                    context.Result = new StatusCodeResult (401);
                    return;
                }

                await next?.Invoke ();
            }
        }
    }
}