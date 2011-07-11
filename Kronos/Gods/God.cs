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
    public abstract World Playground { get; set; }

    public virtual Shot Smites(IPlayerView world) { return new Shot(1,1); }
  }
}

