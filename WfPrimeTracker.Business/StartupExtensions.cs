using System;
using System.Collections.Generic;
using System.Text;
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
            services.AddTransient<IVoidRelicByRewardScraper, VoidRelicByRewardsScraper>();

            // Services
            services.AddTransient<IPrimeItemService, PrimeItemService>();

            // Automapper
            services.AddSingleton(new MapperConfiguration(conf => conf.AddProfile(typeof(MapperProfile))).CreateMapper());

            // Hangfire jobs
            services.AddTransient<IScraperJob, ScraperJob>();

            // Data services
            services.AddDataServices(configuration);
        }
    }
}
