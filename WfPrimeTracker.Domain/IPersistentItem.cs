using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WfPrimeTracker.Domain {
    public interface IPersistentItem {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        int Id { get; }
    }
}
