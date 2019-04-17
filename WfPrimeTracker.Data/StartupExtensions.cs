using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WfPrimeTracker.Data.Repositories;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data {
    public static class StartupExtensions {
        public static void AddDataServices(this IServiceCollection services, IConfiguration configuration) {
            // Repositories
            services.AddTransient<IPersistentRepository<Image>, ImageRepository>();
            services.AddTransient<IPersistentRepository<IngredientsGroup>, IngredientsGroupRepository>();
            services.AddTransient<IPersistentRepository<PrimeItem>, PrimeItemRepository>();
            services.AddTransient<IPersistentRepository<PrimePart>, PrimePartRepository>();
            services.AddTransient<IPersistentRepository<Relic>, RelicRepository>();
            services.AddTransient<IPersistentRepository<Resource>, ResourceRepository>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            // Context
            var connectionString = configuration.GetConnectionString("Default");
            services.AddDbContext<PrimeContext>(options => {
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging();
            });
            services.AddDbContext<InMemoryPrimeContext>(options => options.UseInMemoryDatabase("primeContext"));
        }
    }
}