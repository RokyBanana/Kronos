using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BattleShip.Interface;

namespace Kronos.Worlds.Maps
{
  public class Position
  {
    public Coordinate Coordinate { get; set; }
    public Status Status { get; set; }

    public Position(Coordinate coordinate, Status status)
    {
      Coordinate = coordinate;
      Status = status;
    }
  }

  public enum Status
  {
    Defiled,
    Explored,
    Hidden
  }
}
