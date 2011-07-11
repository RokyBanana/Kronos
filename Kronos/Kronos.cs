using System;
using System.Text;
using System.Collections.Generic;

using BattleShip.Interface;

using Kronos.Worlds;

namespace Kronos
{
  public class Core : IPlayer
  {
    public static Random Dice = new Random(Environment.TickCount);
    public God God { get; private set; }
    public World World { get; private set; }

    public Core()
    {
      God = new Gods.Poseidon();
      World = new World();
    }

    #region IPlayer Members

    public string Name { get { return God.Name; } }

    public void PlaceShips(IPlayerView playerView, ICollection<IVessel> ships)
    {
      World.Boundaries = new Boundaries();
      World.Create(playerView, ships);

      God.Playground = World;
    }

    public void ShotFeedback(int hits, int sunkShips)
    {
    }

    public Shot YourTurn(IPlayerView playerView)
    {
      return God.Smites(playerView);
    }

    #endregion
  }
}
