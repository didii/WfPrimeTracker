using System;
using System.Linq.Expressions;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    class IngredientsGroupRepository : PersistentRepository<IngredientsGroup> {
        /// <inheritdoc />
        public IngredientsGroupRepository(PrimeContext context) : base(context) { }

        /// <inheritdoc />
        protected override Expression<Func<IngredientsGroup, IngredientsGroup, IngredientsGroup>> Updater {
            get {
                return (db, item) => new IngredientsGroup() {
                    Name = item.Name,
                    PrimeItemId = item.PrimeItemId,
                };
            }
        }

        /// <inheritdoc />
        protected override int PropertyUpdateCount => 2;
    }
}