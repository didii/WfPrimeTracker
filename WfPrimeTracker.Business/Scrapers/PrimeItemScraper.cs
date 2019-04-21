using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace WfPrimeTracker.Business.Scrapers {
    internal class PrimeItemScraper : IPrimeItemScraper {
        private const string BaseUrl = "https://warframe.fandom.com";

        private static readonly HttpClient Client = new HttpClient();

        public async Task<PrimeItemData> GetData(string wikiUrl) {
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

            AddPartData(result, doc);

            return result;
        }

        private void AddPartData(PrimeItemData data, HtmlDocument doc) {
            var cells = doc.DocumentNode.SelectNodes(@"//*[@id='mw-content-text']/table[@class='foundrytable']/tr/td");
            var currentKey = "";
            foreach (var cell in cells) {
                // Check if drop locations was found
                if (cell.InnerText.Contains("Drop Locations")) {
                    // End of the important stuff, skip the rest
                    break;
                }

                // Check if next part is encountered
                if (cell.Attributes["colspan"]?.Value == "6") {
                    // Set key if so
                    currentKey = cell.InnerText.Trim();
                    continue;
                }

                // The table always has an anchor if there is an ingredient
                var a = cell.SelectSingleNode(@"./a");
                if (a == null) continue;

                // Get name of part
                var title = a.Attributes["title"]?.Value;
                if (string.IsNullOrEmpty(title)) continue;
                title = HttpUtility.HtmlDecode(title);

                // Get count
                var innerText = cell.InnerText.Replace(",", "").Trim();
                int count;
                if (string.IsNullOrWhiteSpace(innerText)) {
                    count = 1;
                } else if (int.TryParse(innerText, out var tmpCount)) {
                    count = tmpCount;
                } else {
                    continue;
                }

                // Check now if data already exists
                // If it does, we don't bother getting the image again
                if (data.PartsData.TryGetValue(currentKey, out var partDataList)) {
                    var existing = partDataList.FirstOrDefault(p => p.Name == title);
                    if (existing != null) {
                        existing.Count += count;
                        continue;
                    }
                }

                // Get image
                var img = a.SelectSingleNode(@"./img");
                var src = img?.Attributes["data-src"]?.Value ?? img?.Attributes["src"]?.Value;
                if (src == null) continue;
                var regex = new Regex(@"/scale-to-width-down/(\d+)");
                src = regex.Replace(src, "/scale-to-width-down/150", 1);
                Stream image;
                using (var webClient = new WebClient()) {
                    try {
                        image = webClient.OpenRead(new Uri(src));
                    }
                    catch (Exception e) {
                        continue;
                    }
                }

                // Add data
                var partData = new PrimePartData() {
                    Name = title,
                    Count = count,
                    Image = image,
                };
                if (!data.PartsData.ContainsKey(currentKey)) {
                    data.PartsData.Add(currentKey, new List<PrimePartData>());
                }
                data.PartsData[currentKey].Add(partData);
            }

        }
    }
}