using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Kronos.Worlds;
using Kronos.Worlds.Directions;
using Kronos.Worlds.Maps;

namespace Kronos.Minions
{
  public class Minion
  {
    private List<Coordinate> _battlePlan;
    private TargetingSystem _huntingVector;
    private TargetingSystem _trackingVector;
    private MinionState _state;

    public Coordinate Target { get; set; }
    public int EnemySize { get { return _trackingVector == null ? 0 : _trackingVector.Hits; } }
    public Map Battlefield { get; set; }
    public MinionState State { get { return _state; } }
    public OrderType Order { get; set; }
    public ReadOnlyCollection<Coordinate> BattlePlan { get { return _battlePlan.AsReadOnly(); } }

    public Minion()
    {
      _battlePlan = new List<Coordinate>();
      _state = MinionState.Acquiring;
    }

    public void ObeyOrder()
    {
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
      if (position == null)
        throw new ArgumentNullException("position");

      if (Order == OrderType.Kill && position.Status != Status.Damaged)
        _state = MinionState.TargetLost;

      if (Order == OrderType.Kill && position.Status == Status.Damaged)
        _trackingVector.Hits++;

      if (Order == OrderType.Hunt && (position.Status == Status.Explored || position.Status == Status.Ignored))
        _state = MinionState.Acquiring;

      Battlefield.Update(position);
      RemoveBattlePlanItem(position.Coordinate);
    }

    public void RemoveBattlePlanItem(Coordinate zone)
    {
      _battlePlan.Remove(_battlePlan.Find(c => c == zone));
    }

    public void ReadyForBattle()
    {
      List<Coordinate> battleplan = SurveyBattlefield();

      _battlePlan = battleplan.Where(c => (c.Latitude + c.Longitude) % 2 == 1).OrderBy(c => c.Latitude + c.Longitude).ToList();
      //_battlePlan.Shuffle();
    }

    private void Hunt()
    {
      if (_battlePlan.Count == 0)
        _battlePlan = Battlefield.GetAll(Status.Hidden).ToList();

      //_huntingVector = new TargetingSystem(Battlefield.Boundaries, _battlePlan[0], Compass.North, 1);
      Recon();
      _huntingVector = new TargetingSystem(Battlefield.Boundaries, EmptyPoint(), Compass.North, 1);
      _state = MinionState.Acquiring;
    }

    public void Recon()
    {
      List<Position> battlefield = new List<Position>(Battlefield.Layout);

      foreach (Position position in battlefield)
      {
        int count = 0;

        if (position.Status == Status.Hidden)
          foreach (Coordinate neighbor in position.Coordinate.Neighbors())
            count += CountEmptyNeighbors(neighbor, Coordinate.GetDirection(position.Coordinate, neighbor));

        position.NeighborCount = count;
      }
    }

    private Coordinate EmptyPoint()
    {
      List<Coordinate> top = Battlefield.Layout.Where(c => c.NeighborCount == Battlefield.Layout.Max(m => m.NeighborCount)).Select(c => c.Coordinate).ToList();
      top.Shuffle();

      return top.First();
    }

    private int CountEmptyNeighbors(Coordinate neighbor, Compass compass)
    {
      int count = 0;

      while (true)
      {
        if (Battlefield.StatusAt(neighbor.Latitude, neighbor.Longitude) != Status.Hidden)
          break;

        neighbor.Move(Coordinate.GetHeading(compass));
        count++;
      }

      return count;
    }

    private void Kill()
    {
      _trackingVector = new TargetingSystem(Battlefield.Boundaries, new Coordinate(Target), _huntingVector.Direction, 1);
      _state = MinionState.TargetAcquired;
    }

    private void AcquireTarget()
    {
      while (_state == MinionState.Acquiring)
        FindEnemy();

      Target = _huntingVector.Target;
    }

    private void FindEnemy()
    {
      Status zoneStatus = Battlefield.StatusAt(_huntingVector.Target);

      if (zoneStatus == Status.Hidden)
        _state = MinionState.TargetAcquired;
      else
      {
        _huntingVector.Advance();

        if (_huntingVector.IsAtBoundary())
          _huntingVector.Turn();

        if (!BattlePlan.Contains(_huntingVector.Target))
          Hunt();
      }
    }

    private void EngageTarget()
    {
      if (_state == MinionState.TargetLost)
      {
        _trackingVector.Reset();
        _trackingVector.Calibrate();
        _state = MinionState.TargetAcquired;
      }

      while (_state == MinionState.TargetAcquired)
        AttackEnemy();

      _state = MinionState.TargetAcquired;
      Target = _trackingVector.Target;
    }

    private void AttackEnemy()
    {
      Status zoneStatus = Battlefield.StatusAt(_trackingVector.Target);

      if (zoneStatus == Status.Hidden)
        _state = MinionState.Attacking;
      else if (zoneStatus == Status.Damaged)
        _trackingVector.Advance();
      else
      {
        _trackingVector.Reset();
        _trackingVector.Calibrate();
        _trackingVector.Advance();
      }

      if (_trackingVector.IsAtBoundary())
      {
        _trackingVector.Reset();
        _trackingVector.Calibrate();
        _trackingVector.Advance();
      }
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
