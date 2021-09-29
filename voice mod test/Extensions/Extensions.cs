using System;
using System.Collections.Generic;
using System.Linq;

namespace SlapCityVoiceMod.Extensions
{
    static class Extensions
    {
        private static Random _random;
        private static Random Random
        {
            get
            {
                _random ??= new Random(DateTime.Now.Millisecond);
                return _random;
            }
            set => _random = value;
        }

        public static T GetRandomItem<T>(this IEnumerable<T> items, Func<T, float> weightKey)
        {
            var totalWeight = items.Sum(x => weightKey(x));
            var randomWeightedIndex = (float)Random.NextDouble() * totalWeight;
            var itemWeightedIndex = 0f;
            foreach (var item in items)
            {
                itemWeightedIndex += weightKey(item);
                if (randomWeightedIndex < itemWeightedIndex)
                    return item;
            }
            return default;
        }

    }
}
