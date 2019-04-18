using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Business.Scrapers;
using WfPrimeTracker.Data;
using WfPrimeTracker.Data.Repositories;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Jobs {
    internal class ScraperJob : IScraperJob {
        private const string SimpleRewardsTableUrl = "https://warframe.fandom.com/wiki/Void_Relic/ByRewards/SimpleTable";
        private const string BlueprintImageUrl = "https://vignette.wikia.nocookie.net/warframe/images/9/98/Blueprint.png/revision/latest?cb=20140312082301";

        private readonly IHtmlDocumentFetcher _fetcher;
        private readonly IElementFinder _finder;
        private readonly IElementParser _parser;
        private readonly IDataPersister _persister;
        private readonly IPrimeItemScraper _itemScraper;
        private readonly PrimeContext _context;

        public ScraperJob(IHtmlDocumentFetcher fetcher,
                          IElementFinder finder,
                          IElementParser parser,
                          IDataPersister persister,
                          IPrimeItemScraper itemScraper,
                          PrimeContext context) {
            _fetcher = fetcher;
            _finder = finder;
            _parser = parser;
            _persister = persister;
            _itemScraper = itemScraper;
            _context = context;
        }

        /// <inheritdoc />
        public async Task Invoke(PerformContext context) {
            context.WriteLine($"> Fetching page {SimpleRewardsTableUrl}");
            var document = await _fetcher.GetPage(SimpleRewardsTableUrl);
            context.WriteLine($"< Fetched page");

            context.WriteLine($"> Searching for table element and its rows");
            var table = _finder.GetTableOfSimpleRewardsTable(document);
            var rows = _finder.GetRowOfSimpleRewardsTable(table).ToArray();
            var rowCount = rows.Length;
            context.WriteLine($"< Found {rowCount} rows");

            context.WriteLine($"> Start parsing rows");
            var primeItems = new Dictionary<int, PrimeItem>();
            var progressBar = context.WriteProgressBar();
            var count = 0;
            foreach (var row in rows) {
                var rowData = _parser.ParseRowOfSimpleRewardsTable(row);
                if (rowData == null) continue;
                var primeItem = await _persister.AddOrUpdateRowData(rowData);
                primeItems.TryAdd(primeItem.Id, primeItem);
                count++;
                progressBar.SetValue(100 * count / rowCount);
            }

            progressBar.SetValue(100);
            var primeItemsCount = primeItems.Count;
            context.WriteLine($"< Parsed all rows and found {primeItemsCount} prime items");

            context.WriteLine($"> Saving data");
            var rowsChanged = await _context.SaveChangesAsync();
            context.WriteLine($"< Data saved: {rowsChanged} rows changed");

            context.WriteLine($"> Start fetching data per item");
            progressBar = context.WriteProgressBar();
            count = 0;
            foreach (var item in primeItems.Values) {
                var itemData = await _itemScraper.GetData(item.WikiUrl);
                var toSave = await _persister.AddOrUpdatePrimeItem(item, itemData);
                count++;
                progressBar.SetValue(100 * count / primeItemsCount);
            }
            progressBar.SetValue(100);
            context.WriteLine($"< Done setting item data");

            context.WriteLine($"> Saving data");
            rowsChanged = await _context.SaveChangesAsync();
            context.WriteLine($"< Data saved: {rowsChanged} rows changed");

            context.WriteLine($"> Get Blueprint part and attach image");
            var blueprintPart = await _context.PrimeParts.FirstOrDefaultAsync(p => p.Name == "Blueprint");
            byte[] data;
            using (var webClient = new WebClient()) {
                var stream = webClient.OpenRead(new Uri(BlueprintImageUrl));
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                data = memoryStream.ToArray();
            }
            await _persister.AddOrUpdateImage(new Image() {
                Data = data,
                PrimePart = blueprintPart,
            });
            await _context.SaveChangesAsync();
            context.WriteLine($"< Image attached");
        }
    }
}