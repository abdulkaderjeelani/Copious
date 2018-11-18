using System;
using System.Linq;
using System.Threading.Tasks;
using Copious.Infrastructure.Interface.AspNet;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Copious.Infrastructure.AspNet {
    public class AntiForgeryValidator : IAntiForgeryValidator {
        private readonly IAntiforgery _antiforgery;
        private readonly IConfigurationRoot _config;

        public AntiForgeryValidator (IConfigurationRoot config, IAntiforgery antiforgery) {
            _config = config;
            _antiforgery = antiforgery;
        }

        private static readonly string[] IgnoredMessages = {
            "The provided antiforgery token was meant for a different claims-based user than the current user",
            "The provided antiforgery token was meant for user"
        };

        public Task ValidateRequestAsync (HttpContext httpContext) {
            if (!_config.GetValue<bool> ("EnableAntiForgery")) return Task.FromResult (0);
            try {
                return _antiforgery.ValidateRequestAsync (httpContext);
            } catch (Exception ex) {
                // https://github.com/aspnet/Security/blob/7634c5420a85a28217b0384f34324273aed042c5/src/Microsoft.AspNetCore.Authentication.JwtBearer/JwtBearerHandler.cs
                // https://github.com/aspnet/Antiforgery/blob/dev/src/Microsoft.AspNetCore.Antiforgery/Resources.resx

                if (!IgnoredMessages.Any (i => ex.Message.StartsWith (i, StringComparison.OrdinalIgnoreCase)))
                    throw;

                return Task.FromResult (0);
            }
        }
    }
}