using System.Collections.Generic;

using BattleShip.Interface;

using Kronos.Gods;
using Kronos.Worlds;
using Kronos.Worlds.Maps;

namespace Kronos
{
  public class Godfather : IPlayer
  {
    public God God { get; private set; }
    public World World { get; private set; }

    public Godfather()
    {
      God = Olympus.SendGod();
      World = new World();
    }

    #region IPlayer Members

    public string Name { get { return God.Name; } }

    public void PlaceShips(IPlayerView playerView, ICollection<IVessel> ships)
    {
      World.Boundaries = new Boundaries();
      World.Map = new Map(World.Boundaries);
      World.Create(playerView, ships);

      God.World = World;
      God.Play();
    }

    public void ShotFeedback(int hits, int sunkShips)
    {
      God.Contemplate(hits, sunkShips);
    }

    public Shot YourTurn(IPlayerView playerView)
    {
      Shot shot = God.Smite();

      return shot;
    }

    #endregion
  }
}
