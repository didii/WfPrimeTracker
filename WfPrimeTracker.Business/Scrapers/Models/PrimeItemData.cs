using System.Collections.Generic;
using System.IO;

namespace WfPrimeTracker.Business.Scrapers {
    public class PrimeItemData {
        public Stream Image { get; set; }
        public List<PrimePartData> PartsData { get; set; } = new List<PrimePartData>();
    }
}