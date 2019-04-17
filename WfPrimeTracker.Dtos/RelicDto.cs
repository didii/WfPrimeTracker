using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Dtos {
    public class RelicDto {
        public int Id { get; set; }
        public RelicTier Tier { get; set; }
        public string Name { get; set; }
        public bool IsVaulted { get; set; }
        public string WikiUrl { get; set; }
    }
}