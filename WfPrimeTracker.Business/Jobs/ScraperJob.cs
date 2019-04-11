using System.Threading.Tasks;
using WfPrimeTracker.Business.Scrapers;
using WfPrimeTracker.Data;
using WfPrimeTracker.Data.Repositories;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Jobs {
    internal class ScraperJob : IScraperJob {
        private readonly IVoidRelicByRewardScraper _scraper;
        private readonly IRepository<PrimeItem> _primeItemRepo;
        private readonly IRepository<PrimePart> _primePartRepo;
        private readonly IRepository<Relic> _relicRepo;
        private readonly IRepository<RelicDrop> _relicDropRepo;
        private readonly IRepository<Ingredient> _ingredientRepo;
        private readonly IRepository<Image> _imageRepo;

        public ScraperJob(IVoidRelicByRewardScraper scraper,
                          IRepository<PrimeItem> primeItemRepo,
                          IRepository<PrimePart> primePartRepo,
                          IRepository<Relic> relicRepo,
                          IRepository<RelicDrop> relicDropRepo,
                          IRepository<Ingredient> ingredientRepo,
                          IRepository<Image> imageRepo) {
            _scraper = scraper;
            _primeItemRepo = primeItemRepo;
            _primePartRepo = primePartRepo;
            _relicRepo = relicRepo;
            _relicDropRepo = relicDropRepo;
            _ingredientRepo = ingredientRepo;
            _imageRepo = imageRepo;
        }

        /// <inheritdoc />
        public async Task Invoke() {
            // Get the data
            var primeData = await _scraper.GetItemData();

            // Upsert all data, order is important!
            await _imageRepo.AddOrUpdateMany(primeData.Images);
            await _primeItemRepo.AddOrUpdateMany(primeData.PrimeItems);
            await _primePartRepo.AddOrUpdateMany(primeData.PrimeParts);
            await _relicRepo.AddOrUpdateMany(primeData.Relics);
            await _relicDropRepo.AddOrUpdateMany(primeData.RelicDrops);
            await _ingredientRepo.AddOrUpdateMany(primeData.Ingredients);
            //await Task.WhenAll(primeItemTask, primePartTask, relicTask, relicDropTask);

            await _primeItemRepo.SaveChangesAsync();
        }
    }
}