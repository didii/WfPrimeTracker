using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Business.Scrapers;
using WfPrimeTracker.Data;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Jobs {
    class BlueprintScraperJob : IBlueprintScraperJob {
        private const string BlueprintImageUrl = "https://vignette.wikia.nocookie.net/warframe/images/9/98/Blueprint.png/revision/latest?cb=20140312082301";

        private readonly IDataPersister _persister;
        private readonly PrimeContext _context;

        public BlueprintScraperJob(IDataPersister persister, PrimeContext context) {
            _persister = persister;
            _context = context;
        }

        /// <inheritdoc />
        public async Task Invoke(PerformContext context) {
            context.WriteLine($"> Get Blueprint part and attach image");
            byte[] data;
            using (var webClient = new WebClient()) {
                var stream = webClient.OpenRead(new Uri(BlueprintImageUrl));
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                data = memoryStream.ToArray();
            }
            var blueprintImage = await _persister.AddOrUpdateImage(new Image() {
                Data = data,
            });
            var blueprintPart = await _context.PrimeParts.FirstOrDefaultAsync(p => p.Name == "Blueprint");
            blueprintPart.ImageId = blueprintImage.Id;
            blueprintPart.Image = blueprintImage;
            var collarBlueprintPart = await _context.PrimeParts.FirstOrDefaultAsync(p => p.Name == "Collar Blueprint");
            collarBlueprintPart.ImageId = blueprintImage.Id;
            collarBlueprintPart.Image = blueprintImage;
            await _context.SaveChangesAsync();
            context.WriteLine($"< Image attached");
        }
    }
}