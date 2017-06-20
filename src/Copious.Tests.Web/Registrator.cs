using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Copious.Infrastructure.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Tests.Web
{
    public class Registrator : IRegistrator
    {
        public void RegisterDependancies(IConfigurationRoot configuration, IServiceCollection services)
        {
            // intentionally left empty add, deps if needed
        }

        public void RegisterDependancies(IConfigurationRoot configuration, IContainer container)
        {
            // intentionally left empty add, deps if needed
        }
    }
}