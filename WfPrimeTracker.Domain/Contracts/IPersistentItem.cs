using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WfPrimeTracker.Domain {
    public interface IPersistentItem {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        int Id { get; set; }
    }
}