using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BattleShip.Interface;

namespace Kronos.Worlds
{
  public class Observer
  {
    public World World { get; set; }

    public Observer(God god) { }

    public void Show()
    {
      Console.Clear();

      string        _mark = "";
      StringBuilder viewArea = new StringBuilder();
      World          zone;
      State         state;
      Coordinate    lastShot;

      for (int _y = World.Boundaries.North; _y >= 1; _y--)
      {
        viewArea.Append("    ");

        if (_y == World.Boundaries.North)
          for (int _x = 1; _x <= World.Boundaries.East; _x++)
            viewArea.Append(_x).Append(" ");

        viewArea.AppendLine();
        viewArea.Append(_y.ToString().PadLeft(2)).Append(": ");

        for (int _x = 1; _x <= World.Boundaries.East; _x++)
        {
          zone = null;

          if (zone == null)
            state = State.Hidden;
          else
            state = zone.State;

          switch (state)
          {
            case State.Hidden:
              _mark = ". ";
              break;
            case State.Visited:
              _mark = "o ";
              break;
            case State.Destroyed:
              _mark = "X ";
              break;
          }

          viewArea.Append(_mark);
        }

        viewArea.AppendLine();
        viewArea.AppendLine();
      }

      lastShot = World.RandomCoordinate;

      viewArea.Append("Last shot: ").Append(lastShot.X).Append(",").Append(lastShot.Y).AppendLine();
      viewArea.AppendLine(string.Concat("Shots fired: ", World.Impacts));

      Console.Write(viewArea.ToString());
      Console.ReadKey();
    }
  }
}
