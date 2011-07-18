using BattleShip.Interface;

using Kronos.Worlds;

namespace Kronos.Gods
{
  public abstract class God
  {
    public abstract string Name { get; }
    public abstract World World { get; set; }

    public abstract void Play();
    public abstract void EvaluateBattlefield(int casualties, int defiles);
    public virtual Shot Smites()
    {
      Shot shot = new Shot(World.RandomCoordinate.X, World.RandomCoordinate.Y);

      return shot;
    }
  }
}

