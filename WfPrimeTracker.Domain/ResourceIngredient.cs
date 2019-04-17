using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public class ResourceIngredient {
        [ForeignKey(nameof(Resource))]
        public int ResourceId { get; set; }

        public virtual Resource Resource { get; set; }

        [ForeignKey(nameof(IngredientsGroup))]
        public int IngredientsGroupId { get; set; }

        public virtual IngredientsGroup IngredientsGroup { get; set; }

        public int Count { get; set; }
    }
}