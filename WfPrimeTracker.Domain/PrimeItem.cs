using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public class PrimeItem : IPersistentItem {
        /// <inheritdoc />
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string WikiUrl { get; set; }

        public virtual ICollection<PrimePart> PrimeParts { get; set; }

        public virtual int Credits { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }

        [ForeignKey(nameof(Image))]
        public int ImageId { get; set; }

        public virtual Image Image { get; set; }
    }
}