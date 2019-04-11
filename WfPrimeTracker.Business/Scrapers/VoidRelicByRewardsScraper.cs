using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using WfPrimeTracker.Domain;
using WfPrimeTracker.Dtos;

namespace WfPrimeTracker.Business.Scrapers {
    internal class VoidRelicByRewardsScraper : IVoidRelicByRewardScraper {
        private readonly IPrimeItemScraper _primeItemScraper;
        private readonly IIdProvider _idProvider;
        private const string Url = "https://warframe.fandom.com/wiki/Void_Relic/ByRewards/SimpleTable";

        // Store all items temporarily in memory before pushing them all to the database
        private Dictionary<int, PrimeItem> _primeItems = new Dictionary<int, PrimeItem>();
        private Dictionary<int, PrimePart> _primeParts = new Dictionary<int, PrimePart>();
        private Dictionary<int, Relic> _relics = new Dictionary<int, Relic>();
        private Dictionary<int, RelicDrop> _relicDrops = new Dictionary<int, RelicDrop>();
        private Dictionary<int, Ingredient> _ingredients = new Dictionary<int, Ingredient>();
        private List<Image> _images = new List<Image>();

        public VoidRelicByRewardsScraper(IPrimeItemScraper primeItemScraper, IIdProvider idProvider) {
            _primeItemScraper = primeItemScraper;
            _idProvider = idProvider;
        }

        public async Task<PrimeData> GetItemData() {
            var table = await GetTableNode();

            foreach (var row in table.ChildNodes) {
                if (row.Name != "tr")
                    continue;

                // Get data from a single row
                var rowData = ParseRow(row);
                // Make sure no unexpected null data is present
                if (!ValidateRowData(rowData))
                    continue;
                // Add it to the items list
                AddRowData(rowData);
            }

            foreach (var item in _primeItems.Values) {
                var data = await _primeItemScraper.GetData(item.WikiUrl);
                foreach (var ingredientData in data.PartsData) {
                    // First check if ingredient are credits
                    if (ingredientData.Name.Equals("Credits", StringComparison.InvariantCultureIgnoreCase)) {
                        item.Credits = ingredientData.Count;
                        continue;
                    }
                    // Then try to find the part and add its count
                    var foundPart = item.PrimeParts.FirstOrDefault(x => x.Name == ingredientData.Name);
                    if (foundPart != null) {
                        foundPart.Count = ingredientData.Count;
                        continue;
                    }
                    // Otherwise we create or update the ingredient count
                    var tmpIngredient = new Ingredient() {
                        Name = ingredientData.Name,
                        Quantity = ingredientData.Count,
                        PrimeItemId = item.Id,
                        PrimeItem = item,
                    };
                    tmpIngredient.Id = _idProvider.GetPersistentKey(tmpIngredient);
                    if (_ingredients.TryGetValue(tmpIngredient.Id, out var ingredient)) {
                        ingredient.Quantity += tmpIngredient.Quantity;
                    } else {
                        ingredient = tmpIngredient;
                        _ingredients.Add(tmpIngredient.Id, tmpIngredient);
                    }

                    // Add ingredient to item if not already present
                    if (item.Ingredients.All(i => i.Id != ingredient.Id)) {
                        item.Ingredients.Add(ingredient);
                    }
                }

                // Add the image
                var memoryStream = new MemoryStream();
                await data.Image.CopyToAsync(memoryStream);
                var image = new Image() {
                    Id = item.Id,
                    Data = memoryStream.ToArray(),
                };
                _images.Add(image);
                data.Image.Dispose();
                item.ImageId = image.Id;
                item.Image = image;
            }

            return new PrimeData() {
                Ingredients = _ingredients.Values,
                PrimeItems = _primeItems.Values,
                PrimeParts = _primeParts.Values,
                Relics = _relics.Values,
                RelicDrops = _relicDrops.Values,
                Images = _images,
            };
        }

        private async Task<HtmlNode> GetTableNode() {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(Url);
            return doc.DocumentNode.SelectSingleNode(@"//table");
        }

        private RowData ParseRow(HtmlNode row) {
            var col = 0;
            var rowData = new RowData();
            foreach (var cell in row.ChildNodes) {
                if (cell.Name != "td")
                    continue;

                switch (col) {
                    case 0:
                        ParseItemCell(cell, rowData);
                        break;
                    case 1:
                        ParsePartCell(cell, rowData);
                        break;
                    case 2:
                        ParseRelicTierCell(cell, rowData);
                        break;
                    case 3:
                        ParseRelicNameCell(cell, rowData);
                        break;
                    case 4:
                        ParseRelicRarityCell(cell, rowData);
                        break;
                    case 5:
                        ParseVaulted(cell, rowData);
                        break;
                }
                col++;
            }
            return rowData;
        }

