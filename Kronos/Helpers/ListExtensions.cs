﻿using System;
using System.Collections.Generic;

using Kronos.Helpers;

namespace Kronos
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            int items = list.Count;

            while (items > 1)
            {
                int position = Dice.Next(0, items) % items;

                items--;

                T item = list[position];
                list[position] = list[items];
                list[items] = item;
            }
        }

        public static void Rotate<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            T first = list[0];

            list.RemoveAt(0);
            list.Add(first);
        }
    }
}
