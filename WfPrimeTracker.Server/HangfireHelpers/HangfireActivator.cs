using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace WfPrimeTracker.Server.HangfireActivators {
    public class HangfireActivator : JobActivator {
        private IServiceProvider _provider;

        public HangfireActivator(IServiceProvider provider) {
            _provider = provider;
        }

        public override object ActivateJob(Type type) {
            return _provider.GetRequiredService(type);
        }

        /// <inheritdoc />
        public override JobActivatorScope BeginScope(JobActivatorContext context) {
            return new HangfireActivatorScope(_provider);
        }
    }
}
