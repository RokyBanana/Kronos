using System.Collections.Generic;

using BattleShip.Interface;

using Kronos.Worlds;
using Kronos.Worlds.Maps;
using Kronos.Worlds.Directions;

namespace Kronos.Minions
{
  public class Minion
  {
    public Coordinate Target { get; set; }
    public List<Coordinate> BattlePlan { get; set; }
    public Map BattleField { get; set; }
    public Movement HuntingVector { get; set; }
    public Orders Order { get; set; }
    public string Name { get; set; }

    private List<Coordinate> _footPrints;
    private List<Position> _mayhem;
    private Movement _trackingVector;
    private States _state;

    public Minion()
    {
      Name = "Triton";

      _footPrints = new List<Coordinate>();
      _state = States.Idle;
    }

    #region Methods

    public void CarryOutHisWill()
    {
      if (Order == Orders.Hunt)
        AcquireTarget();

      if (Order == Orders.Kill)
        SmiteEnemy();
    }

    public void RecieveOrders(Orders order)
    {
      Order = order;

      switch (order)
      {
        case Orders.Hunt:
          break;
        case Orders.Kill:
          Position position = new Position(Target, Status.Damaged);

          _mayhem = new List<Position>();
          _mayhem.Add(position);

          _trackingVector = new Movement(position.Coordinate, Direction.East, 1);
          _trackingVector.StartPosition = position.Coordinate;
          break;
        case Orders.Retire:
          break;
      }
    }

    public void CoverTracks(Position position)
    {
      if (Order == Orders.Kill)
      {
        _mayhem.Add(position);

        if (position.Status == Status.Explored)
          _state = States.TargetLost;
      }

      _footPrints.Add(position.Coordinate);

      BattleField.Update(position);
      BattlePlan.Remove(BattlePlan.Find(c => c.X == position.Coordinate.X && c.Y == position.Coordinate.Y));

      if (BattlePlan.Count == 0)
        RecieveOrders(Orders.Retire);
    }

    public void Debrief(Coordinate target)
    {
    }

    public void ReadyForBattle()
    {
      SurveyBattleField();

      HuntingVector = new Movement();
      HuntingVector.Direction = Direction.East;
      HuntingVector.Speed = 2;
      Order = Orders.Hunt;
    }

    #endregion

    private Coordinate LastFootPrint()
    {
      return _footPrints[_footPrints.Count - 1];
    }

    private void AcquireTarget()
    {
      HuntingVector.Coordinate = BattlePlan[0];

      while (_state == States.Idle)
        TrackEnemy();

      Target = HuntingVector.Coordinate;
    }

    private void SmiteEnemy()
    {
      if (_state == States.TargetLost)
      {
        _trackingVector.ReGroup();
        _state = States.TargetAcquired;
      }

      while (_state == States.TargetAcquired)
        PrepareAttack();

      Target = _trackingVector.Coordinate;
      _state = States.TargetAcquired;
    }

    private void CheckPosition(Movement vector)
    {
      if (vector.Coordinate.X > BattleField.Boundaries.East)
      {
        vector.Coordinate.X = BattleField.Boundaries.East;
        vector.Turn();
      }

      if (vector.Coordinate.X < BattleField.Boundaries.West)
      {
        vector.Coordinate.X = BattleField.Boundaries.West;
        vector.Turn();
      }

      if (vector.Coordinate.Y > BattleField.Boundaries.North)
      {
        vector.Coordinate.Y = BattleField.Boundaries.North;
        vector.Turn();
      }

      if (vector.Coordinate.Y < BattleField.Boundaries.South)
      {
        vector.Coordinate.Y = BattleField.Boundaries.South;
        vector.Turn();
      }
    }

    private void TrackEnemy()
    {
      if (BattleField.IsOutside(HuntingVector.Coordinate))
      {
        CheckPosition(HuntingVector);
        HuntingVector.Turn();
      }

      if (BattleField.StatusAt(HuntingVector.Coordinate) == Status.Explored || BattleField.StatusAt(HuntingVector.Coordinate) == Status.Defiled)
        HuntingVector.Advance();
      else
        _state = States.TargetAcquired;
    }

    private void PrepareAttack()
    {
      if (BattleField.IsOutside(_trackingVector.Coordinate))
        _trackingVector.ReGroup();

      if (BattleField.StatusAt(_trackingVector.Coordinate) == Status.Explored || BattleField.StatusAt(_trackingVector.Coordinate) == Status.Damaged || BattleField.StatusAt(_trackingVector.Coordinate) == Status.Defiled)
        _trackingVector.Advance();
      else
        _state = States.Attacking;
    }

    private void SurveyBattleField()
    {
      BattlePlan = new List<Coordinate>();

      for (int latitude = BattleField.Boundaries.West; latitude <= BattleField.Boundaries.East; latitude++)
        for (int longitude = BattleField.Boundaries.South; longitude <= BattleField.Boundaries.North; longitude++)
          BattlePlan.Add(new Coordinate(latitude, longitude));
    }
  }
}
