using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using BattleShip.Interface;

using Kronos.Worlds;
using Kronos.Worlds.Directions;
using Kronos.Worlds.Maps;

namespace Kronos.Minions
{
  public class Minion
  {
    public Coordinate Target { get; set; }
    public int Hits { get; set; }
    public int Kills { get; set; }
    public Map Battlefield { get; set; }
    public MinionState State { get { return _state; } }
    public OrderType Order { get; set; }
    public ReadOnlyCollection<Coordinate> BattlePlan { get { return _battlePlan.AsReadOnly(); } }
    public string Name { get; set; }

    private List<Coordinate> _battlePlan;
    private Movement _huntingVector;
    private Movement _trackingVector;
    private MinionState _state;

    public Minion()
    {
      Name = "Triton";

      _battlePlan = new List<Coordinate>();
      _state = MinionState.Acquiring;
    }

    #region Methods

    public void ObeyOrder()
    {
      if (Hits > 5)
        throw new System.InvalidOperationException("Not effin possible...");

      if (Order == OrderType.Hunt)
        AcquireTarget();
      else
        EngageTarget();
    }

    public void ReceiveOrders(OrderType order)
    {
      Order = order;

      switch (Order)
      {
        case OrderType.Hunt:
          Hunt();
          break;
        case OrderType.Kill:
          Kill();
          break;
        case OrderType.Retire:
          break;
      }
    }

    public void CoverTracks(Position position)
    {
      if (Order == OrderType.Kill && position.Status != Status.Damaged)
        _state = MinionState.TargetLost;
      if (Order == OrderType.Hunt && (position.Status == Status.Explored || position.Status == Status.Ignored))
        _state = MinionState.Acquiring;

      Battlefield.Update(position);
      RemoveBattleplanItem(position.Coordinate);
    }

    public void RemoveBattleplanItem(Coordinate zone)
    {
      _battlePlan.Remove(_battlePlan.Find(c => c.X == zone.X && c.Y == zone.Y));
    }

    public void ReadyForBattle()
    {
      List<Coordinate> battleplan = SurveyBattlefield();

      _battlePlan = (battleplan.OrderBy(c => (c.X + c.Y) % 4)).ToList();
    }

    #endregion

    private void Hunt()
    {
      _huntingVector = new Movement(Battlefield.Boundaries, _battlePlan[0], Direction.North, 2);
      _state = MinionState.Acquiring;
    }

    private void Kill()
    {
      _trackingVector = new Movement(Battlefield.Boundaries, new Coordinate(Target), _huntingVector.Heading, 1);
      _state = MinionState.TargetAcquired;
      Hits = 0;
    }

    private void AcquireTarget()
    {
      while (_state == MinionState.Acquiring)
        FindEnemy();

      Target = _huntingVector.CurrentPosition;
    }

    private void FindEnemy()
    {
      Status zoneStatus = Battlefield.StatusAt(_huntingVector.CurrentPosition);

      //ShowMinion(_huntingVector.CurrentPosition, Status.Tracked);

      if (zoneStatus == Status.Hidden)
        _state = MinionState.TargetAcquired;
      else
      {
        _huntingVector.Advance();

        if (Battlefield.IsOutside(_huntingVector.CurrentPosition))
        {
          _huntingVector.UpdateVector();
          _huntingVector.Turn();
        }

        if (!BattlePlan.Contains(_huntingVector.CurrentPosition))
          Hunt();
      }
    }

    private void EngageTarget()
    {
      if (_state == MinionState.TargetLost)
      {
        _trackingVector.Regroup();
        SmartMove();
        _state = MinionState.TargetAcquired;
      }

      while (_state == MinionState.TargetAcquired)
        AttackEnemy();

      _state = MinionState.TargetAcquired;
      Target = _trackingVector.CurrentPosition;
    }

    private void AttackEnemy()
    {
      Status zoneStatus = Battlefield.StatusAt(_trackingVector.CurrentPosition);

      if (zoneStatus == Status.Hidden)
        _state = MinionState.Attacking;
      else if (zoneStatus == Status.Damaged)
        _trackingVector.Advance();
      else
      {
        _trackingVector.Regroup();
        SmartMove();
        _trackingVector.Advance();
      }

      if (!Battlefield.IsOutside(_trackingVector.CurrentPosition))
        ShowMinion(_trackingVector.CurrentPosition, Status.Attacked);

      if (_trackingVector.UpdateVector())
      {
        _trackingVector.Regroup();
        SmartMove();
        _trackingVector.Advance();
      }
    }

    private void SmartMove()
    {
      if (Hits > 1)
        _trackingVector.Turn();

      _trackingVector.Turn();
    }

    private void ShowMinion(Coordinate coordinate, Status status)
    {
      Status savedStatus = Battlefield.StatusAt(coordinate);

      Battlefield.Update(coordinate, status);
      Observer.Show(Battlefield);
      Battlefield.Update(coordinate, savedStatus);
    }

    private List<Coordinate> SurveyBattlefield()
    {
      List<Coordinate> battlefield = new List<Coordinate>();

      for (int latitude = Battlefield.Boundaries.West; latitude <= Battlefield.Boundaries.East; latitude++)
        for (int longitude = Battlefield.Boundaries.South; longitude <= Battlefield.Boundaries.North; longitude++)
          battlefield.Add(new Coordinate(latitude, longitude));

      return battlefield;
    }
  }
}
