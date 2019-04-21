using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Data;

namespace WfPrimeTracker.Business.Jobs {
    class ResetDataJob : IResetDataJob {
        private readonly PrimeContext _context;

        public ResetDataJob(PrimeContext context) {
            _context = context;
        }

        /// <inheritdoc />
        public async Task Invoke(PerformContext context) {
            // Schedule full scraper job as continuation job
            BackgroundJob.ContinueWith<IFullScraperJob>(context.BackgroundJob.Id, job => job.Invoke(null));

            // Start cleaning the data
            context.WriteLine("> Clearing data");
            var entityTypes = _context.Model.GetEntityTypes();
            var rows = await _context.Database.ExecuteSqlCommandAsync(@"
DELETE FROM dbo.RelicDrop;
DELETE FROM dbo.ResourceIngredient;
DELETE FROM dbo.PrimePartIngredient;
DELETE FROM dbo.Relics;
DELETE FROM dbo.IngredientsGroup;
DELETE FROM dbo.Resources;
DELETE FROM dbo.PrimeParts;
DELETE FROM dbo.PrimeItems;
DELETE FROM dbo.Images;"
            );
            context.WriteLine($"< Rows changed: {rows}");

        }
    }
}