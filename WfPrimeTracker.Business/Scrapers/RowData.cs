using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Scrapers {
    public class RowData {
        public string ItemName { get; set; }
        public string ItemUrl { get; set; }
        public string PartName { get; set; }
        public RelicTier? RelicTier { get; set; }
        public string RelicName { get; set; }
        public string RelicUrl { get; set; }
        public DropChance? DropChance { get; set; }
        public bool? IsVaulted { get; set; }
    }
}
