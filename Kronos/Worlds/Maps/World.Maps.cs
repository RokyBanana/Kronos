using System.Collections.Generic;
using System.Linq;

using BattleShip.Interface;

using Kronos.Worlds.Maps;

namespace Kronos.Worlds.Maps
{
  public class Map
  {
    public Boundaries Boundaries;
    public int Impacts { get { return _positions.Sum(p => p.Hits); } }
    public Coordinate Impact { get { return _positions[_positions.Count - 1].Coordinate; } }
    private List<Position> _positions;

    public Map(Boundaries boundaries)
    {
      Boundaries = boundaries;
      _positions = new List<Position>();

      CreateMap();
    }

    public bool IsOutside(Coordinate coordinate)
    {
      return IsOutside(coordinate.X, coordinate.Y);
    }

    public bool IsOutside(int latitude, int longitude)
    {
      if (latitude > Boundaries.East)
        return true;

      if (latitude < Boundaries.West)
        return true;

      if (longitude > Boundaries.North)
        return true;

      if (longitude < Boundaries.South)
        return true;

      return false;
    }

    public Status StatusAt(Coordinate coordinate)
    {
      return StatusAt(coordinate.X, coordinate.Y);
    }

    public Status StatusAt(int latitude, int longitude)
    {
      return _positions.Single<Position>(p => p.Coordinate.X == latitude && p.Coordinate.Y == longitude).Status;
    }

    public void Update(Position position)
    {
      Update(position.Coordinate, position.Status);
    }

    public void Update(Coordinate coordinate, Status status)
    {
      _positions.Single<Position>(p => p.Coordinate.X == coordinate.X && p.Coordinate.Y == coordinate.Y).Status = status;
    }

    private void CreateMap()
    {
      for (int latitude = Boundaries.West; latitude <= Boundaries.East; latitude++)
        for (int longitude = Boundaries.South; longitude <= Boundaries.North; longitude++)
          _positions.Add(new Position(new Coordinate(latitude, longitude), Status.Hidden));
    }
  }
}
