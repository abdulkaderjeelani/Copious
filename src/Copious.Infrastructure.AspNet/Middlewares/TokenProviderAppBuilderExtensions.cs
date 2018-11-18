using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Copious.Infrastructure.AspNet.Middlewares {
    /// <summary>
    /// Adds a token generation endpoint to an application pipeline.
    /// </summary>
    public static class TokenProviderAppBuilderExtensions {
        /// <summary>
        /// Adds the <see cref="TokenProviderMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables token generation capabilities.
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A  <see cref="TokenProviderOptions"/> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// </summary>
        /// <param name="app">r</param>
        /// <param name="options"></param>
        public static IApplicationBuilder UseTokenProvider (this IApplicationBuilder app, TokenProviderOptions options) {
            if (app == null) {
                throw new ArgumentNullException (nameof (app));
            }

            if (options == null) {
                throw new ArgumentNullException (nameof (options));
            }

            return app.UseMiddleware<TokenProviderMiddleware> (Options.Create (options));
        }
    }
}