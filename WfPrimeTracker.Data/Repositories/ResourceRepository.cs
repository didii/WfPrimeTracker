using System;
using System.Linq.Expressions;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    public class ResourceRepository : PersistentRepository<Resource> {
        /// <inheritdoc />
        public ResourceRepository(PrimeContext context) : base(context) { }

        /// <inheritdoc />
        protected override Expression<Func<Resource, Resource, Resource>> Updater {
            get {
                return (db, item) => new Resource() {
                    Name = item.Name,
                    ImageId = item.ImageId,
                };
            }
        }

        /// <inheritdoc />
        protected override int PropertyUpdateCount => 2;
    }
}