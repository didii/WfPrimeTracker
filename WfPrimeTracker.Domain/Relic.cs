using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public class Relic : IPersistentItem {
        /// <inheritdoc />
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public RelicTier Tier { get; set; }
        
        public string Name { get; set; }
        
        public bool IsVaulted { get; set; }
        
        public string WikiUrl { get; set; }
        
        public virtual ICollection<RelicDrop> RelicDrops { get; set; }
    }
}