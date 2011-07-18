using System.Collections.Generic;

namespace Kronos.Helpers
{
  public class CoordinateComparer : IEqualityComparer<BattleShip.Interface.Coordinate>
  {
    public bool Equals(BattleShip.Interface.Coordinate x, BattleShip.Interface.Coordinate y)
    {
      if (x == null || y == null)
        return false;

      return x.X == y.X && x.Y == y.Y;
    }

    public int GetHashCode(BattleShip.Interface.Coordinate obj)
    {
      if (obj == null)
        return 0;

      return obj.GetHashCode();
    }
  }
}
