using Copious.Infrastructure.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Copious.Infrastructure.DependencyInjection {
    public static class ContainerProvider {
        public static IContainer Create (IServiceCollection services) {
            switch (CopiousConfiguration.Config.DIContainer) {
                case DIContainer.LightInject:
                    return new LightInjectContainer (services);
                case DIContainer.DryIoc:
                    return new DryIocContainer (services);
                case DIContainer.Autofac:
                default:
                    return new AutofacContainer (services);
            }

        }
    }
}