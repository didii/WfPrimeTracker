using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WfPrimeTracker.Business.Automapper;
using WfPrimeTracker.Business.Jobs;
using WfPrimeTracker.Business.Scrapers;
using WfPrimeTracker.Business.Services;
using WfPrimeTracker.Data;

namespace WfPrimeTracker.Business {
    public static class StartupExtensions {
        public static void AddBusinessServices(this IServiceCollection services, IConfiguration configuration) {
            // Scrapers or scraper helpers
            services.AddTransient<IIdProvider, IdProvider>();
            services.AddTransient<IPrimeItemScraper, PrimeItemScraper>();
            services.AddTransient<IHtmlDocumentFetcher, HtmlDocumentFetcher>();
            services.AddTransient<IDataPersister, DataPersister>();
            services.AddTransient<IElementFinder, ElementFinder>();
            services.AddTransient<IElementParser, ElementParser>();
            services.AddTransient<IFieldUpdater, FieldUpdater>();

            // Services
            services.AddTransient<IPrimeItemService, PrimeItemService>();
            services.AddTransient<IResourceService, ResourceService>();
            services.AddTransient<IPrimePartService, PrimePartService>();

            // Automapper
            services.AddSingleton(
                new MapperConfiguration(conf => conf.AddProfile(typeof(MapperProfile))).CreateMapper());

            // Hangfire jobs
            services.AddTransient<IFullScraperJob, FullScraperJob>();
            services.AddTransient<IRelicRewardsScraperJob, RelicRewardsScraperJob>();
            services.AddTransient<IPrimeItemsScraperJob, PrimeItemsScraperJob>();
            services.AddTransient<IBlueprintScraperJob, BlueprintScraperJob>();
            services.AddTransient<IResetDataJob, ResetDataJob>();

            // Data services
            services.AddDataServices(configuration);
        }
    }
}