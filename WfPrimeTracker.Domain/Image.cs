namespace WfPrimeTracker.Domain {
    public class Image : IPersistentItem {
        /// <inheritdoc />
        public int Id { get; set; }

        public byte[] Data { get; set; }

        public virtual PrimeItem PrimeItem { get; set; }

        public virtual PrimePart PrimePart { get; set; }

        public virtual Relic Relic { get; set; }

        public virtual Resource Resource { get; set; }
    }
}