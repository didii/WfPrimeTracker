using System.Collections.Generic;

namespace WfPrimeTracker.Dtos {
    public class PrimeItemDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WikiUrl { get; set; }
        public ICollection<PrimePartIngredientDto> PrimePartIngredients { get; set; }
        public ICollection<IngredientsGroupDto> IngredientsGroups { get; set; }
    }
}