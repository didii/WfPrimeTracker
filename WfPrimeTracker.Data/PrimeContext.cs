using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data {
    public class PrimeContext : DbContext {
        public PrimeContext(DbContextOptions<PrimeContext> options) : base(options) { }

        public DbSet<PrimeItem> PrimeItems { get; set; }

        public DbSet<PrimePart> PrimeParts { get; set; }

        public DbSet<Relic> Relics { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Image> Images { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Entities with composite primary keys
            modelBuilder.Entity<RelicDrop>().HasKey(d => new { d.RelicId, d.PrimePartIngredientId });
            modelBuilder.Entity<ResourceIngredient>().HasKey(i => new { i.IngredientsGroupId, i.ResourceId });

            // One-to-one relations to Image
            modelBuilder.Entity<Image>().HasOne(image => image.PrimeItem).WithOne(item => item.Image).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Image>().HasOne(image => image.PrimePart).WithOne(part => part.Image).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Image>().HasOne(image => image.Relic).WithOne(relic => relic.Image).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Image>().HasOne(image => image.Resource).WithOne(resource => resource.Image).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PrimeItem>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<PrimePart>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<PrimePartIngredient>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<Relic>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<IngredientsGroup>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<Resource>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<Image>().Property(i => i.Id).ValueGeneratedNever();
        }
    }
}