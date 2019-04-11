using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public class RelicDrop : IPersistentItem {
        /// <inheritdoc />
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [ForeignKey(nameof(Relic))]
        public int RelicId { get; set; }

        public virtual Relic Relic { get; set; }

        [ForeignKey(nameof(PrimePart))]
        public int PrimePartId { get; set; }

        public virtual PrimePart PrimePart { get; set; }

        public DropChance DropChance { get; set; }

    }
}