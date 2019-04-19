using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using WfPrimeTracker.Business.Scrapers;
using WfPrimeTracker.Data;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Tests {
    public class PrimeItemScraperTests {
        private PrimeItemScraper _sut;
        private PrimeContext _context;
        private IIdProvider _idProvider;
        private IFieldUpdater _updater;
        private IDataPersister _persister;

        [SetUp]
        public void Setup() {
            _sut = new PrimeItemScraper();
            _context = new PrimeContext(new DbContextOptionsBuilder<PrimeContext>()
                                       .UseSqlServer("Data Source=ORD03016\\SQLEXPRESS;Integrated Security=True;Initial Catalog=WfPrimeTracker")
                                       .Options);
            _idProvider = new IdProvider();
            _updater = new FieldUpdater();
            _persister = new DataPersister(_context, _idProvider, _updater);
        }

        [TestCase("/wiki/Ash/Prime")]
        [TestCase("/wiki/Akbolto_Prime")]
        [TestCase("/wiki/Akbronco_Prime")]
        [TestCase("/wiki/Kavasa_Prime_Collar")]
        [TestCase("/wiki/Odonata/Prime")]
        public async Task GetItemData(string url) {
            //Act
            var data = await _sut.GetData(url);
        }

        [TestCase(1481194682, "/wiki/Akbolto_Prime")]
        public async Task SaveItemData(int primeItemId, string url) {
            // Arrange
            var primeItem = await _context.Set<PrimeItem>()
                                          .Include(i => i.PrimePartIngredients)
                                          .ThenInclude(i => i.PrimePart)
                                          .ThenInclude(p => p.Image)
                                          .Include(i => i.PrimePartIngredients)
                                          .ThenInclude(i => i.RelicDrops)
                                          .ThenInclude(r => r.Relic)
                                          .Include(i => i.IngredientsGroups)
                                          .ThenInclude(g => g.ResourceIngredients)
                                          .ThenInclude(i => i.Resource)
                                          .FirstOrDefaultAsync(i => i.Id == primeItemId);

            //Act
            var data = await _sut.GetData(url);
            await _persister.AddOrUpdatePrimeItem(primeItem, data);

            await _context.SaveChangesAsync();
        }
    }
}