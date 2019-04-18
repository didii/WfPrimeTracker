using System.Collections.Generic;
using System.IO;

namespace WfPrimeTracker.Business.Scrapers {
    public class PrimeItemData {
        public Stream Image { get; set; }
        public Dictionary<string, List<PrimePartData>> PartsData { get; set; } = new Dictionary<string, List<PrimePartData>>();
    }
}