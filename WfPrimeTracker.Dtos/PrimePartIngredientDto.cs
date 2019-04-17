using System.Collections.Generic;

namespace WfPrimeTracker.Dtos {
    public class PrimePartIngredientDto {
        public int Id { get; set; }
        public PrimePartDto PrimePart { get; set; }
        public ICollection<RelicDropDto> RelicDrops { get; set; }
        public int Count { get; set; }
    }
}