using System;
using System.Collections.Generic;

namespace Stratego.Core.Utility
{
    public static class ExtensionMethods
    {
        public static int IndexOf<T>(this IEnumerable<T> list, T value)
        {
            var i = 0;

            foreach (var e in list)
                if (EqualityComparer<T>.Default.Equals(e, value))
                    return i;
                else
                    i++;

            return -1;
        }

        public static T NextItem<T>(this Random random, IReadOnlyList<T> items)
        {
            return items[random.Next(items.Count)];
        }

        public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            var index = 0;
            foreach (var item in source)
            {
                if (predicate(item))
                    return index;
                index++;
            }

            return -1;
        }
    }
}