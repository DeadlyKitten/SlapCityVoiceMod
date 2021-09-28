using System;
using System.Collections.Generic;
using System.Linq;

namespace SlapCityVoiceMod.Extensions
{
    static class Extensions
    {
        public static T GetRandomItem<T>(this IEnumerable<T> itemsEnumerable, Func<T, float> weightKey)
        {
            var items = itemsEnumerable.ToList();

            var totalWeight = items.Sum(x => weightKey(x));
            var randomWeightedIndex = (float) new Random(DateTime.Now.Millisecond).NextDouble() * totalWeight;
            var itemWeightedIndex = 0f;
            foreach (var item in items)
            {
                itemWeightedIndex += weightKey(item);
                if (randomWeightedIndex < itemWeightedIndex)
                    return item;
            }
            throw new ArgumentException("Collection count and weights must be greater than 0");
        }

    }
}
