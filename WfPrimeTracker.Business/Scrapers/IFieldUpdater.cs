using System;
using Remotion.Linq.Parsing;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Scrapers {
    public interface IFieldUpdater {
        T Update<T>(T source, T destination);
    }

    class FieldUpdater : IFieldUpdater {
        /// <inheritdoc />
        public T Update<T>(T source, T destination) {
            switch (source) {
                case Image image:
                    UpdateImage(image, destination as Image);
                    break;
                case IngredientsGroup ingredientsGroup:
                    UpdateIngredientsGroup(ingredientsGroup, destination as IngredientsGroup);
                    break;
                case PrimeItem primeItem:
                    UpdatePrimeItem(primeItem, destination as PrimeItem);
                    break;
                case PrimePart primePart:
                    UpdatePrimePart(primePart, destination as PrimePart);
                    break;
                case PrimePartIngredient partIngredient:
                    UpdatePrimePartIngredient(partIngredient, destination as PrimePartIngredient);
                    break;
                case Relic relic:
                    UpdateRelic(relic, destination as Relic);
                    break;
                case RelicDrop relicDrop:
                    UpdateRelicDrop(relicDrop, destination as RelicDrop);
                    break;
                case Resource resource:
                    UpdateResource(resource, destination as Resource);
                    break;
                case ResourceIngredient resourceIngredient:
                    UpdateResourceIngredient(resourceIngredient, destination as ResourceIngredient);
                    break;
                default:
                    throw new ArgumentException("Type can only be an entity type", nameof(source));
            }
            return destination;
        }

        private void UpdateImage(Image source, Image dest) {
            dest.Data = source.Data;
            dest.PrimeItem = source.PrimeItem;
            dest.PrimePart = source.PrimePart;
            dest.Relic = source.Relic;
            dest.Resource = source.Resource;
        }

        private void UpdateIngredientsGroup(IngredientsGroup source, IngredientsGroup dest) {
            dest.Name = source.Name;
            dest.PrimeItemId = source.PrimeItemId;
            dest.PrimeItem = source.PrimeItem;
        }

        private void UpdatePrimeItem(PrimeItem source, PrimeItem dest) {
            dest.Name = source.Name;
            dest.WikiUrl = source.WikiUrl;
            dest.ImageId = source.ImageId;
            dest.Image = source.Image;
        }

        private void UpdatePrimePart(PrimePart source, PrimePart dest) {
            dest.Name = source.Name;
            dest.ImageId = source.ImageId;
            dest.Image = source.Image;
        }

        private void UpdatePrimePartIngredient(PrimePartIngredient source, PrimePartIngredient dest) {
            dest.PrimeItemId = source.PrimeItemId;
            dest.PrimeItem = source.PrimeItem;
            dest.PrimePartId = source.PrimePartId;
            dest.PrimePart = source.PrimePart;
            dest.Count = source.Count;
        }

        private void UpdateRelic(Relic source, Relic dest) {
            dest.Name = source.Name;
            dest.Tier = source.Tier;
            dest.IsVaulted = source.IsVaulted;
            dest.WikiUrl = source.WikiUrl;
            dest.ImageId = source.ImageId;
            dest.Image = source.Image;
        }

        private void UpdateRelicDrop(RelicDrop source, RelicDrop dest) {
            dest.RelicId = source.RelicId;
            dest.Relic = source.Relic;
            dest.PrimePartIngredientId = source.PrimePartIngredientId;
            dest.PrimePartIngredient = source.PrimePartIngredient;
            dest.DropChance = source.DropChance;
        }

        private void UpdateResource(Resource source, Resource dest) {
            dest.Name = source.Name;
            dest.ImageId = source.ImageId;
            dest.Image = source.Image;
        }

        private void UpdateResourceIngredient(ResourceIngredient source, ResourceIngredient dest) {
            dest.ResourceId = source.ResourceId;
            dest.Resource = source.Resource;
            dest.IngredientsGroupId = source.IngredientsGroupId;
            dest.IngredientsGroup = source.IngredientsGroup;
            dest.Count = source.Count;
        }
    }
}