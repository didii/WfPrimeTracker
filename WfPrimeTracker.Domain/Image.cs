using System.Collections.Generic;

namespace WfPrimeTracker.Domain {
    public class Image : IPersistentItem {
        /// <inheritdoc />
        public int Id { get; set; }

        public byte[] Data { get; set; }

        public virtual ICollection<PrimeItem> PrimeItem { get; set; }

        public virtual ICollection<PrimePart> PrimePart { get; set; }

        public virtual ICollection<Relic> Relic { get; set; }

        public virtual ICollection<Resource> Resource { get; set; }
    }
}