using System;
using System.Globalization;
using System.Linq;
using System.Text;

using Kronos.Worlds.Maps;

namespace Kronos.Worlds
{
  public static class Observer
  {
    public static void Show(Map map)
    {
      if (map == null)
        throw new ArgumentNullException("map");

      Console.Clear();

      string mark = "";
      StringBuilder viewArea = new StringBuilder();

      for (int _y = map.Boundaries.North; _y >= map.Boundaries.South; _y--)
      {
        viewArea.Append(_y.ToString(CultureInfo.InvariantCulture).PadLeft(2)).Append(": ");

        for (int _x = map.Boundaries.West; _x <= map.Boundaries.East; _x++)
        {
          switch (map.StatusAt(_x, _y))
          {
            case Status.Hidden:
              mark = ". ";
              break;
            case Status.Explored:
              mark = "o ";
              break;
            case Status.Damaged:
              mark = "D ";
              break;
            case Status.Defiled:
              mark = "X ";
              break;
            case Status.Ignored:
              mark = "  ";
              break;
          }

          viewArea.Append(mark);
        }

        if (_y == map.Boundaries.South)
        {
          viewArea.AppendLine().Append("    ");

          for (int _x = map.Boundaries.West; _x <= map.Boundaries.East; _x++)
            viewArea.Append(_x).Append(" ");
        }

        viewArea.AppendLine();
        viewArea.AppendLine();
      }

      viewArea.AppendLine(string.Concat("Shots fired: ", map.Impacts));

      if (map.Impacts > 0)
        viewArea.AppendLine(string.Concat("Target coordinate: ", map.Impact.X, ",", map.Impact.Y));

      Console.Write(viewArea.ToString());
      Console.ReadKey();
    }
  }
}
