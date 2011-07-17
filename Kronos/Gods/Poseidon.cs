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

    public override void EvaluateBattlefield(int casualties, int defiles)
    {
      if (casualties == 0)
        _minion.CoverTracks(new Position(Target, Status.Explored));

      if (casualties > 0)
      {
        _killHistory.Add(Target);
        _minion.CoverTracks(new Position(Target, Status.Damaged));
        _minion.ReceiveOrders(OrderType.Kill);
      }

      if (defiles == 1)
      {
        _killHistory.Add(Target);
        _minion.CoverTracks(new Position(Target, Status.Damaged));
        _minion.ReceiveOrders(OrderType.Hunt);

        UseDivinePower();
      }

      Observer.Show(World.Map);
    }

    public override void Play()
    {
      Minion minion = new Minion();

      minion.Battlefield = World.Map;
      minion.ReadyForBattle();

      _minions.Add(minion);
      _minion = minion;
    }

    public override Shot Smites()
    {
      if (_minion.Order == OrderType.Retire)
        _minions.Remove(_minion);

      _minion.ObeyOrder();
      Target = _minion.Target;

      Shot shot = new Shot(Target.X, Target.Y);

      return shot;
    }

    private void UseDivinePower()
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

            _minion.RemovePath(c => c.X == enemyBox.X && c.Y == enemyBox.Y);

            if (World.Map.StatusAt(enemyBox) == Status.Hidden)
              _minion.Battlefield.Update(enemyBox, Status.Explored);
          }
      }

      foreach (Coordinate coordinate in _killHistory)
        _minion.Battlefield.Update(coordinate, Status.Defiled);

      _killHistory.Clear();
    }

  }
}
