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
                if (item.PrimePartIngredients == null)
                    continue;
                item.PrimePartIngredients =
                    item.PrimePartIngredients.OrderBy(ingredient => ingredient.PrimePart.Name).ToList();
                foreach (var ingredient in item.PrimePartIngredients) {
                    if (ingredient.RelicDrops == null)
                        continue;
                    ingredient.RelicDrops = ingredient.RelicDrops
                                                      .OrderBy(x => x.DropChance)
                                                      .ThenBy(x => x.Relic.Tier)
                                                      .ThenBy(x => x.Relic.Name)
                                                      .ToList();
                }
            }

            return result;
        }
    }
}
