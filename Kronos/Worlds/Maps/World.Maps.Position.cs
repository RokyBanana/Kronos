using BattleShip.Interface;

namespace Kronos.Worlds.Maps
{
  public class Position
  {
    public Coordinate Coordinate { get; set; }
    public int Hits { get; set; }
    public Status Status { get; set; }

    public Position()
    {
      Coordinate = new Coordinate(0, 0);
    }
    
    public Position(Coordinate coordinate, Status status)
    {
      Coordinate = new Coordinate(coordinate);
      Status = status;
    }
  }
}
