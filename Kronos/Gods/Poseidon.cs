using System.Collections.Generic;

using BattleShip.Interface;

using Kronos.Minions;
using Kronos.Worlds;
using Kronos.Worlds.Directions;
using Kronos.Worlds.Maps;

namespace Kronos.Gods
{
  public class Poseidon : God
  {
    public override string Name { get { return "Poseidon"; } }
    public override World World { get; set; }
    public override Coordinate Target { get; set; }

    private List<Minion> _minions = new List<Minion>();
    private Minion _minion;

    public Poseidon() { }

    public override void EvaluateBattleField(int casualties, int defiles)
    {
      if (casualties == 0)
        _minion.CoverTracks(new Position(Target, Status.Explored));

      if (casualties > 0)
      {
        _minion.CoverTracks(new Position(Target, Status.Damaged));
        _minion.RecieveOrders(Orders.Kill);
      }

      if (defiles == 1)
      {
        _minion.CoverTracks(new Position(Target, Status.Defiled));
        _minion.RecieveOrders(Orders.Hunt);
      }
    }

    public override void Play()
    {
      Minion minion = new Minion();

      minion.BattleField = World.Map;
      minion.ReadyForBattle();

      _minions.Add(minion);
      _minion = minion;
    }

    public override Shot Smites()
    {
      if (_minion.Order == Orders.Retire)
        _minions.Remove(_minion);

      _minion.CarryOutHisWill();
      Target = _minion.Target;

      Shot shot = new Shot(Target.X, Target.Y);

      return shot;
    }
  }
}
