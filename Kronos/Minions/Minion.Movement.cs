using BattleShip.Interface;

using Kronos.Worlds;
using Kronos.Worlds.Directions;
using Kronos.Worlds.Maps;

namespace Kronos.Minions
{
  public class Movement
  {
    public Coordinate CurrentPosition { get; set; }
    public Direction Heading { get; set; }
    public int Speed { get; set; }

    private Boundaries _boundaries;
    private Coordinate _start;
    private Direction _heading;

    public Movement(Boundaries boundaries)
    {
      _boundaries = boundaries;

      CurrentPosition = new Coordinate(boundaries.West - 1, boundaries.South - 1);
    }

    public Movement(Boundaries boundaries, Coordinate coordinate, Direction direction, int speed)
    {
      _boundaries = boundaries;

      CurrentPosition = new Coordinate(coordinate);
      Heading = direction;
      Speed = speed;

      _start = new Coordinate(coordinate);
      _heading = direction;
    }

    public void Advance()
    {
      switch (Heading)
      {
        case Direction.North:
          MoveNorth();
          break;
        case Direction.East:
          MoveEast();
          break;
        case Direction.South:
          MoveSouth();
          break;
        case Direction.West:
          MoveWest();
          break;
      }
    }

    public void Regroup()
    {
      CurrentPosition = new Coordinate(_start);
    }

    public void Turn()
    {
      switch (Heading)
      {
        case Direction.North:
          Heading = Direction.East;
          break;
        case Direction.East:
          Heading = Direction.South;
          break;
        case Direction.South:
          Heading = Direction.West;
          break;
        case Direction.West:
          Heading = Direction.North;
          break;
      }
    }

    public bool UpdateVector()
    {
      if (CurrentPosition.X > _boundaries.East)
      {
        CurrentPosition.X = _boundaries.East;

        return true;
      }

      if (CurrentPosition.X < _boundaries.West)
      {
        CurrentPosition.X = _boundaries.West;

        return true;
      }

      if (CurrentPosition.Y > _boundaries.North)
      {
        CurrentPosition.Y = _boundaries.North;

        return true;
      }

      if (CurrentPosition.Y < _boundaries.South)
      {
        CurrentPosition.Y = _boundaries.South;

        return true;
      }

      return false;
    }

    private void MoveNorth()
    {
      CurrentPosition.Y += Speed;
    }

    private void MoveEast()
    {
      CurrentPosition.X += Speed;
    }

    private void MoveSouth()
    {
      CurrentPosition.Y -= Speed;
    }

    public void MoveWest()
    {
      CurrentPosition.X -= Speed;
    }
  }
}
