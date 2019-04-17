using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public class PrimePartIngredient : IPersistentItem {
        /// <inheritdoc />
        public int Id { get; set; }

        [ForeignKey(nameof(PrimeItem))]
        public int PrimeItemId { get; set; }

        public virtual PrimeItem PrimeItem { get; set; }

        [ForeignKey(nameof(PrimePart))]
        public int PrimePartId { get; set; }

        public virtual PrimePart PrimePart { get; set; }

        public int Count { get; set; }

        public virtual ICollection<RelicDrop> RelicDrops { get; set; }
    }
}