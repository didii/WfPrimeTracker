using System;
using System.Collections.Generic;
using System.Linq;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Scrapers {
    public class IdProvider : IIdProvider {
        public T InsertPersistentKey<T>(T obj) where T : IPersistentItem {
            obj.Id = GetPersistentKey(obj);
            return obj;
        }

        private int GetPersistentKey<T>(T obj) where T : IPersistentItem {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            int hash;
            switch (obj) {
                case PrimeItem item:
                    // A prime item can be uniquely identified by its name
                    hash = GetHashCode(item.Name);
                    break;
                case PrimePartIngredient partIngredient:
                    // A part ingredient is uniquely identified by its item and part
                    hash = (GetPersistentKey(partIngredient.PrimeItem) * 397) ^ GetPersistentKey(partIngredient.PrimePart);
                    break;
                case PrimePart part:
                    // A prime part can be uniquely identified by its name
                    hash = GetHashCode(part.Name);
                    break;
                case Relic relic:
                    // A relic can be uniquely identified by its name and tier
                    hash = (GetHashCode(relic.Name) * 397) ^ (int)relic.Tier;
                    break;
                case IngredientsGroup ingredientsGroup:
                    // An ingredients group can be uniquely identified by its name and its parent
                    hash = (GetPersistentKey(ingredientsGroup.PrimeItem) * 397) ^ GetHashCode(ingredientsGroup.Name);
                    break;
                case Resource resource:
                    // An ingredient can be uniquely identified by its name together with the unique key of the item it's for
                    hash = GetHashCode(resource.Name);
                    break;
                case Image image:
                    // An image can be uniquely defined by its data
                    hash = GetHashCode(image.Data.Select(b => (char)b).ToArray());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), $"Object given can only be of the pre-existing types");
            }
            return Math.Abs(hash);
        }

        private int GetHashCode(string value) {
            if (value == null) return 0;
            return GetHashCode(value.ToLower().ToCharArray());
        }

        /// <summary>
        /// We need a consistent and predictable, but still unique value to identify an item. This is a custom GetHashCode implementation to make sure the
        /// computed value is consistent across all systems.
        /// Code taken from https://gist.github.com/gerriten/7542231#file-gethashcode32-net
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        private int GetHashCode(IReadOnlyList<char> chars) {
            if (chars == null) throw new ArgumentNullException(nameof(chars));
            var lastCharInd = chars.Count - 1;
            var num1 = 0x15051505;
            var num2 = num1;
            var ind = 0;
            while (ind <= lastCharInd) {
                var ch = chars[ind];
                var nextCh = ++ind > lastCharInd ? '\0' : chars[ind];
                num1 = ((num1 << 5) + num1 + (num1 >> 0x1b)) ^ (nextCh << 16 | ch);
                if (++ind > lastCharInd) break;
                ch = chars[ind];
                nextCh = ++ind > lastCharInd ? '\0' : chars[ind++];
                num2 = ((num2 << 5) + num2 + (num2 >> 0x1b)) ^ (nextCh << 16 | ch);
            }
            return num1 + num2 * 0x5d588b65;
        }
    }
}