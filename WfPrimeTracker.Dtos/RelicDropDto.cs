using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Dtos {
    public class RelicDropDto {
        public DropChance DropChance { get; set; }
        public RelicDto Relic { get; set; }
    }
}