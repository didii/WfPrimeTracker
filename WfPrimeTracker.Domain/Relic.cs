using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public class Relic : IPersistentItem {
        /// <inheritdoc />
        public int Id { get; set; }

        public RelicTier Tier { get; set; }
        public string Name { get; set; }
        public string WikiUrl { get; set; }
        public bool IsVaulted { get; set; }

        [ForeignKey(nameof(Image))]
        public int? ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<RelicDrop> RelicDrops { get; set; }
    }
}