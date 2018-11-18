using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Copious.Infrastructure;
using Copious.Infrastructure.AspNet;
using Copious.Infrastructure.DependencyInjection;
using Copious.Infrastructure.Interface;
using Copious.Persistance;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Copious.Main {
    public static class BootstrapperHelper {
        public static List<IRegistrator> GetFrameworkRegistrators () {
            var frameworkRegisrators = new List<IRegistrator> {
                new Infrastructure.Registrator (),
                new Infrastructure.AspNet.Registrator (),
                new Persistance.Registrator (),
                new Application.Registrator (),
                new SharedKernel.Registrator ()
            };

            if (CopiousConfiguration.Config.EnableDocument) frameworkRegisrators.Add (new Document.Registrator ());

            return frameworkRegisrators;
        }
    }
}