using BattleShip.Interface;

using Kronos.Worlds;
using Kronos.Worlds.Directions;

namespace Kronos.Minions
{
  public class TargetingSystem
  {
    public Coordinate CurrentPosition { get; set; }
    public Direction Heading { get; set; }
    public int Hits { get; set; }
    public int Speed { get; set; }

    private Boundaries _boundaries;
    private Coordinate _start;

    public TargetingSystem(Boundaries boundaries)
    {
      _boundaries = boundaries;

      CurrentPosition = new Coordinate(boundaries.West - 1, boundaries.South - 1);
    }

    public TargetingSystem(Boundaries boundaries, Coordinate coordinate, Direction direction, int speed)
    {
      CurrentPosition = new Coordinate(coordinate);
      Heading = direction;
      Hits = 0;
      Speed = speed;

      _boundaries = boundaries;
      _start = new Coordinate(coordinate);
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

    public void Calibrate()
    {
      if (Hits > 1)
        Turn();

      Turn();
    }

    public void Reset()
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
      bool wasOutOfBounds = false;

      if (CurrentPosition.X > _boundaries.East)
      {
        CurrentPosition.X = _boundaries.East;
        wasOutOfBounds = true;
      }

      if (CurrentPosition.X < _boundaries.West)
      {
        CurrentPosition.X = _boundaries.West;
        wasOutOfBounds = true;
      }

      if (CurrentPosition.Y > _boundaries.North)
      {
        CurrentPosition.Y = _boundaries.North;
        wasOutOfBounds = true;
      }

      if (CurrentPosition.Y < _boundaries.South)
      {
        CurrentPosition.Y = _boundaries.South;
        wasOutOfBounds = true;
      }

      return wasOutOfBounds;
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
