using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using WfPrimeTracker.Business.Scrapers;
using WfPrimeTracker.Data;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Jobs {
    class RelicRewardsScraperJob : IRelicRewardsScraperJob {
        private const string SimpleRewardsTableUrl = "https://warframe.fandom.com/wiki/Void_Relic/ByRewards/SimpleTable";
        private readonly IHtmlDocumentFetcher _fetcher;
        private readonly IElementFinder _finder;
        private readonly IElementParser _parser;
        private readonly IDataPersister _persister;
        private readonly PrimeContext _context;


        public RelicRewardsScraperJob(IHtmlDocumentFetcher fetcher,
                                      IElementFinder finder,
                                      IElementParser parser,
                                      IDataPersister persister,
                                      PrimeContext context) {
            _fetcher = fetcher;
            _finder = finder;
            _parser = parser;
            _persister = persister;
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
            int rowsChanged = 0;
            int retriesLeft = 3;
            var errors = new List<Exception>(retriesLeft);
            while (retriesLeft > 0) {
                try {
                    rowsChanged = await _context.SaveChangesAsync();
                }
                catch (Exception ex) {
                    retriesLeft--;
                    errors.Add(ex);
                    context.SetTextColor(ConsoleTextColor.Red);
                    context.WriteLine("An error occured while trying to save");
                    context.WriteLine(ex);
                    context.WriteLine("Retries left: " + retriesLeft);
                    context.ResetTextColor();
                    continue;
                }
                break;
            }
            if (retriesLeft == 0) {
                throw new AggregateException(errors);
            }
            context.WriteLine($"< Data saved: {rowsChanged} rows changed");
        }
    }
}