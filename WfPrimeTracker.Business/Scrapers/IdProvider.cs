using System;
using WfPrimeTracker.Domain;

namespace WfPrimeTracker.Business.Scrapers {
    public class IdProvider : IIdProvider {
        public int GetPersistentKey<T>(T obj) where T : IPersistentItem {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            switch (obj) {
                case PrimeItem item:
                    // A prime item can be uniquely identified by its name
                    return Math.Abs(GetHashcode(item.Name));
                case PrimePart part:
                    // A prime part can be uniquely identified by its name together with the unique key of the item it's for
                    return Math.Abs((GetPersistentKey(part.PrimeItem) * 397) ^ GetHashcode(part.Name));
                case Relic relic:
                    // A relic can be uniquely identified by its name and tier
                    return Math.Abs((GetHashcode(relic.Name) * 397) ^ (int)relic.Tier);
                case RelicDrop relicDrop:
                    // A relic drop can be uniquely identified by the relic it drops from and its prime part
                    return Math.Abs((GetPersistentKey(relicDrop.Relic) * 397) ^ GetPersistentKey(relicDrop.PrimePart));
                case Ingredient ingredient:
                    // An ingredient can be uniquely identified by its name together with the unique key of the item it's for
                    return Math.Abs((GetPersistentKey(ingredient.PrimeItem) * 397) ^ GetHashcode(ingredient.Name));
            }
            throw new ArgumentOutOfRangeException(nameof(obj), $"Object given can only be of the pre-existing types");
        }

        /// <summary>
        /// We need a consistent and predictable, but still unique value to identify an item. This is a custom GetHashCode implementation to make sure the
        /// computed value is consistent across all systems.
        /// Code taken from https://gist.github.com/gerriten/7542231#file-gethashcode32-net
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int GetHashcode(string value) {
            var chars = value.ToLower().ToCharArray();
            var lastCharInd = chars.Length - 1;
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