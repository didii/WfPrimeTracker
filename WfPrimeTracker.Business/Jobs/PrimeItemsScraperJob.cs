using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Business.Scrapers;
using WfPrimeTracker.Data;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Jobs {
    class PrimeItemsScraperJob : IPrimeItemsScraperJob {
        private readonly IDataPersister _persister;
        private readonly PrimeContext _context;
        private readonly IPrimeItemScraper _itemScraper;

        public PrimeItemsScraperJob(IPrimeItemScraper itemScraper,
                                    IDataPersister persister,
                                    PrimeContext context) {
            _persister = persister;
            _context = context;
            _itemScraper = itemScraper;
        }

        /// <inheritdoc />
        public async Task Invoke(PerformContext context) {
            List<Exception> errors = new List<Exception>();

            context.WriteLine($"> Fetch all prime items");
            var primeItems = await _context.Set<PrimeItem>()
                                           .Include(item => item.PrimePartIngredients)
                                           .ThenInclude(i => i.PrimePart)
                                           .Include(part => part.PrimePartIngredients)
                                           .ThenInclude(i => i.RelicDrops)
                                           .ThenInclude(drop => drop.Relic)
                                           .Include(item => item.IngredientsGroups)
                                           .ThenInclude(g => g.ResourceIngredients)
                                           .ThenInclude(i => i.Resource)
                                           .ToArrayAsync();
            var primeItemsCount = primeItems.Length;
            context.WriteLine($"< Found {primeItemsCount} items");

            context.WriteLine($"> Start fetching data per item");
            var progressBar = context.WriteProgressBar();
            var count = 0;
            foreach (var item in primeItems) {
                try {
                    var itemData = await _itemScraper.GetData(item.WikiUrl);
                    var toSave = await _persister.AddOrUpdatePrimeItem(item, itemData);
                }
                catch (Exception ex) {
                    errors.Add(ex);
                    context.SetTextColor(ConsoleTextColor.Red);
                    context.WriteLine("Error occured when trying to parse " + item.Name);
                    context.WriteLine(ex);
                    context.WriteLine("");
                    context.ResetTextColor();
                }
                count++;
                progressBar.SetValue(100 * count / primeItemsCount);
            }
            progressBar.SetValue(100);
            context.WriteLine($"< Done setting item data");

            context.WriteLine($"> Saving data");
            var rowsChanged = await _context.SaveChangesAsync();
            context.WriteLine($"< Data saved: {rowsChanged} rows changed");

            if (errors.Count > 0) {
                context.SetTextColor(ConsoleTextColor.Red);
                throw new AggregateException(errors);
            }
        }
    }
}