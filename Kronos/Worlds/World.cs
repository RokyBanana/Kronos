using System.Collections.Generic;

using BattleShip.Interface;

using Kronos.Worlds.Maps;

namespace Kronos.Worlds
{
  public class World
  {
    public Boundaries Boundaries { get; set; }
    public Coordinate RandomCoordinate { get { return new Coordinate(GodFather.Dice.Next(Boundaries.East) + Boundaries.West, GodFather.Dice.Next(Boundaries.North) + Boundaries.South); } }
    public List<Coordinate> Impacts { get; set; }
    public Map Map { get; set; }

    public World() { }

    public void Create(IPlayerView world, ICollection<IVessel> countries)
    {
      Impacts = new List<Coordinate>();
      Map = new Map(Boundaries);

      CreateInternal(world, countries);
    }

    private void CreateInternal(IPlayerView world, ICollection<IVessel> countries)
    {
      Coordinate coordinate;
      Orientation orientation = Orientation.Horizontal;

      coordinate = RandomCoordinate;

      foreach (IVessel country in countries)
      {
        orientation = GodFather.Dice.Next(2) == 1 ? Orientation.Horizontal : Orientation.Vertical;

        if (orientation == Orientation.Horizontal && coordinate.X + country.Length > Boundaries.East)
          coordinate.X -= coordinate.X + country.Length - Boundaries.East;

        if (orientation == Orientation.Vertical && coordinate.Y - country.Length < 1)
          coordinate.Y = country.Length;

        int _try = 0;

        while (!world.PutShip(country.SailTo(coordinate, orientation)))
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
}
