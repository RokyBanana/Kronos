using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BattleShip.Interface;

using Kronos.Minions;
using Kronos.Worlds;
using Kronos.Worlds.Maps;

namespace Kronos.Gods
{
  public class Poseidon : God
  {
    #region Properties

    public override string Name { get { return "Poseidon"; } }
    public override World World { get; set; }
    public override Coordinate LastSmite { get; set; }

    #endregion

    private List<Coordinate> _battlePlan;

    public Poseidon() { }

    #region Methods

    public override void EvaluateBattleField(int casualties, int defiles)
    {
      if (casualties == 0)
      {
        World.Map.Update(LastSmite, Status.Explored);
        _battlePlan.Reverse(0, _battlePlan.Count / 2);
      }
      else
        World.Map.Update(LastSmite, Status.Defiled);
    }

    public override void Dominate()
    {
      _battlePlan = CreateBattlePlan();
    }

    public override Shot Smites(IPlayerView world)
    {
      LastSmite = _battlePlan[0];

      Shot shot = new Shot(LastSmite.X, LastSmite.Y);
      
      _battlePlan.Remove(LastSmite);

      return shot;
    }

    #endregion

    private List<Coordinate> CreateBattlePlan()
    {
      List<Coordinate> battlePlan = new List<Coordinate>();
      
      for (int latitude=World.Boundaries.West; latitude <= World.Boundaries.East; latitude++)
        for (int longitude=World.Boundaries.South; longitude <= World.Boundaries.North; longitude++)
          battlePlan.Add(new Coordinate(latitude, longitude));

      return battlePlan;
    }
  }
}
