using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using BattleShip.Interface;

using Kronos.Worlds;
using Kronos.Worlds.Maps;
using Kronos.Worlds.Directions;

namespace Kronos.Minions
{
  public class Minion
  {
    public Coordinate Target { get; set; }
    public ReadOnlyCollection<Coordinate> BattlePlan { get { return _battlePlan.AsReadOnly(); } }
    public Map Battlefield { get; set; }
    public Movement HuntingVector { get; set; }
    public OrderType Order { get; set; }
    public string Name { get; set; }

    private readonly List<Coordinate> _battlePlan;
    private List<Coordinate> _footPrints;
    private Movement _trackingVector;
    private MinionState _state;

    public int RemovePath(Predicate<Coordinate> match) { return _battlePlan.RemoveAll(match); }

    public Minion()
    {
      Name = "Triton";

      _battlePlan = new List<Coordinate>();
      _footPrints = new List<Coordinate>();
      _state = MinionState.Acquiring;
    }

    #region Methods

    public void ObeyOrder()
    {
      if (Order == OrderType.Hunt)
        AcquireTarget();

      if (Order == OrderType.Kill)
        SmiteEnemy();
    }

    public void ReceiveOrders(OrderType order)
    {
      if (order == Order)
        return;

      Order = order;

      switch (order)
      {
        case OrderType.Hunt:
          break;
        case OrderType.Kill:
          Position position = new Position(Target, Status.Damaged);

          _trackingVector = new Movement(position.Coordinate, Direction.East, 1);
          _trackingVector.StartPosition = position.Coordinate;
          break;
        case OrderType.Retire:
          break;
      }
    }

    public void CoverTracks(Position position)
    {
      if (Order == OrderType.Kill && position.Status == Status.Explored)
        _state = MinionState.TargetLost;

      _footPrints.Add(position.Coordinate);

      Battlefield.Update(position);
      _battlePlan.Remove(_battlePlan.Find(c => c.X == position.Coordinate.X && c.Y == position.Coordinate.Y));

      if (_battlePlan.Count == 0)
        ReceiveOrders(OrderType.Retire);
    }

    public void ReadyForBattle()
    {
      SurveyBattleField();

      HuntingVector = new Movement();
      HuntingVector.Direction = Direction.East;
      HuntingVector.Speed = 2;
      Order = OrderType.Hunt;
    }

    #endregion

    private void AcquireTarget()
    {
      HuntingVector.Coordinate = _battlePlan[0];

      while (_state == MinionState.Acquiring)
        TrackEnemy();

      Target = HuntingVector.Coordinate;
    }

    private void SmiteEnemy()
    {
      if (_state == MinionState.TargetLost)
      {
        _trackingVector.Regroup();
        _state = MinionState.TargetAcquired;
      }

      while (_state == MinionState.TargetAcquired)
        PrepareAttack();

      Target = _trackingVector.Coordinate;
      _state = MinionState.TargetAcquired;
    }

    private void CheckPosition(Movement vector)
    {
      if (vector.Coordinate.X > Battlefield.Boundaries.East)
      {
        vector.Coordinate.X = Battlefield.Boundaries.East;
        vector.Turn();
      }

      if (vector.Coordinate.X < Battlefield.Boundaries.West)
      {
        vector.Coordinate.X = Battlefield.Boundaries.West;
        vector.Turn();
      }

      if (vector.Coordinate.Y > Battlefield.Boundaries.North)
      {
        vector.Coordinate.Y = Battlefield.Boundaries.North;
        vector.Turn();
      }

      if (vector.Coordinate.Y < Battlefield.Boundaries.South)
      {
        vector.Coordinate.Y = Battlefield.Boundaries.South;
        vector.Turn();
      }
    }

    private void TrackEnemy()
    {
      if (Battlefield.IsOutside(HuntingVector.Coordinate))
      {
        CheckPosition(HuntingVector);
        HuntingVector.Turn();
      }

      if (Battlefield.StatusAt(HuntingVector.Coordinate) == Status.Explored || Battlefield.StatusAt(HuntingVector.Coordinate) == Status.Defiled)
        HuntingVector.Advance();
      else
        _state = MinionState.TargetAcquired;
    }

    private void PrepareAttack()
    {
      if (Battlefield.IsOutside(_trackingVector.Coordinate))
        _trackingVector.Regroup();

      if (Battlefield.StatusAt(_trackingVector.Coordinate) == Status.Explored || Battlefield.StatusAt(_trackingVector.Coordinate) == Status.Damaged || Battlefield.StatusAt(_trackingVector.Coordinate) == Status.Defiled)
        _trackingVector.Advance();
      else
        _state = MinionState.Attacking;
    }

    private void SurveyBattleField()
    {
      for (int latitude = Battlefield.Boundaries.West; latitude <= Battlefield.Boundaries.East; latitude++)
        for (int longitude = Battlefield.Boundaries.South; longitude <= Battlefield.Boundaries.North; longitude++)
          _battlePlan.Add(new Coordinate(latitude, longitude));
    }
  }
}
