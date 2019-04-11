using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Dashboard;

namespace WfPrimeTracker.Server.HangfireHelpers {
    public class HangfireAutorizationFilter : IDashboardAuthorizationFilter {
        /// <inheritdoc />
        public bool Authorize(DashboardContext context) {
            return true;
        }
    }
}
