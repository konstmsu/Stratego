using System.Collections.ObjectModel;
using System.Linq;

namespace Stratego
{
    public static class ExtensionMethods
    {
        public static ReadOnlyCollection<T> ToReadOnly<T>(this T[] values)
        {
            return values.ToList().AsReadOnly();
        }
    }
}