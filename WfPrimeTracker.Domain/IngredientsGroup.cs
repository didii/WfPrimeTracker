using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public class IngredientsGroup : IPersistentItem {
        /// <inheritdoc />
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(PrimeItem))]
        public int PrimeItemId { get; set; }

        public virtual PrimeItem PrimeItem { get; set; }

        public virtual ICollection<ResourceIngredient> ResourceIngredients { get; set; }
    }
}