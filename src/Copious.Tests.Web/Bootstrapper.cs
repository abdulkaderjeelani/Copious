using Copious.Infrastructure.Interface;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Copious.Tests.Web
{
    public sealed class Bootstrapper : Main.Bootstrapper
    {
        public Bootstrapper(IConfigurationBuilder builder, params (string path, bool optional, bool reloadOnChange)[] configFiles) : base(builder, configFiles)
        {
        }

        protected override IEnumerable<IRegistrator> GetAppRegistrators(IConfigurationRoot configuration)
        => new IRegistrator[] { };
    }
}