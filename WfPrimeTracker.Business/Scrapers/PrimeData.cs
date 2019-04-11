using System;
using System.Collections.Generic;
using System.Text;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Scrapers {
    class PrimeData {
        public IEnumerable<Ingredient> Ingredients { get; set; }
        public IEnumerable<PrimeItem> PrimeItems { get; set; }
        public IEnumerable<PrimePart> PrimeParts { get; set; }
        public IEnumerable<Relic> Relics { get; set; }
        public IEnumerable<RelicDrop> RelicDrops { get; set; }
        public IEnumerable<Image> Images { get; set; }
    }
}
