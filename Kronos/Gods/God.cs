using BattleShip.Interface;

using Kronos.Worlds;

namespace Kronos.Gods
{
  public abstract class God
  {
    public static int Smites { get; set; }
    public abstract string Name { get; }
    public abstract World World { get; set; }

    public abstract void Play();
    public abstract void Contemplate(int casualties, int defiles);
    public virtual Shot Smite()
    {
      Shot shot = new Shot(World.RandomCoordinate.X, World.RandomCoordinate.Y);

      return shot;
    }
  }
}

