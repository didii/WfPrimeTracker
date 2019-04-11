using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WfPrimeTracker.Business.Scrapers {
    internal class PrimeItemScraper : IPrimeItemScraper {
        private const string BaseUrl = "https://warframe.fandom.com";

        private static readonly HttpClient Client = new HttpClient();

        private readonly Dictionary<string, PrimeItemData> _data = new Dictionary<string, PrimeItemData>();

        public async Task<PrimeItemData> GetData(string wikiUrl) {
            if (_data.TryGetValue(wikiUrl, out var cached)) {
                return cached;
            }

            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync($"{BaseUrl}{wikiUrl}");

            // Select where image should normally be
            var img = doc.DocumentNode.SelectSingleNode(@"//*[@id='mw-content-text']//aside//img");
            string src;
            if (img != null) {
                src = img.Attributes["data-src"]?.Value; // In case of prime item
                if (string.IsNullOrEmpty(src)) {
                    // In case of prime warframe
                    src = img.Attributes["src"]?.Value;
                }
            } else {
                // In case of kavasa prime collar
                img = doc.DocumentNode.SelectSingleNode(@"//*[@id='mw-content-text']/div[1]/a/img");
                src = img.Attributes["data-src"].Value;
            }

            var result = new PrimeItemData();
            using (var webClient = new WebClient()) {
                result.Image = webClient.OpenRead(new Uri(src));
            }

            var cells = doc.DocumentNode.SelectNodes(@"//*[@id='mw-content-text']/table[@class='foundrytable']//td");
            foreach (var cell in cells) {
                var a = cell.SelectSingleNode(@"./a");
                if (a == null) continue;
                var title = a.Attributes["title"]?.Value;
                if (string.IsNullOrEmpty(title)) continue;
                var innerText = cell.InnerText.Replace(",", "").Trim();
                if (int.TryParse(innerText, out var count)) {
                    result.PartsData.Add(new PrimePartData() {
                        Name = title,
                        Count = count,
                    });
                }
            }

            return result;
        }
    }

    public class PrimeItemData {
        public Stream Image { get; set; }
        public List<PrimePartData> PartsData { get; set; } = new List<PrimePartData>();
    }

    public class PrimePartData {
        public string Name { get; set; }
        public int Count { get; set; }
    }
}