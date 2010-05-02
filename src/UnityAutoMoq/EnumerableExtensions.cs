using System;
using System.Collections.Generic;

namespace UnityAutoMoq
{
    internal static class EnumerableExtensions
    {
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T obj in enumerable)
            {
                action(obj);
            }
        }
    }

}