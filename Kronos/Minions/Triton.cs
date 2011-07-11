using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BattleShip.Interface;

using Kronos.Worlds;
using Kronos.Minions;

namespace Kronos
{
  public class Triton : Minion
  {
    #region Properties

    public override string Name { get; set; }
    public God Father { get; set; }
    public World World { get; set; }

    #endregion

    public Triton() { }
    public Triton( God father ) {Father = father;}

    #region Methods

    internal void TrackPrey( int hits )
    {
    }

    public Coordinate HuntingAt()
    {
      Coordinate coordinate = new Coordinate(1, 1);

      coordinate.X++;

      if ( coordinate.X > World.Boundaries.East )
      {
        coordinate.X = World.Boundaries.West;
        coordinate.Y++;
      }

      if ( coordinate.Y > World.Boundaries.North )
        coordinate.Y = World.Boundaries.South;

      return coordinate;
    }

    #endregion
  }
}
