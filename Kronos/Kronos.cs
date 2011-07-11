using System;
using System.Text;
using System.Collections.Generic;

using BattleShip.Interface;

using Kronos.Worlds;
using Kronos.Worlds.Maps;

namespace Kronos
{
  public class Core : IPlayer
  {
    public static Random Dice = new Random(Environment.TickCount);
    public God God { get; private set; }
    public Observer Observer { get; private set; }
    public World World { get; private set; }

    public Core()
    {
      God = new Gods.Poseidon();
      World = new World();
      Observer = new Observer(World);
    }

    #region IPlayer Members

    public string Name { get { return God.Name; } }

    public void PlaceShips(IPlayerView playerView, ICollection<IVessel> ships)
    {
      World.Boundaries = new Boundaries();
      World.Map = new Map(World.Boundaries);
      World.Create(playerView, ships);

      God.Playground = World;
    }

    public void ShotFeedback(int hits, int sunkShips)
    {
      if (hits > 0)
        World.Map.Update(World.Impacts[World.Impacts.Count - 1], Status.Defiled);
      else
        World.Map.Update(World.Impacts[World.Impacts.Count - 1], Status.Explored);

      Observer.Show();
    }

    public Shot YourTurn(IPlayerView playerView)
    {
      Shot shot = God.Smites(playerView);

      World.Impacts.Add(shot);

      return shot;
    }

    #endregion
  }
}
