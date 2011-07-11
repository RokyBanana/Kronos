using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BattleShip.Interface;
using Kronos.Worlds.Maps;

namespace Kronos.Worlds
{
  public class Map
  {
    private Boundaries _boundaries;
    private List<Position> _positions;

    public Map(Boundaries boundaries)
    {
      _boundaries = boundaries;
      _positions = new List<Position>();

      CreateMap();
    }

    internal Status StatusAt(int latitude, int longitude)
    {
      return _positions.Single<Position>(p => p.Coordinate.X == latitude && p.Coordinate.Y == longitude).Status;
    }

    internal void Update(Coordinate coordinate, Status status)
    {
      _positions.Single<Position>(p => p.Coordinate.X == coordinate.X && p.Coordinate.Y == coordinate.Y).Status = status;
    }

    private void CreateMap()
    {
      for (int latitude = _boundaries.West; latitude <= _boundaries.East; latitude++)
        for (int longitude=_boundaries.South; longitude <= _boundaries.North; longitude++)
          _positions.Add(new Position(new Coordinate(latitude, longitude), Status.Hidden));
    }
  }
}
