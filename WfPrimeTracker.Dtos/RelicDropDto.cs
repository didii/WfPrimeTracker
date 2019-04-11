using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Dtos {
    public class RelicDropDto {
        public int Id { get; set; }
        public DropChance DropChance { get; set; }
        public RelicDto Relic { get; set; }
    }
}