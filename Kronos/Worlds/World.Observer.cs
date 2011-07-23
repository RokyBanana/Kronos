using System;
using System.Globalization;
using System.Text;

using Kronos.Gods;
using Kronos.Worlds.Maps;

namespace Kronos
{
  public static class Observer
  {
    private static Map _map;
    private static string _consoleOutput;

    public static void Show(Map map)
    {
      if (map == null)
        throw new ArgumentNullException("map");

      _map = map;

      RenderMap();

      char[] buffer = new char[_consoleOutput.Length];
      buffer = _consoleOutput.ToCharArray(0, _consoleOutput.Length);

      Console.Clear();
      Console.Write(buffer);

      //System.Threading.Thread.Sleep(10);
      //Console.ReadKey();
    }

    private static void RenderMap()
    {
      StringBuilder viewArea = new StringBuilder();

      for (int longitude = _map.Boundaries.North; longitude >= _map.Boundaries.South; longitude--)
      {
        viewArea.Append(longitude.ToString(CultureInfo.InvariantCulture).PadLeft(2)).Append(": ");

        for (int latitude = _map.Boundaries.West; latitude <= _map.Boundaries.East; latitude++)
          viewArea.AppendFormat("{0} ", _map.GetMarker(latitude, longitude));

        if (longitude == _map.Boundaries.South)
        {
          viewArea.AppendLine().Append("    ");

          for (int _x = _map.Boundaries.West; _x <= _map.Boundaries.East; _x++)
            viewArea.Append(_x).Append(" ");
        }

        viewArea.AppendLine();
        viewArea.AppendLine();
      }

      viewArea.Append("Shots fired: ").Append(God.Smites).AppendLine();
      viewArea.Append("Target coordinates: ").Append(_map.Impact.X).Append(",").Append(_map.Impact.Y).AppendLine();

      _consoleOutput = viewArea.ToString();
    }
  }
}
