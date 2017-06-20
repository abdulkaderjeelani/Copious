using Copious.Infrastructure.AspNet.Middlewares;
using Copious.Infrastructure.Interface;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Copious.Infrastructure.AspNet
{
    public static class Features
    {   
        public static void ConfigureServices(IServiceCollection services)
        {
            // https://stackoverflow.com/questions/31243068/access-httpcontext-current
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // https://angular.io/docs/ts/latest/guide/security.html#!#xsrf
            // https://github.com/aspnet/Antiforgery
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            if (CopiousConfiguration.Config.EnableCors)
                services.AddCors(options =>
                {
                    options.AddPolicy("SiteCorsPolicy", new CorsPolicyBuilder()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin() // For anyone access.
                        .AllowCredentials()
                        .Build());
                });

            // Register all controllers as services
            services.AddMvc().AddControllersAsServices().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // https://stackoverflow.com/questions/40275195/how-to-setup-automapper-in-asp-net-core
            // https://lostechies.com/jimmybogard/2016/07/20/integrating-automapper-with-asp-net-core-di/
        }

        public static void Configure(IApplicationBuilder app, IAntiforgery antiforgery, Func<string, string, Task<ClaimsIdentity>> identityResolver)
        {
            UseCors(app);
            UseAntiforgery(app, antiforgery);
            UseJWTBearerAuthentication(app, identityResolver);
            UseAspNetIdentity(app);
            UseBasics(app);
        }

        private static void UseJWTBearerAuthentication(IApplicationBuilder app, Func<string, string, Task<ClaimsIdentity>> identityResolver)
        {
            if (identityResolver == null) return;

            // If need, move the secret key into env. variable or any other safe source,
            const string SecretKey = "SECRET_KEY_1!2@3#4$";
            const string Audience = "ClientApps";
            const string Issuer = "TokenProvider";

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            app.UseTokenProvider(new TokenProviderOptions
            {
                Path = "/api/token",
                Audience = Audience,
                Issuer = Issuer,
                SigningCredentials = signingCredentials,
                IdentityResolver = identityResolver,
                Expiration = TimeSpan.FromHours(4)
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = Issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = Audience,
                AudienceValidator = (aud, tkn, prm) =>
                {
                    return aud.First() == Audience;
                },

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });
        }

        private static void UseAspNetIdentity(IApplicationBuilder app)
        {
            if (CopiousConfiguration.Config.IncludeAspNetIdentity)
                app.UseIdentity();
        }

        private static void UseCors(IApplicationBuilder app)
        {
            if (CopiousConfiguration.Config.EnableCors)
                app.UseCors("SiteCorsPolicy");
        }

        private static void UseAntiforgery(IApplicationBuilder app, IAntiforgery antiforgery)
        {
            if (antiforgery == null) return;

            // Middleware to provide antiforgery tokens to client.
            app.Use(next => context =>
            {
                if (string.Equals(context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(context.Request.Path.Value, "/index.html", StringComparison.OrdinalIgnoreCase))
                {
                    // We can send the request token as a JavaScript-readable cookie, and Angular will use it by default.
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false });
                }

                return next?.Invoke(context);
            });
        }

        public static void UseBasics(IApplicationBuilder app)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}