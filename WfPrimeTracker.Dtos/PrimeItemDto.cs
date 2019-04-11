using System.Collections.Generic;

namespace WfPrimeTracker.Dtos {
    public class PrimeItemDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WikiUrl { get; set; }
        public ICollection<PrimePartDto> PrimeParts { get; set; }
        public int Credits { get; set; }
        public ICollection<IngredientDto> Ingredients { get; set; }
    }
}