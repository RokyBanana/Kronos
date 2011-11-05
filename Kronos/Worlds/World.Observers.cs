using System;
using System.Globalization;
using System.Text;

using Kronos.Gods;
using Kronos.Worlds.Maps;

namespace Kronos
{
  public class Observer
  {
    public bool ShowBattlefield { get; set; }
    public bool ShowEachTurn { get; set; }
    public bool ShowNeighbors { get; set; }
    public Map Map { get; set; }

    private static string _consoleOutput;

    public void Show()
    {
      RenderMap();

      char[] buffer = new char[_consoleOutput.Length];
      buffer = _consoleOutput.ToCharArray(0, _consoleOutput.Length);

      Console.Clear();
      Console.Write(buffer);

      if(ShowEachTurn)
        Console.ReadKey();
    }

    private void RenderMap()
    {
      StringBuilder viewArea = new StringBuilder();

      if (ShowBattlefield)
      {
        for (int longitude = Map.Boundaries.North; longitude >= Map.Boundaries.South; longitude--)
        {
          viewArea.Append(longitude.ToString(CultureInfo.InvariantCulture).PadLeft(2)).Append(": ");

          for (int latitude = Map.Boundaries.West; latitude <= Map.Boundaries.East; latitude++)
            if (ShowNeighbors)
              viewArea.Append(Map.Loneliness(latitude, longitude).ToString().PadLeft(3));
            else
              viewArea.Append(Map.GetMarker(latitude, longitude).PadLeft(3));

          if (longitude == Map.Boundaries.South)
          {
            viewArea.AppendLine().Append("    ");

            for (int xAxis = Map.Boundaries.West; xAxis <= Map.Boundaries.East; xAxis++)
              viewArea.Append("-".PadLeft(3));

            viewArea.AppendLine().Append("    ");

            for (int xAxis = Map.Boundaries.West; xAxis <= Map.Boundaries.East; xAxis++)
              viewArea.Append(xAxis.ToString().PadLeft(3));
          }

          viewArea.AppendLine();
          viewArea.AppendLine();
        }
      }

      viewArea.Append("Shots fired: ").Append(God.Smites).AppendLine();

      _consoleOutput = viewArea.ToString();
    }
  }
}
