using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WfPrimeTracker.Data;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Scrapers {
    class DataPersister : IDataPersister {
        private readonly PrimeContext _context;
        private readonly IIdProvider _idProvider;
        private readonly IFieldUpdater _updater;

        public DataPersister(PrimeContext context, IIdProvider idProvider, IFieldUpdater updater) {
            _context = context;
            _idProvider = idProvider;
            _updater = updater;
        }

        /// <inheritdoc />
        public async Task<PrimeItem> AddOrUpdateRowData(RowData rowData) {
            // Create or retreive item
            var primeItem = await AddOrUpdatePeristentItem(
                new PrimeItem() {
                    Name = rowData.ItemName,
                    WikiUrl = rowData.ItemUrl,
                },
                (dest, source) => {
                    if (dest.Name != source.Name) dest.Name = source.Name;
                    if (dest.WikiUrl != source.WikiUrl) dest.WikiUrl = source.WikiUrl;
                }
            );

            // Create or retreive part
            var primePart = await AddOrUpdatePeristentItem(
                new PrimePart() {
                    Name = rowData.PartName,
                },
                (dest, source) => {
                    if (dest.Name != source.Name) dest.Name = source.Name;
                });

            // Create or retreive relic
            var relic = await AddOrUpdatePeristentItem(
                new Relic() {
                    Tier = rowData.RelicTier.Value,
                    Name = rowData.RelicName,
                    WikiUrl = rowData.RelicUrl,
                    IsVaulted = rowData.IsVaulted.Value,
                },
                (dest, source) => {
                    if (dest.Tier != source.Tier) dest.Tier = source.Tier;
                    if (dest.Name != source.Name) dest.Name = source.Name;
                    if (dest.WikiUrl != source.WikiUrl) dest.WikiUrl = source.WikiUrl;
                    if (dest.IsVaulted != source.IsVaulted) dest.IsVaulted = source.IsVaulted;
                });

            // Link prime item with prime part
            var partIngredient = await AddOrUpdatePeristentItem(
                new PrimePartIngredient() {
                    PrimeItemId = primeItem.Id,
                    PrimeItem = primeItem,
                    PrimePartId = primePart.Id,
                    PrimePart = primePart,
                    Count = 1 // Placeholder value
                },
                (dest, source) => {
                    if (dest.PrimeItemId != source.PrimeItemId) {
                        dest.PrimeItemId = source.PrimeItemId;
                        dest.PrimeItem = source.PrimeItem;
                    }
                    if (dest.PrimePartId != source.PrimePartId) {
                        dest.PrimePartId = source.PrimePartId;
                        dest.PrimePart = source.PrimePart;
                    }
                    if (dest.Count != source.Count) dest.Count = source.Count;
                });

            // Link prime part with relic
            var relicDrop = await AddOrUpdateItem(
                new RelicDrop() {
                    RelicId = relic.Id,
                    Relic = relic,
                    PrimePartIngredientId = partIngredient.Id,
                    PrimePartIngredient = partIngredient,
                    DropChance = rowData.DropChance.Value,
                },
                r => new object[] { r.RelicId, r.PrimePartIngredientId },
                (dest, source) => {
                    if (dest.RelicId != source.RelicId) {
                        dest.RelicId = source.RelicId;
                        dest.Relic = source.Relic;
                    }
                    if (dest.PrimePartIngredientId != source.PrimePartIngredientId) {
                        dest.PrimePartIngredientId = source.PrimePartIngredientId;
                        dest.PrimePartIngredient = source.PrimePartIngredient;
                    }
                    if (dest.DropChance != source.DropChance) dest.DropChance = source.DropChance;
                }
            );

            return primeItem;
        }

        /// <inheritdoc />
        public async Task<PrimeItem> AddOrUpdatePrimeItem(PrimeItem primeItem, PrimeItemData itemData) {
            // Keep track of prime part ingredients that have been found
            var foundPrimePartIngredients = new Dictionary<string, PrimePartIngredient>();

            // Create a base group for Ingredients
            foreach (var ingredientGroupData in itemData.PartsData) {
                // Create the ingredients group
                var groupName = ingredientGroupData.Key;
                if (string.IsNullOrEmpty(groupName)) {
                    groupName = null;
                }
                var group = await AddOrUpdatePeristentItem(
                    new IngredientsGroup() {
                        Name = groupName,
                        PrimeItemId = primeItem.Id,
                        PrimeItem = primeItem,
                    },
                    (dest, source) => {
                        if (dest.Name != source.Name) dest.Name = source.Name;
                        if (dest.PrimeItemId != source.PrimeItemId) {
                            dest.PrimeItemId = source.PrimeItemId;
                            dest.PrimeItem = source.PrimeItem;
                        }
                    });

                // Add all ingredients to it
                foreach (var ingredientData in ingredientGroupData.Value) {
                    // Try to find the part and set its count to 0 if new
                    if (!foundPrimePartIngredients.TryGetValue(ingredientData.Name, out var foundPart)) {
                        foundPart = primeItem.PrimePartIngredients.FirstOrDefault(
                            x => x.PrimePart.Name.StartsWith(ingredientData.Name));
                        if (foundPart != null) {
                            foundPart.Count = 0;
                            foundPrimePartIngredients.Add(ingredientData.Name, foundPart);
                        }
                    }
                    // If part was found, update count and image
                    if (foundPart != null) {
                        foundPart.Count += ingredientData.Count;
                        // Add image
                        var partImage = await AddOrUpdatePeristentItem(
                            new Image() {
                                Data = await StreamToByteArray(ingredientData.Image),
                            },
                            (dest, source) => dest.Data = source.Data);

                        foundPart.PrimePart.ImageId = partImage.Id;
                        foundPart.PrimePart.Image = partImage;
                        continue;
                    }
                    // Otherwise we create or update a resource
                    var resourceImage = await AddOrUpdatePeristentItem(
                        new Image() {
                            Data = await StreamToByteArray(ingredientData.Image),
                        },
                        (dest, source) => dest.Data = source.Data);

                    var resource = await AddOrUpdatePeristentItem(
                        new Resource() {
                            Name = ingredientData.Name,
                            ImageId = resourceImage.Id,
                            Image = resourceImage,
                        },
                        (dest, source) => {
                            if (dest.Name != source.Name) dest.Name = source.Name;
                            if (dest.ImageId != source.ImageId) {
                                dest.ImageId = source.ImageId;
                                dest.Image = source.Image;
                            }
                        });

                    var resourceIngredient = await AddOrUpdateItem(
                        new ResourceIngredient() {
                            ResourceId = resource.Id,
                            Resource = resource,
                            IngredientsGroupId = group.Id,
                            IngredientsGroup = group,
                            Count = ingredientData.Count,
                        },
                        i => new object[] { i.IngredientsGroupId, i.ResourceId },
                        (dest, source) => {
                            if (dest.ResourceId != source.ResourceId) {
                                dest.ResourceId = source.ResourceId;
                                dest.Resource = source.Resource;
                            }
                            if (dest.IngredientsGroupId != source.IngredientsGroupId) {
                                dest.IngredientsGroupId = source.IngredientsGroupId;
                                dest.IngredientsGroup = source.IngredientsGroup;
                            }
                            if (dest.Count != source.Count) dest.Count = source.Count;
                        }
                    );
                }
            }

            // Add the image
            var image = await AddOrUpdatePeristentItem(
                new Image() {
                    Data = await StreamToByteArray(itemData.Image),
                },
                (dest, source) => dest.Data = source.Data
            );

            primeItem.ImageId = image.Id;
            primeItem.Image = image;

            return primeItem;
        }

        /// <inheritdoc />
        public async Task<Image> AddOrUpdateImage(Image image) {
            return await AddOrUpdatePeristentItem(image, (dest, source) => dest.Data = source.Data);
        }

        private async Task<T> AddOrUpdateItem<T>(T item, Func<T, object[]> keysFunc, Action<T, T> updater)
            where T : class {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (keysFunc == null) throw new ArgumentNullException(nameof(keysFunc));
            if (updater == null) throw new ArgumentNullException(nameof(updater));

            // Try to find the item
            var foundItem = await _context.Set<T>().FindAsync(keysFunc(item));
            if (foundItem != null) {
                var entry = _context.Entry(foundItem);
                if (entry.State == EntityState.Added)
                    return foundItem;
                entry.State = EntityState.Modified;
                updater(foundItem, item);
                return foundItem;
            }
            _context.Add(item);
            return item;
        }

        private async Task<T> AddOrUpdatePeristentItem<T>(T item, Action<T, T> updater)
            where T : class, IPersistentItem {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (updater == null) throw new ArgumentNullException(nameof(updater));

            var localItem = _idProvider.InsertPersistentKey(item);
            var foundItem = await _context.Set<T>().FindAsync(localItem.Id);
            if (foundItem != null) {
                var entry = _context.Entry(foundItem);
                if (entry.State == EntityState.Added)
                    return foundItem;
                entry.State = EntityState.Modified;
                updater(foundItem, localItem);
                return foundItem;
            }
            _context.Add(localItem);
            return localItem;
        }

        private async Task<byte[]> StreamToByteArray(Stream stream) {
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var result = memoryStream.ToArray();
            stream.Dispose();
            return result;
        }
    }
}