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
    }
}