using System;
using System.Collections.Generic;
using System.Linq;

namespace Kronos
{
  public static class Extensions
  {
    public static void Shuffle<T>(this IList<T> list)
    {
      int items = list.Count;
      Random rnd = new Random();

      while (items > 1)
      {
        int position = rnd.Next(0, items) % items;

        items--;

        T item = list[position];
        list[position] = list[items];
        list[items] = item;
      }
    }

    public static void Rotate<T>(this IList<T> list)
    {
      T first = list[0];

      list.RemoveAt(0);
      list.Add(first);
    }
  }
}
