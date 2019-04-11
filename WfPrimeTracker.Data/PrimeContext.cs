using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data {
    public class PrimeContext : DbContext {
        public PrimeContext(DbContextOptions<PrimeContext> options) : base(options) { }

        public DbSet<PrimeItem> PrimeItems { get; set; }

        public DbSet<Relic> Relics { get; set; }

        public DbSet<Image> Images { get; set; }
    }
}