using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BattleShip.Interface;

using Kronos.Worlds;

namespace Kronos.Gods
{
  public class Poseidon : God
  {
    #region Properties

    public override string Name { get { return "Poseidon"; } }
    public override World Playground { get; set; }

    #endregion

    public Poseidon() { }

    #region Methods

    public override Shot Smites(IPlayerView world)
    {
      return new Shot(Playground.RandomCoordinate.X, Playground.RandomCoordinate.Y);
    }
    #endregion
  }
}
