using System;
using System.Linq.Expressions;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    class PrimePartRepository : PersistentRepository<PrimePart> {
        /// <inheritdoc />
        public PrimePartRepository(PrimeContext context) : base(context) { }

        /// <inheritdoc />
        protected override Expression<Func<PrimePart, PrimePart, PrimePart>> Updater {
            get {
                return (db, item) => new PrimePart() {
                    Name = item.Name,
                    ImageId = item.ImageId,
                };
            }
        }

        /// <inheritdoc />
        protected override int PropertyUpdateCount => 2;
    }
}