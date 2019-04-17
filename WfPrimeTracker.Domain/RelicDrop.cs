using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public class RelicDrop {
        [ForeignKey(nameof(Relic))]
        public int RelicId { get; set; }

        public Relic Relic { get; set; }

        [ForeignKey(nameof(PrimePartIngredient))]
        public int PrimePartIngredientId { get; set; }

        public virtual PrimePartIngredient PrimePartIngredient { get; set; }

        public virtual DropChance DropChance { get; set; }
    }
}