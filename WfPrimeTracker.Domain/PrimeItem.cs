using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WfPrimeTracker.Domain.Users;

namespace WfPrimeTracker.Domain {
    public class PrimeItem : IPersistentItem {
        /// <inheritdoc />
        public int Id { get; set; }

        public string Name { get; set; }

        public string WikiUrl { get; set; }

        [ForeignKey(nameof(Image))]
        public int? ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<PrimePartIngredient> PrimePartIngredients { get; set; }

        public virtual ICollection<IngredientsGroup> IngredientsGroups { get; set; }

        public virtual ICollection<UserPrimeItemSaveData> UserPrimeItemSaveDatas { get; set; }
    }
}