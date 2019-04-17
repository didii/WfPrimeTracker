using System;
using System.Linq.Expressions;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    class RelicRepository : PersistentRepository<Relic> {
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
                    ImageId = item.ImageId,
                };
            }
        }

        /// <inheritdoc />
        protected override int PropertyUpdateCount => 5;
    }
}