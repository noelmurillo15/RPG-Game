/*
 * ListExtensions - Custom functionality for Lists
 * Created by : Allan N. Murillo
 * Last Edited : 8/13/2021
 */

using UnityEngine;
using System.Collections.Generic;

namespace ANM.Framework.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = Random.Range(i, count);
                (ts[i], ts[r]) = (ts[r], ts[i]);
            }
        }
    }
}