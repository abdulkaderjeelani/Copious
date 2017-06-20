using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;

namespace Copious.Infrastructure.Scheduler
{
    public class ServiceProviderActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
            => _serviceProvider.GetService(jobType);
    }
}