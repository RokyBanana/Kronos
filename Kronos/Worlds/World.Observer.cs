using System;
using System.Text;

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
            case Status.Damaged:
              mark = "! ";
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

      if (_world.Impacts.Count > 0)
        viewArea.AppendLine(string.Concat("Impact coordinate: ", _world.Impacts[_world.Impacts.Count - 1].X, ",", _world.Impacts[_world.Impacts.Count - 1].Y));

      Console.Write(viewArea.ToString());
      Console.ReadKey();
    }
  }
}
