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

    private List<Coordinate> _killHistory = new List<Coordinate>();
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

        _killHistory.Add(Target);
      }

      if (defiles == 1)
      {
        _minion.CoverTracks(new Position(Target, Status.Damaged));
        _minion.RecieveOrders(Orders.Hunt);

        _killHistory.Add(Target);

        RevealEnemy();
      }

      Observer.Show(World.Map);
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

    private void RevealEnemy()
    {
      Coordinate enemyBox;

      foreach (Coordinate coordinate in _killHistory)
      {
        for (int latitude=-1; latitude <= 1; latitude++)
          for (int longitude=-1; longitude <= 1; longitude++)
          {
            enemyBox = new Coordinate(coordinate.X + latitude, coordinate.Y + longitude);

            if (World.Map.IsOutside(enemyBox))
              continue;

            _minion.BattlePlan.RemoveAll(c => c.X == enemyBox.X && c.Y == enemyBox.Y);

            if (World.Map.StatusAt(enemyBox) == Status.Hidden)
              _minion.BattleField.Update(enemyBox, Status.Explored);
          }
      }

      foreach (Coordinate coordinate in _killHistory)
        _minion.BattleField.Update(coordinate, Status.Defiled);

      _killHistory.Clear();
    }

  }
}
