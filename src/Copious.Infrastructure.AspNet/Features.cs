using System;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Copious.Infrastructure.AspNet.Middlewares;
using Copious.Infrastructure.Interface;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Copious.Infrastructure.AspNet {
    /// <summary>
    /// Provides helper methods to add aspnet specific features
    /// </summary>
    public static class Features {
        private const string Audience = "ClientApps";
        private const string Issuer = "TokenProvider";

        // If need, move the secret key into env. variable or any other safe source,
        private const string SecretKey = "SECRET_KEY_1!2@3#4$";

        public static void Configure (IApplicationBuilder app, IAntiforgery antiforgery, Func<string, string, Task<ClaimsIdentity>> identityResolver) {
            UseCors (app);
            UseAntiforgery (app, antiforgery);
            UseAuthentication (app, identityResolver);
        }

        public static void ConfigureServices (IServiceCollection services, Action<MvcOptions> setupAction) {
            Init (services);

            AddCors (services);

            if (setupAction == null) setupAction = mvcOptions => { };

            // Register all controllers as services
            services.AddMvc (setupAction).AddControllersAsServices ().AddJsonOptions (options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            // https://angular.io/docs/ts/latest/guide/security.html#!#xsrf
            // https://github.com/aspnet/Antiforgery
            AddAntiforgery (services);

            AddAuthentication (services);

            AddAspnetIdentity (services);
        }

        public static void Init (IServiceCollection services) {
            // https://stackoverflow.com/questions/31243068/access-httpcontext-current
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor> ();
        }

        public static void AddAntiforgery (IServiceCollection services) {
            if (CopiousConfiguration.Config.EnableAntiforgery)
                services.AddAntiforgery (options => options.HeaderName = "X-XSRF-TOKEN");
        }

        public static void AddAuthentication (IServiceCollection services) {
            switch (CopiousConfiguration.Config.AuthenticationType) {
                case AuthenticationType.JWT:
                    AddJwtAuthentication (services);
                    break;

                case AuthenticationType.None:
                    break;
            }
        }

        public static void UseBasics (IApplicationBuilder app) {
            app.UseDefaultFiles ();
            app.UseStaticFiles ();
            app.UseMvc (routes => {
                routes.MapRoute (
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public static void AddAspnetIdentity (IServiceCollection services) {
            if (CopiousConfiguration.Config.IncludeAspNetIdentity)
                services.Configure<IdentityOptions> (options => {
                    // Password settings
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 6;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (30);
                    options.Lockout.MaxFailedAccessAttempts = 10;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings
                    options.User.RequireUniqueEmail = true;
                });

            services.ConfigureApplicationCookie (options => {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays (150);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });
        }

        public static void AddCors (IServiceCollection services) {
            // https://docs.microsoft.com/en-us/aspnet/core/security/cors

            if (!CopiousConfiguration.Config.EnableCors) return;

            services.AddCors (options => {
                options.AddPolicy ("SiteCorsPolicy", new CorsPolicyBuilder ()
                    .AllowAnyHeader ()
                    .AllowAnyMethod ()
                    .AllowAnyOrigin () // For anyone access.
                    .AllowCredentials ()
                    .Build ());
            });
        }

        public static void AddSession (IServiceCollection services) {
            services.AddSession ();
        }

        private static void AddJwtAuthentication (IServiceCollection services) {
            services
                .AddAuthentication (CookieAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer (j => {
                    j.TokenValidationParameters = new TokenValidationParameters {
                        // The signing key must match!
                        ValidateIssuerSigningKey = true,
                            IssuerSigningKey = GetSigningKey (),

                            // Validate the JWT Issuer (iss) claim
                            ValidateIssuer = true,
                            ValidIssuer = Issuer,

                            // Validate the JWT Audience (aud) claim
                            ValidateAudience = true,
                            ValidAudience = Audience,
                            AudienceValidator = (aud, tkn, prm) => {
                                return aud.First () == Audience;
                            },

                            // Validate the token expiry
                            ValidateLifetime = true,

                            // If you want to allow a certain amount of clock drift, set that here:
                            ClockSkew = TimeSpan.Zero
                    };
                });
        }

        private static SymmetricSecurityKey GetSigningKey () => new SymmetricSecurityKey (Encoding.ASCII.GetBytes (SecretKey));

        public static void UseAntiforgery (IApplicationBuilder app, IAntiforgery antiforgery) {
            if (!CopiousConfiguration.Config.EnableAntiforgery || antiforgery == null) return;

            // Middleware to provide antiforgery tokens to client.
            app.Use (next => context => {
                if (string.Equals (context.Request.Path.Value, "/", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals (context.Request.Path.Value, "/index.html", StringComparison.OrdinalIgnoreCase)) {
                    // We can send the request token as a JavaScript-readable cookie, and Angular will use it by default.
                    var tokens = antiforgery.GetAndStoreTokens (context);
                    context.Response.Cookies.Append ("XSRF-TOKEN", tokens.RequestToken, new CookieOptions { HttpOnly = false });
                }

                return next?.Invoke (context);
            });
        }

        public static void UseCors (IApplicationBuilder app) {
            if (CopiousConfiguration.Config.EnableCors)
                app.UseCors ("SiteCorsPolicy");
        }

        private static void UseAuthentication (IApplicationBuilder app, Func<string, string, Task<ClaimsIdentity>> identityResolver) {
            if (identityResolver == null) return;

            switch (CopiousConfiguration.Config.AuthenticationType) {
                case AuthenticationType.JWT:
                    UseJwtBearerAuthentication (app, identityResolver);
                    break;
            }

            if (CopiousConfiguration.Config.AuthenticationType != AuthenticationType.None)
                app.UseAuthentication ();
        }

        private static void UseJwtBearerAuthentication (IApplicationBuilder app, Func<string, string, Task<ClaimsIdentity>> identityResolver) {
            var signingKey = GetSigningKey ();
            var signingCredentials = new SigningCredentials (signingKey, SecurityAlgorithms.HmacSha256);

            app.UseTokenProvider (new TokenProviderOptions {
                Path = "/api/token",
                    Audience = Audience,
                    Issuer = Issuer,
                    SigningCredentials = signingCredentials,
                    IdentityResolver = identityResolver,
                    Expiration = TimeSpan.FromHours (4)
            });
        }
    }
}