        private bool ValidateRowData(RowData data) {
            if (data.ItemName == null)
                return false;
            return true;
        }

        private void AddRowData(RowData rowData) {
            // Create or retreive item
            var tmpPrimeItem = new PrimeItem() {
                Name = rowData.ItemName,
                WikiUrl = rowData.ItemUrl,
                PrimeParts = new List<PrimePart>(),
                Ingredients = new List<Ingredient>(),
            };
            tmpPrimeItem.Id = _idProvider.GetPersistentKey(tmpPrimeItem);
            if (!_primeItems.TryGetValue(tmpPrimeItem.Id, out var primeItem)) {
                primeItem = tmpPrimeItem;
                _primeItems.Add(tmpPrimeItem.Id, tmpPrimeItem);
            }

            // Create or retreive relic
            var tmpRelic = new Relic() {
                Tier = rowData.RelicTier.Value,
                Name = rowData.RelicName,
                WikiUrl = rowData.RelicUrl,
                IsVaulted = rowData.IsVaulted.Value,
                RelicDrops = new List<RelicDrop>(),
            };
            tmpRelic.Id = _idProvider.GetPersistentKey(tmpRelic);
            if (!_relics.TryGetValue(tmpRelic.Id, out var relic)) {
                relic = tmpRelic;
                _relics.Add(tmpRelic.Id, tmpRelic);
            }

            // Create or retreive part
            var tmpPrimePart = new PrimePart() {
                PrimeItemId = primeItem.Id,
                PrimeItem = primeItem,
                Name = rowData.PartName,
                Count = 1, // TODO: fetch this data
                RelicDrops = new List<RelicDrop>(),
            };
            tmpPrimePart.Id = _idProvider.GetPersistentKey(tmpPrimePart);
            if (!_primeParts.TryGetValue(tmpPrimePart.Id, out var primePart)) {
                primePart = tmpPrimePart;
                _primeParts.Add(tmpPrimePart.Id, tmpPrimePart);
            }

            // Create or retreive relic drop
            var tmpRelicDrop = new RelicDrop() {
                RelicId = relic.Id,
                Relic = relic,
                PrimePartId = primePart.Id,
                PrimePart = primePart,
                DropChance = rowData.DropChance.Value,
            };
            tmpRelicDrop.Id = _idProvider.GetPersistentKey(tmpRelicDrop);
            if (!_relicDrops.TryGetValue(tmpRelicDrop.Id, out var relicDrop)) {
                relicDrop = tmpRelicDrop;
                _relicDrops.Add(tmpRelicDrop.Id, tmpRelicDrop);
            }

            // Add part to the item if necessary
            if (primeItem.PrimeParts.All(part => part.Id != primePart.Id)) {
                primeItem.PrimeParts.Add(primePart);
            }

            // Add relic drop to the part if necessary
            if (primePart.RelicDrops.All(drop => drop.Id != relicDrop.Id)) {
                primePart.RelicDrops.Add(relicDrop);
            }

            // Add relic drop to the relic if necessary
            if (relic.RelicDrops.All(drop => drop.Id != relicDrop.Id)) {
                relic.RelicDrops.Add(relicDrop);
            }
        }

        private void ParseItemCell(HtmlNode cell, RowData data) {
            var a = cell.ChildNodes.First(c => c.Name == "a");
            var href = a.Attributes.First(attr => attr.Name == "href");
            data.ItemName = a.InnerText;
            data.ItemUrl = href.Value;
        }

        private void ParsePartCell(HtmlNode cell, RowData data) {
            data.PartName = cell.InnerText;
        }

        private void ParseRelicTierCell(HtmlNode cell, RowData data) {
            var text = cell.InnerText;
            if (Enum.TryParse<RelicTier>(text, true, out var tier))
                data.RelicTier = tier;
        }

        private void ParseRelicNameCell(HtmlNode cell, RowData data) {
            var a = cell.SelectSingleNode(".//a");
            var href = a.Attributes.First(attr => attr.Name == "href");
            data.RelicName = a.InnerText;
            data.RelicUrl = href.Value;
        }

        private void ParseRelicRarityCell(HtmlNode cell, RowData data) {
            var text = cell.InnerText;
            if (Enum.TryParse<DropChance>(text, true, out var rarity)) {
                data.DropChance = rarity;
            }
        }

        private void ParseVaulted(HtmlNode cell, RowData data) {
            var text = cell.InnerText.Trim();
            if (text.Equals("yes", StringComparison.InvariantCultureIgnoreCase))
                data.IsVaulted = true;
            else if (text.Equals("no", StringComparison.InvariantCultureIgnoreCase))
                data.IsVaulted = false;
        }
    }
}