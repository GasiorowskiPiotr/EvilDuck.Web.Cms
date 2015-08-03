using System;
using System.Collections.Generic;
using System.Linq;

namespace EvilDuck.Framework.Core.Utils
{
    public static class FunctionalExtensions
    {
        public static void Do<T>(this IEnumerable<T> elems, Action<T> action)
        {
            foreach (var elem in elems)
            {
                action(elem);
            }
        }

        public static IEnumerable<T> InitEnumerable<T>(this T e)
        {
            return new List<T> { e };
        }

        public static IEnumerable<T> Compact<T>(this IEnumerable<T> elems) where T : class
        {
            return elems.Where(e => e != null);
        }

        public static IEnumerable<string> Compact(this IEnumerable<string> strings)
        {
            return strings.Where(s => !String.IsNullOrEmpty(s));
        }

    }
}