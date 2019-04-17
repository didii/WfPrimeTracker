using System.Collections.Generic;

namespace WfPrimeTracker.Dtos {
    public class IngredientsGroupDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ResourceIngredientDto> ResourceIngredients { get; set; }
    }
}