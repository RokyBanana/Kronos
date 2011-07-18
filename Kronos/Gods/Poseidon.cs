using System.Collections.Generic;
using System.Linq;

using BattleShip.Interface;

using Kronos.Helpers;
using Kronos.Minions;
using Kronos.Worlds;
using Kronos.Worlds.Maps;

namespace Kronos.Gods
{
  public class Poseidon : God
  {
    public override string Name { get { return "Poseidon"; } }
    public override World World { get; set; }

    private List<Coordinate> _killHistory = new List<Coordinate>();
    private List<Minion> _minions = new List<Minion>();
    private Minion _minion;

    public Poseidon() { }

    public override void EvaluateBattlefield(int casualties, int defiles)
    {
      Coordinate target = new Coordinate(_minion.Target);

      if (casualties == 0)
        _minion.CoverTracks(new Position(target, Status.Explored));

      if (casualties > 0)
      {
        _minion.CoverTracks(new Position(target, Status.Damaged));
        _minion.ReceiveOrders(OrderType.Kill);
        _killHistory.Add(target);
      }

      if (defiles == 1)
      {
        _minion.CoverTracks(new Position(target, Status.Damaged));
        _minion.ReceiveOrders(OrderType.Hunt);
        _killHistory.Add(target);

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

      Shot shot = new Shot(_minion.Target.X, _minion.Target.Y);

      return shot;
    }

    private void UseDivinePower()
    {
      Coordinate enemyBox;

      _killHistory.OrderBy(cX => cX.X).ThenBy(cY => cY.Y);
      _killHistory = _killHistory.Distinct(new CoordinateComparer()).ToList();

      foreach (Coordinate coordinate in _killHistory)
      {
        for (int latitude=-1; latitude <= 1; latitude++)
          for (int longitude=-1; longitude <= 1; longitude++)
          {
            enemyBox = new Coordinate(coordinate.X + latitude, coordinate.Y + longitude);

            if (World.Map.IsOutside(enemyBox))
              continue;

            _minion.RemoveBattleZoneCoordinate(enemyBox);

            if (_minion.Battlefield.StatusAt(enemyBox) == Status.Hidden)
              _minion.Battlefield.Update(enemyBox, Status.Explored);
          }
      }

      foreach (Coordinate coordinate in _killHistory)
        _minion.Battlefield.Update(coordinate, Status.Destroyed);

      _killHistory.Clear();
    }
  }
}
