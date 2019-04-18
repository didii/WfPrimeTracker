﻿using System.Threading.Tasks;
using Hangfire;
using Hangfire.Server;

namespace WfPrimeTracker.Business.Jobs {
    internal class FullScraperJob : IFullScraperJob {
        /// <inheritdoc />
        public async Task Invoke(PerformContext context) {
            var relicRewardsJob = BackgroundJob.Enqueue<IRelicRewardsScraperJob>(job => job.Invoke(null));
            var primeItemsJob =
                BackgroundJob.ContinueWith<IPrimeItemsScraperJob>(relicRewardsJob, job => job.Invoke(null));
            var blueprintJob = BackgroundJob.ContinueWith<IBlueprintScraperJob>(primeItemsJob, job => job.Invoke(null));
        }
    }
}