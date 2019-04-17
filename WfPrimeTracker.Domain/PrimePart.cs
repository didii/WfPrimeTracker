using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public class PrimePart : IPersistentItem {
        /// <inheritdoc />
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(Image))]
        public int? ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<PrimePartIngredient> PrimePartIngredients { get; set; }
    }
}