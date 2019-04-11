using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public class Ingredient : IPersistentItem {
        /// <inheritdoc />
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        [ForeignKey(nameof(PrimeItem))]
        public int PrimeItemId { get; set; }

        public virtual PrimeItem PrimeItem { get; set; }
    }
}