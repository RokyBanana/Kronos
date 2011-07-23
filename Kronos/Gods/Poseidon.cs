using System.Collections.Generic;
using System.Linq;

using BattleShip.Interface;

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

    public override void Contemplate(int casualties, int defiles)
    {
      Coordinate target = new Coordinate(_minion.Target);

      Smites++;

      if (casualties == 0)
        _minion.CoverTracks(new Position(target, Status.Explored));

      if (casualties > 0)
      {
        _minion.CoverTracks(new Position(target, Status.Damaged));

        if (_minion.Order == OrderType.Hunt)
          _minion.ReceiveOrders(OrderType.Kill);

        _killHistory.Add(target);
      }

      if (defiles > 0)
      {
        _minion.ReceiveOrders(OrderType.Hunt);

        UseDivinePower();
      }

#if DEBUG
      Observer.Show(World.Map);
#endif
    }

    public override void Play()
    {
      Minion minion = new Minion();

      minion.Battlefield = World.Map;
      minion.ReadyForBattle();
      minion.ReceiveOrders(OrderType.Hunt);

      _minions.Add(minion);
      _minion = minion;
    }

    public override Shot Smite()
    {
      if (_minion.Order == OrderType.Retire)
        _minions.Remove(_minion);

      _minion.ObeyOrder();

      Shot shot = new Shot(_minion.Target.X, _minion.Target.Y);

      return shot;
    }

    private void UseDivinePower()
    {
      Coordinate ignore;

      _killHistory = _killHistory.OrderBy(cX => cX.X).ThenBy(cY => cY.Y).ToList();

      foreach (Coordinate coordinate in _killHistory)
        _minion.Battlefield.Update(coordinate, Status.Destroyed);

      foreach (Coordinate coordinate in _killHistory)
      {
        for (int latitude=-1; latitude <= 1; latitude++)
          for (int longitude=-1; longitude <= 1; longitude++)
          {
            ignore = new Coordinate(coordinate.X + latitude, coordinate.Y + longitude);

            if (World.Map.IsOutside(ignore))
              continue;

            if (_minion.Battlefield.StatusAt(ignore) != Status.Hidden)
              continue;

            _minion.RemoveBattleplanItem(ignore);
            _minion.Battlefield.Update(ignore, Status.Ignored);
          }
      }

      _killHistory.Clear();
    }
  }
}
