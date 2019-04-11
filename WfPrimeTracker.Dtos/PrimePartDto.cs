using System;
using System.Collections.Generic;

namespace WfPrimeTracker.Dtos {
    public class PrimePartDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public ICollection<RelicDropDto> RelicDrops { get; set; }
    }
}