using System;
using System.Linq.Expressions;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    class RelicDropRepository : Repository<RelicDrop> {
        /// <inheritdoc />
        public RelicDropRepository(PrimeContext context) : base(context) { }

        /// <inheritdoc />
        protected override Expression<Func<RelicDrop, RelicDrop, RelicDrop>> Updater {
            get {
                return (db, item) => new RelicDrop() {
                    DropChance = item.DropChance,
                    PrimePartId = item.PrimePartId,
                    RelicId = item.RelicId,
                };
            }
        }

        /// <inheritdoc />
        protected override int PropertyUpdateCount => 3;
    }
}