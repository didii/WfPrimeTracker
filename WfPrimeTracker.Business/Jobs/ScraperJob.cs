using System.Threading.Tasks;
using WfPrimeTracker.Business.Scrapers;
using WfPrimeTracker.Data;
using WfPrimeTracker.Data.Repositories;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Jobs {
    internal class ScraperJob : IScraperJob {
        private readonly IVoidRelicByRewardScraper _scraper;
        private readonly InMemoryPrimeContext _memContext;
        private readonly IPersistentRepository<PrimeItem> _primeItemRepo;
        private readonly IPersistentRepository<PrimePart> _primePartRepo;
        private readonly IPersistentRepository<Relic> _relicRepo;
        private readonly IPersistentRepository<Resource> _resourceRepo;
        private readonly IPersistentRepository<Image> _imageRepo;
        private readonly IPersistentRepository<IngredientsGroup> _ingredientsGroupRepository;

        public ScraperJob(IVoidRelicByRewardScraper scraper,
                          InMemoryPrimeContext memContext,
                          IPersistentRepository<PrimeItem> primeItemRepo,
                          IPersistentRepository<PrimePart> primePartRepo,
                          IPersistentRepository<Relic> relicRepo,
                          IPersistentRepository<Resource> resourceRepo,
                          IPersistentRepository<Image> imageRepo,
                          IPersistentRepository<IngredientsGroup> ingredientsGroupRepository) {
            _scraper = scraper;
            _memContext = memContext;
            _primeItemRepo = primeItemRepo;
            _primePartRepo = primePartRepo;
            _relicRepo = relicRepo;
            _resourceRepo = resourceRepo;
            _imageRepo = imageRepo;
            _ingredientsGroupRepository = ingredientsGroupRepository;
        }

        /// <inheritdoc />
        public async Task Invoke() {
            // Get the data
            await _scraper.FetchAllPrimeItemData();

            // Upsert all data, order is important!
            await _imageRepo.AddOrUpdateMany(_memContext.Set<Image>());
            await _resourceRepo.AddOrUpdateMany(_memContext.Set<Resource>());
            await _relicRepo.AddOrUpdateMany(_memContext.Set<Relic>());
            await _primePartRepo.AddOrUpdateMany(_memContext.Set<PrimePart>());
            await _primeItemRepo.AddOrUpdateMany(_memContext.Set<PrimeItem>());
            await _ingredientsGroupRepository.AddOrUpdateMany(_memContext.Set<IngredientsGroup>());

            await _primeItemRepo.SaveChangesAsync();
        }
    }
}