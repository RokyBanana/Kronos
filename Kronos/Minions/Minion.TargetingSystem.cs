using Kronos.Worlds;
using Kronos.Worlds.Directions;

namespace Kronos.Minions
{
  public class TargetingSystem
  {
    public Coordinate Target { get; set; }
    public Compass Direction { get; set; }
    public int Hits { get; set; }
    public int Speed { get; set; }

    private Boundaries _boundaries;
    private Coordinate _start;

    public TargetingSystem(Boundaries boundaries)
    {
      _boundaries = boundaries;
    }

    public TargetingSystem(Boundaries boundaries, Coordinate coordinate, Compass direction, int speed)
    {
      Target = new Coordinate(coordinate);
      Direction = direction;
      Hits = 1;
      Speed = speed;

      _boundaries = boundaries;
      _start = new Coordinate(coordinate);
    }

    public void Advance()
    {
      Target.Move(Coordinate.GetHeading(Direction));
    }

    public void Calibrate()
    {
      if (Hits > 1)
        Turn();

      Turn();
    }

    public void Reset()
    {
      Target = new Coordinate(_start);
    }

    public void Turn()
    {
      switch (Direction)
      {
        case Compass.North:
          Direction = Compass.East;
          break;
        case Compass.East:
          Direction = Compass.South;
          break;
        case Compass.South:
          Direction = Compass.West;
          break;
        case Compass.West:
          Direction = Compass.North;
          break;
      }
    }

    public bool IsAtBoundary()
    {
      Coordinate state = new Coordinate(Target.Latitude, Target.Longitude);

      if (Target.Latitude > _boundaries.East)
        Target.Latitude = _boundaries.East;

      if (Target.Latitude < _boundaries.West)
        Target.Latitude = _boundaries.West;

      if (Target.Longitude > _boundaries.North)
        Target.Longitude = _boundaries.North;

      if (Target.Longitude < _boundaries.South)
        Target.Longitude = _boundaries.South;

      return !Target.Equals(state);
    }
  }
}
