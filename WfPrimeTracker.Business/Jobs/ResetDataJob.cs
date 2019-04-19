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
DROP TABLE dbo.RelicDrop;
GO;
DROP TABLE dbo.ResourceIngredient;
GO;
DROP TABLE dbo.PrimePartIngredient;
GO;
DROP TABLE dbo.Relics;
GO;
DROP TABLE dbo.IngredientsGroup;
GO;
DROP TABLE dbo.Resources;
GO;
DROP TABLE dbo.PrimeParts;
GO;
DROP TABLE dbo.PrimeItems;
GO;
DROP TABLE dbo.Images;
GO;"
            );
            context.WriteLine($"< Rows changed: {rows}");

        }
    }
}