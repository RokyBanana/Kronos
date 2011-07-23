using System;
using System.Collections.Generic;
using System.Linq;

using BattleShip.Interface;

namespace Kronos.Worlds.Maps
{
  public class Map
  {
    public Boundaries Boundaries { get; set; }
    public Coordinate Impact { get { return _impacts[_lastImpact].Coordinate; } }
    public int Impacts { get { return _impacts.Count(p => p.Hits > 0); } }

    private int _lastImpact;
    private List<Position> _impacts;

    public Map(Boundaries boundaries)
    {
      Boundaries = boundaries;
      _impacts = new List<Position>();

      CreateMap();
    }

    public bool IsOutside(Coordinate coordinate)
    {
      if (coordinate == null)
        throw new ArgumentNullException("coordinate");

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

    public char GetMarker(int latitude, int longitude)
    {
      char marker = ' ';

      switch (StatusAt(latitude, longitude))
      {
        case Status.Hidden:
          marker = ' ';
          break;
        case Status.Explored:
          marker = 'o';
          break;
        case Status.Damaged:
          marker = '!';
          break;
        case Status.Destroyed:
          marker = 'X';
          break;
        case Status.Ignored:
          marker = '.';
          break;
        case Status.Attacked:
          marker = '*';
          break;
        case Status.Tracked:
          marker = '?';
          break;
      }

      return marker;
    }

    public Status StatusAt(Coordinate coordinate)
    {
      if (coordinate == null)
        throw new ArgumentNullException("coordinate");

      return StatusAt(coordinate.X, coordinate.Y);
    }

    public Status StatusAt(int latitude, int longitude)
    {
      if (IsOutside(latitude, longitude))
        return Status.Outside;

      return _impacts.Single<Position>(p => p.Coordinate.X == latitude && p.Coordinate.Y == longitude).Status;
    }

    public void Update(Position position)
    {
      if (position == null)
        throw new ArgumentNullException("position");

      Update(position.Coordinate, position.Status);
    }

    public void Update(Coordinate coordinate, Status status)
    {
      Position impact = _impacts.Single<Position>(p => p.Coordinate.X == coordinate.X && p.Coordinate.Y == coordinate.Y);

      if (status == Status.Damaged || status == Status.Destroyed)
        impact.Hits++;

      impact.Status = status;

      _lastImpact = _impacts.IndexOf(impact);
    }

    private void CreateMap()
    {
      for (int latitude = Boundaries.West; latitude <= Boundaries.East; latitude++)
        for (int longitude = Boundaries.South; longitude <= Boundaries.North; longitude++)
          _impacts.Add(new Position(new Coordinate(latitude, longitude), Status.Hidden));
    }
  }
}
