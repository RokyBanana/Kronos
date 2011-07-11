using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BattleShip.Interface;

using Kronos.Worlds.Maps;

namespace Kronos.Worlds
{
  public class Observer
  {
    private World _world;

    public Observer(World world)
    {
      _world = world;
    }

    public void Show()
    {
      Console.Clear();

      string mark = "";
      StringBuilder viewArea = new StringBuilder();

      for (int _y = _world.Boundaries.North; _y >= _world.Boundaries.South; _y--)
      {
        viewArea.Append("    ");

        if (_y == _world.Boundaries.North)
          for (int _x = _world.Boundaries.West; _x <= _world.Boundaries.East; _x++)
            viewArea.Append(_x).Append(" ");

        viewArea.AppendLine();
        viewArea.Append(_y.ToString().PadLeft(2)).Append(": ");

        for (int _x = _world.Boundaries.West; _x <= _world.Boundaries.East; _x++)
        {
          switch (_world.Map.StatusAt(_x, _y))
          {
            case Status.Hidden:
              mark = ". ";
              break;
            case Status.Explored:
              mark = "o ";
              break;
            case Status.Defiled:
              mark = "X ";
              break;
          }

          viewArea.Append(mark);
        }

        viewArea.AppendLine();
        viewArea.AppendLine();
      }

      viewArea.AppendLine(string.Concat("Shots fired: ", _world.Impacts.Count));

      Console.Write(viewArea.ToString());
      Console.ReadKey();
    }
  }
}
