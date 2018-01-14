using Copious.Foundation;
using Copious.Infrastructure.Interface;
using Microsoft.AspNetCore.Http;

namespace Copious.Infrastructure.AspNet
{
    public class AspNetContextProvider : IContextProvider
    {
        private readonly HttpContext _httpContext;

        public AspNetContextProvider(IHttpContextAccessor contextAccessor)
        {
            _httpContext = contextAccessor.HttpContext;
        }

        //todo: Create context from aspnet request
        public RequestContext Context =>
            new RequestContext
            {
                Items = _httpContext.Items,
                AbortRequest = () =>
                {
                    _httpContext.Response.StatusCode = 400;
                    _httpContext.Response.WriteAsync("Request aborted.");
                },
                Actor = new Actor
                {
                    Principal = _httpContext.User
                }
            };
    }
}