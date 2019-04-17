using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Data;
using WfPrimeTracker.Domain;
using WfPrimeTracker.Dtos;

namespace WfPrimeTracker.Business.Scrapers {
    internal class VoidRelicByRewardsScraper : IVoidRelicByRewardScraper {
        private readonly IPrimeItemScraper _primeItemScraper;
        private readonly IIdProvider _idProvider;
        private readonly InMemoryPrimeContext _memContext;
        private const string Url = "https://warframe.fandom.com/wiki/Void_Relic/ByRewards/SimpleTable";

        public VoidRelicByRewardsScraper(IPrimeItemScraper primeItemScraper, IIdProvider idProvider, InMemoryPrimeContext memContext) {
            _primeItemScraper = primeItemScraper;
            _idProvider = idProvider;
            _memContext = memContext;
        }

        public async Task FetchAllPrimeItemData() {
            _memContext.Clear();
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
            _memContext.SaveChanges();

            foreach (var item in _memContext.PrimeItems.ToArray()) {
                var data = await _primeItemScraper.GetData(item.WikiUrl);
                await AddItemData(item, data);
            }
            _memContext.SaveChanges();
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
            if (data.ItemName == null || data.ItemName == "Forma")
                return false;
            if (!data.DropChance.HasValue)
                return false;
            if (!data.IsVaulted.HasValue)
                return false;
            if (!data.RelicTier.HasValue)
                return false;
            return true;
        }

        private void AddRowData(RowData rowData) {
            // Create or retreive item
            var primeItem = AddOrUpdatePeristentItem(new PrimeItem() {
                Name = rowData.ItemName,
                WikiUrl = rowData.ItemUrl,
            });

            // Create or retreive part
            var primePart = AddOrUpdatePeristentItem(new PrimePart() {
                Name = rowData.PartName,
            });

            // Create or retreive relic
            var relic = AddOrUpdatePeristentItem(new Relic() {
                Tier = rowData.RelicTier.Value,
                Name = rowData.RelicName,
                WikiUrl = rowData.RelicUrl,
                IsVaulted = rowData.IsVaulted.Value,
            });

            // Link prime item with prime part
            var partIngredient = AddOrUpdatePeristentItem(new PrimePartIngredient() {
                    PrimeItemId = primeItem.Id,
                    PrimeItem = primeItem,
                    PrimePartId = primePart.Id,
                    PrimePart = primePart,
                    Count = 1 // Placeholder value
            });

            // Link prime part with relic
            var relicDrop = AddOrUpdateItem(
                new RelicDrop() {
                    RelicId = relic.Id,
                    Relic = relic,
                    PrimePartIngredientId = partIngredient.Id,
                    PrimePartIngredient = partIngredient,
                    DropChance = rowData.DropChance.Value,
                },
                r => new object[] { r.RelicId, r.PrimePartIngredientId }
            );
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

        private async Task AddItemData(PrimeItem primeItem, PrimeItemData itemData) {
            // Create a base group for Ingredients
            var baseGroup = AddOrUpdatePeristentItem(new IngredientsGroup() {
                Name = null,
                PrimeItemId = primeItem.Id,
                PrimeItem = primeItem,
            });

            foreach (var ingredientData in itemData.PartsData) {
                // Try to find the part and set its count
                var foundPart = primeItem.PrimePartIngredients.FirstOrDefault(x => x.PrimePart.Name == ingredientData.Name);
                if (foundPart != null) {
                    foundPart.Count = ingredientData.Count;
                    continue;
                }
                // Otherwise we create or update a resource
                var resource = AddOrUpdatePeristentItem(new Resource() {
                    Name = ingredientData.Name,
                });

                // And link it to the item
                var resourceIngredient = AddOrUpdateItem(
                    new ResourceIngredient() {
                        IngredientsGroupId = baseGroup.Id,
                        IngredientsGroup = baseGroup,
                        ResourceId = resource.Id,
                        Resource = resource,
                        Count = ingredientData.Count,
                    },
                    x => new object[] { x.IngredientsGroupId, x.ResourceId }
                );
            }

            // Add the image
            var memoryStream = new MemoryStream();
            await itemData.Image.CopyToAsync(memoryStream);
            var image = AddOrUpdatePeristentItem(new Image() {
                Id = primeItem.Id,
                Data = memoryStream.ToArray(),
            });
            itemData.Image.Dispose();

            // Add a link to the item
            primeItem.ImageId = image.Id;
            primeItem.Image = image;
        }

        private T AddOrUpdateItem<T>(T item, Func<T, object[]> keysFunc) where T : class {
            var foundItem = _memContext.Set<T>().Find(keysFunc(item));
            if (foundItem != null) {
                return foundItem;
            }
            _memContext.Add(item);
            return item;
        }

        private T AddOrUpdatePeristentItem<T>(T item) where T : class, IPersistentItem {
            var localItem = _idProvider.InsertPersistentKey(item);
            var foundItem = _memContext.Set<T>().Find(localItem.Id);
            if (foundItem != null) {
                return foundItem;
            }
            _memContext.Add(localItem);
            return localItem;
        }
    }
}