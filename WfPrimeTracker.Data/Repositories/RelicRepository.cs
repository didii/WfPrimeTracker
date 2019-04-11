using System;
using System.Linq.Expressions;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    class RelicRepository : Repository<Relic> {
        /// <inheritdoc />
        public RelicRepository(PrimeContext context) : base(context) { }

        /// <inheritdoc />
        protected override Expression<Func<Relic, Relic, Relic>> Updater {
            get {
                return (db, item) => new Relic() {
                    Name = item.Name,
                    Tier = item.Tier,
                    WikiUrl = item.WikiUrl,
                    IsVaulted = item.IsVaulted,
                };
            }
        }

        /// <inheritdoc />
        protected override int PropertyUpdateCount => 4;
    }
}