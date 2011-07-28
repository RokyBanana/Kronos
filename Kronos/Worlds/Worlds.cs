using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using BattleShip.Interface;

using Kronos.Helpers;
using Kronos.Worlds.Maps;

namespace Kronos.Worlds
{
  public class World
  {
    public Boundaries Boundaries { get; set; }
    public Collection<IVessel> Enemies { get { return new Collection<IVessel>(_enemies); } }
    public Coordinate RandomCoordinate { get { return new Coordinate(Dice.Next(Boundaries.East) + Boundaries.West, Dice.Next(Boundaries.North) + Boundaries.South); } }
    public Map Map { get; set; }

    private List<IVessel> _enemies;

    public World() { }

    public void Create(IPlayerView world, ICollection<IVessel> countries)
    {
      _enemies = new List<IVessel>();
      Map = new Map(Boundaries);

      CreateInternal(world, countries);
    }

    private void CreateInternal(IPlayerView world, ICollection<IVessel> enemies)
    {
      Coordinate coordinate;
      int tries = 0;
      Orientation orientation = Orientation.Horizontal;

      foreach (IVessel enemy in enemies.OrderByDescending(v=>v.Length))
      {
        Enemies.Add(enemy);
        coordinate = RandomCoordinate;
        orientation = Dice.Next(2) == 1 ? Orientation.Horizontal : Orientation.Vertical;

        if (orientation == Orientation.Horizontal && coordinate.Latitude + enemy.Length > Boundaries.East)
          coordinate.Latitude -= coordinate.Latitude + enemy.Length - Boundaries.East;

        if (orientation == Orientation.Vertical && coordinate.Longitude - enemy.Length < 1)
          coordinate.Longitude = enemy.Length;

        AvoidEdges(coordinate);

        while (!world.PutShip(enemy.SailTo(coordinate.ToInterfaceCoordinate(), orientation)))
        {
          if (++coordinate.Latitude > Boundaries.East)
            coordinate.Latitude = Boundaries.West;

          if (++coordinate.Longitude > Boundaries.North)
            coordinate.Longitude = Boundaries.South;

          if (++tries > 10)
            orientation = orientation == Orientation.Horizontal ? Orientation.Vertical : Orientation.Horizontal;

          if (tries > 20)
          {
            tries = 0;
            coordinate = RandomCoordinate;
          }

          AvoidEdges(coordinate);
        }
      }
    }

    private void AvoidEdges(Coordinate coordinate)
    {
      if (coordinate.Longitude == Map.Boundaries.North)
        coordinate.Longitude--;

      if (coordinate.Latitude == Map.Boundaries.East)
        coordinate.Latitude--;

      if (coordinate.Longitude == Map.Boundaries.South)
        coordinate.Longitude++;

      if (coordinate.Latitude == Map.Boundaries.West)
        coordinate.Latitude++;
    }
  }
}
