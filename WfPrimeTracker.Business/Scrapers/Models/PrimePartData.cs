﻿using System.IO;

namespace WfPrimeTracker.Business.Scrapers {
    public class PrimePartData {
        public string Name { get; set; }
        public int Count { get; set; }
        public Stream Image { get; set; }
    }
}