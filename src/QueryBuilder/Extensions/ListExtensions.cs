using System.Collections.Generic;
using System.Linq;

namespace QueryBuilder.Extensions
{
    public static class ListExtensions
    {
        public static bool HasChild<T>(this IEnumerable<T> collection)
        {
            return collection?.Any() ?? false;
        }
    }
}