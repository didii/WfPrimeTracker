using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    class PrimeItemRepository : PersistentRepository<PrimeItem> {
        /// <inheritdoc />
        public PrimeItemRepository(PrimeContext context) : base(context) { }

        /// <inheritdoc />
        protected override Expression<Func<PrimeItem, PrimeItem, PrimeItem>> Updater {
            get {
                return (db, item) => new PrimeItem() {
                    Name = item.Name,
                    WikiUrl = item.WikiUrl,
                    ImageId = item.ImageId,
                };
            }
        }

        /// <inheritdoc />
        protected override int PropertyUpdateCount => 3;

        /// <inheritdoc />
        public override async Task<IEnumerable<PrimeItem>> GetAll(Includes<PrimeItem> includes = null) {
            var result = await GetBaseQuery(includes).OrderBy(item => item.Name)
                                                     .ToArrayAsync();

            foreach (var item in result) {
                if (item.PrimePartIngredients != null) {
                    item.PrimePartIngredients = item.PrimePartIngredients.OrderBy(ingredient => ingredient.PrimePart.Name).ToList();
                    foreach (var ingredient in item.PrimePartIngredients) {
                        if (ingredient.RelicDrops != null)
                            ingredient.RelicDrops = ingredient.RelicDrops
                                                              .OrderBy(x => x.DropChance)
                                                              .ThenBy(x => x.Relic.Tier)
                                                              .ThenBy(x => x.Relic.Name)
                                                              .ToList();
                    }
                }
                if (item.IngredientsGroups != null) {
                    item.IngredientsGroups = item.IngredientsGroups.OrderBy(g => g.Name).ToList();
                    foreach (var ingredientGroup in item.IngredientsGroups) {
                        if (ingredientGroup.ResourceIngredients != null) {
                            ingredientGroup.ResourceIngredients = ingredientGroup
                                                                 .ResourceIngredients
                                                                 .OrderBy(x => x.Resource, new ResourceComparer())
                                                                 .ToList();
                        }
                    }
                }
            }
            return result;
        }
    }

    internal class ResourceComparer : IComparer<Resource> {
        /// <inheritdoc />
        public int Compare(Resource x, Resource y) {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            if (x.Name == "Credits") return -1;
            if (y.Name == "Credits") return 1;
            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }
    }
}
