using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public class PrimePart : IPersistentItem {
        /// <inheritdoc />
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        [ForeignKey(nameof(PrimeItem))]
        public int PrimeItemId { get; set; }

        public virtual PrimeItem PrimeItem { get; set; }

        public virtual ICollection<RelicDrop> RelicDrops { get; set; }

    }
}