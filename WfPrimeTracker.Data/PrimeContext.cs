﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WfPrimeTracker.Domain;
using WfPrimeTracker.Domain.Contracts;
using WfPrimeTracker.Domain.Users;

namespace WfPrimeTracker.Data {
    public class PrimeContext : DbContext {
        public PrimeContext(DbContextOptions<PrimeContext> options) : base(options) { }

        public DbSet<PrimeItem> PrimeItems { get; set; }

        public DbSet<PrimePart> PrimeParts { get; set; }

        public DbSet<Relic> Relics { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<User> Users { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Entities with composite primary keys
            modelBuilder.Entity<RelicDrop>().HasKey(d => new { d.RelicId, d.PrimePartIngredientId });
            modelBuilder.Entity<ResourceIngredient>().HasKey(i => new { i.IngredientsGroupId, i.ResourceId });

            // One-to-one relations to Image
            modelBuilder.Entity<Image>().HasMany(image => image.PrimeItem).WithOne(item => item.Image).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Image>().HasMany(image => image.PrimePart).WithOne(part => part.Image).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Image>().HasMany(image => image.Relic).WithOne(relic => relic.Image).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Image>().HasMany(image => image.Resource).WithOne(resource => resource.Image).OnDelete(DeleteBehavior.SetNull);

            // Set the persistent item ID's to generate never so we can manually assign them
            modelBuilder.Entity<PrimeItem>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<PrimePart>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<PrimePartIngredient>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<Relic>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<IngredientsGroup>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<Resource>().Property(i => i.Id).ValueGeneratedNever();
            modelBuilder.Entity<Image>().Property(i => i.Id).ValueGeneratedNever();

            // Set User tables
            modelBuilder.Entity<UserPrimeItemSaveData>().HasKey(d => new { d.UserId, d.PrimeItemId });
            modelBuilder.Entity<UserPrimePartIngredientSaveData>().HasKey(d => new { d.UserId, d.PrimePartIngredientId });
        }

        public override int SaveChanges() {
            UpdateEntities(ChangeTracker.Entries());
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            UpdateEntities(ChangeTracker.Entries());
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateEntities(IEnumerable<EntityEntry> entries) {
            var now = DateTime.Now;
            foreach (var entry in entries) {
                if (entry.Entity is ITrackable trackable) {
                    if (entry.State == EntityState.Added) {
                        trackable.CreatedOn = now;
                        trackable.ModifiedOn = now;
                    } else if (entry.State == EntityState.Modified) {
                        trackable.ModifiedOn = now;
                    }
                }
            }
        }
    }
}