using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Data.Repositories {
    class ImageRepository : Repository<Image> {
        /// <inheritdoc />
        public ImageRepository(PrimeContext context) : base(context) { }

        /// <inheritdoc />
        protected override Expression<Func<Image, Image, Image>> Updater {
            get {
                return (db, item) => new Image() {
                    Data = item.Data,
                };
            }
        }

        /// <inheritdoc />
        protected override int PropertyUpdateCount => 1;
    }
}
