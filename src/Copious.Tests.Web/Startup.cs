using Copious.Infrastructure.Interface;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Copious.Tests.Web
{
    public class Startup
    {
        private readonly Bootstrapper _bootstrapper;


        public Startup(IHostingEnvironment env)
        {
            _bootstrapper = new Bootstrapper(new ConfigurationBuilder().AddEnvironmentVariables().SetBasePath(env.ContentRootPath),
                                                    //config files params
                                                    ("appsettings.json", optional: true, reloadOnChange: true),
                                                    ($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true),
                                                    ("config.json", optional: true, reloadOnChange: true));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services) =>
            _bootstrapper.ConfigureServices(services);

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IAntiforgery antiforgery, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime, IServiceProvider serviceProvider) => 
            _bootstrapper.Configure(app, appLifetime, antiforgery, env, loggerFactory, serviceProvider, app.ApplicationServices.GetService<Controllers.AuthenticationController>().GetIdentity);
    }
}