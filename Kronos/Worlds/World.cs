using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BattleShip.Interface;
using Kronos;

namespace Kronos.Worlds
{
  public class World
  {
    public Boundaries Boundaries { get; set; }
    public Coordinate Coordinate { get; set; }
    public Coordinate RandomCoordinate { get { return new Coordinate(Core.Dice.Next(Boundaries.East), Core.Dice.Next(Boundaries.North)); } }
    public static int Impacts { get; set; }
    public State State { get; set; }

    public World() { }

    public void Create(IPlayerView world, ICollection<IVessel> enemies)
    {
      Coordinate coordinate;
      Orientation orientation = Orientation.Horizontal;

      coordinate = RandomCoordinate;

      foreach (IVessel ship in enemies)
      {
        orientation = Core.Dice.Next(2) == 1 ? Orientation.Horizontal : Orientation.Vertical;

        if (orientation == Orientation.Horizontal && coordinate.X + ship.Length > Boundaries.East)
          coordinate.X -= coordinate.X + ship.Length - Boundaries.East;

        if (orientation == Orientation.Vertical && coordinate.Y - ship.Length < 1)
          coordinate.Y = ship.Length;

        int _try = 0;

        while (!world.PutShip(ship.SailTo(coordinate, orientation)))
        {
          if (++coordinate.X > Boundaries.East)
            coordinate.X = Boundaries.West;

          if (--coordinate.Y > Boundaries.South)
            coordinate.Y = Boundaries.North;

          if (++_try > 10)
          {
            _try = 0;
            coordinate = RandomCoordinate;
          }
        }
      }
    }
  }

  public enum State
  {
    Hidden,
    Visited,
    Destroyed
  }
}
