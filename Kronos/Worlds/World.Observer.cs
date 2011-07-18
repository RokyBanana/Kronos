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

      StringBuilder viewArea = new StringBuilder();

      for (int longitude = map.Boundaries.North; longitude >= map.Boundaries.South; longitude--)
      {
        viewArea.Append(longitude.ToString(CultureInfo.InvariantCulture).PadLeft(2)).Append(": ");

        for (int latitude = map.Boundaries.West; latitude <= map.Boundaries.East; latitude++)
          viewArea.AppendFormat("{0} ", map.GetMarker(latitude, longitude));

        if (longitude == map.Boundaries.South)
        {
          viewArea.AppendLine().Append("    ");

          for (int _x = map.Boundaries.West; _x <= map.Boundaries.East; _x++)
            viewArea.Append(_x).Append(" ");
        }

        viewArea.AppendLine();
        viewArea.AppendLine();
      }

      viewArea.Append("Shots fired: ").Append(map.Impacts).AppendLine();

      if (map.Impacts > 0)
        viewArea.Append("Target coordinates: ").Append(map.Impact.X).Append(",").Append(map.Impact.Y).AppendLine();

      Console.Write(viewArea.ToString());
      Console.ReadKey();
    }
  }
}
