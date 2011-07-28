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
    public string Name { get { return God.Name; } }
    public World World { get; private set; }

    public Godfather()
    {
      God = Olympus.SendGod();
      World = new World();
    }

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
      God.Smite();

      return God.Smote();
    }
  }
}
