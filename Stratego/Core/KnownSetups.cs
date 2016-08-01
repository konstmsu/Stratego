using System.Collections.Generic;

namespace Stratego.Core
{
    // http://www.ultrastratego.com/ru/setups.php
    public static class KnownSetups
    {
        public static readonly IReadOnlyList<IReadOnlyList<int>> VincentDeboer = new[]
        {
            new[] { 6, 2, 2, 5, 2, 6, 3, 10, 2, 6 },
            new[] { 5, 4, 11, 1, 9, 2, 7, 7, 8, 2 },
            new[] { 4, 11, 4, 7, 8, 5, 11, 5, 6, 4 },
            new[] { 2, 3, 11, 2, 3, 11, 0, 11, 3, 3 }
        };
    }
}