using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Extensions
{
    public static class LinquExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection)
        {
            Random random = new Random();
            return collection.OrderBy(x => random.Next()).ToArray();
        }
    }
}
