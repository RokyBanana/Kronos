using BattleShip.Interface;

using Kronos.Worlds.Directions;

namespace Kronos.Minions
{
  public class Movement
  {
    public Coordinate Coordinate { get; set; }
    public Coordinate StartPosition { get; set; }
    public Direction Direction { get; set; }
    public int Speed { get; set; }

    public Movement()
    {
      Coordinate = new Coordinate(0, 0);
      StartPosition = new Coordinate(0, 0);
    }

    public Movement(Coordinate coordinate, Direction direction, int speed)
    {
      Coordinate = new Coordinate(coordinate);
      Direction = direction;
      Speed = speed;
    }

    public void Advance()
    {
      switch (Direction)
      {
        case Direction.North:
          Coordinate.Y += Speed;
          break;
        case Direction.East:
          Coordinate.X += Speed;
          break;
        case Direction.South:
          Coordinate.Y -= Speed;
          break;
        case Direction.West:
          Coordinate.X -= Speed;
          break;
      }
    }

    public void Regroup()
    {
      Coordinate = new Coordinate(StartPosition);
      Turn();
    }

    public void Turn()
    {
      switch(Direction)
      {
        case Direction.North:
          Direction = Direction.East;
          break;
        case Direction.East:
          Direction = Direction.South;
          break;
        case Direction.South:
          Direction = Direction.West;
          break;
        case Direction.West:
          Direction = Direction.North;
          break;
      }
    }
  }
}
