using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BattleShip.Interface;

namespace Kronos
{
  public class World
  {
    public Coordinate Coordinate { get; set; }
    public State State { get; set; }

    public World( Coordinate coordinate )
    {
      Coordinate = coordinate;
      State = State.Hidden;
    }

    public World ( Coordinate coordinate, State state )
    {
      Coordinate = coordinate;
      State = state;
    }
  }

  public enum State
  {
    Hidden,
    Visited,
    Destroyed
  }
}
