using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace WfPrimeTracker.Server.HangfireActivators {
    public class HangfireActivatorScope : JobActivatorScope {
        private readonly IServiceScope _scope;

        public HangfireActivatorScope(IServiceProvider provider) {
            _scope = provider.CreateScope();
        }

        /// <inheritdoc />
        public override object Resolve(Type type) {
            return _scope.ServiceProvider.GetRequiredService(type);
        }

        /// <inheritdoc />
        public override void DisposeScope() {
            _scope.Dispose();
            base.DisposeScope();
        }

    }
}
