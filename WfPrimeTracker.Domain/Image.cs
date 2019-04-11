using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WfPrimeTracker.Domain {
    public class Image : IPersistentItem {
        /// <inheritdoc />
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public PrimeItem PrimeItem { get; set; }

        public byte[] Data { get; set; }
    }
}
