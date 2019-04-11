using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    class IngredientRepository : Repository<Ingredient> {
        /// <inheritdoc />
        public IngredientRepository(PrimeContext context) : base(context) { }

        /// <inheritdoc />
        protected override Expression<Func<Ingredient, Ingredient, Ingredient>> Updater {
            get {
                return (db, item) => new Ingredient() {
                    Name = item.Name,
                    Quantity = item.Quantity,
                };
            }
        }

        /// <inheritdoc />
        protected override int PropertyUpdateCount => 2;
    }
}
