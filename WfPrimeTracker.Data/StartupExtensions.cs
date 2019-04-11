using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WfPrimeTracker.Data.Repositories;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data {
    public static class StartupExtensions {
        public static void AddDataServices(this IServiceCollection services, IConfiguration configuration) {
            // Repositories
            services.AddTransient<IRepository<PrimeItem>, PrimeItemRepository>();
            services.AddTransient<IRepository<PrimePart>, PrimePartRepository>();
            services.AddTransient<IRepository<RelicDrop>, RelicDropRepository>();
            services.AddTransient<IRepository<Relic>, RelicRepository>();
            services.AddTransient<IRepository<Image>, ImageRepository>();
            services.AddTransient<IRepository<Ingredient>, IngredientRepository>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            // Context
            var connectionString = configuration.GetConnectionString("Default");
            services.AddDbContext<PrimeContext>(options => {
                options.UseSqlServer(connectionString);
                options.EnableSensitiveDataLogging();
            });
        }
    }
}