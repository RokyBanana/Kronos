using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BattleShip.Interface;

using Kronos.Worlds;

namespace Kronos
{
  public abstract class God
  {
    public abstract string Name { get; }
    public abstract World World { get; set; }
    public abstract Coordinate LastSmite { get; set; }

    public abstract void Dominate();
    public abstract void EvaluateBattleField(int casualties, int defiles);
    public virtual Shot Smites(IPlayerView world)
    {
      Shot shot = new Shot(World.RandomCoordinate.X, World.RandomCoordinate.Y);

      World.Impacts.Add(shot);

      return shot;
    }
  }
}

