using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Copious.Infrastructure.Interface.AspNet {
    public interface IAntiForgeryValidator {
        Task ValidateRequestAsync (HttpContext httpContext);
    }
}