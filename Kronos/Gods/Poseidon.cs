using System.Collections.Generic;
using System.Linq;

using Kronos.Minions;
using Kronos.Worlds;
using Kronos.Worlds.Maps;
using Kronos.Worlds.Directions;

namespace Kronos.Gods
{
  public class Poseidon : God
  {
    public override string Name { get { return "Poseidon"; } }
    public override World World { get; set; }
    public override Minion Minion { get { return _minion; } }

    private List<Coordinate> _killHistory = new List<Coordinate>();
    private Minion _minion;
    private Observer _observer;

    public Poseidon() { }

    public override void Contemplate(int casualties, int defiles)
    {
      Coordinate target = new Coordinate(Minion.Target);

      if (casualties == 0)
        Minion.CoverTracks(new Position(target, Status.Explored));

      if (casualties > 0)
      {
        Minion.CoverTracks(new Position(target, Status.Damaged));

        if (Minion.Order == OrderType.Hunt)
          Minion.ReceiveOrders(OrderType.Kill);

        _killHistory.Add(target);
      }

      if (defiles > 0)
      {
        World.Enemies.Remove(World.Enemies.First(e => e.Length == Minion.EnemySize));

        Minion.ReceiveOrders(OrderType.Hunt);

        UseDivinePower();
      }

      if (Minion.Order == OrderType.Hunt)
        EliminatePossibilities();

#if DEBUG
      if (_observer != null)
        _observer.Show();
#endif
    }

    public override void Play()
    {
      _minion = new Minion();
      _minion.Battlefield = World.Map;
      _minion.ReadyForBattle();
      _minion.ReceiveOrders(OrderType.Hunt);

      //_observer = new Observer();
      //_observer.Map = World.Map;
      //_observer.ShowBattlefield = false;
      //_observer.ShowEachTurn = false;
      //_observer.ShowNeighbors = false;
    }

    public override void Smite()
    {
      _minion.ObeyOrder();
    }

    private void EliminatePossibilities()
    {
      int smallestEnemy = World.Enemies.Min(e => e.Length);
      List<Coordinate> possibilities = Minion.Battlefield.GetAll(Status.Hidden).ToList();

      EliminateSingles(possibilities);

      if (smallestEnemy > 2)
      {
        possibilities = Minion.Battlefield.GetAll(Status.Hidden).ToList();

        for (int start = World.Boundaries.South; start <= World.Boundaries.North; start++)
          EliminateLatitudes(start, smallestEnemy);

        for (int start = World.Boundaries.West; start <= World.Boundaries.East; start++)
          EliminateLongitudes(start, smallestEnemy);
      }
    }

    private void EliminateSingles(List<Coordinate> possibilities)
    {
      Coordinate possibility;

      while (possibilities.Count > 0)
      {
        possibility = possibilities.ElementAt(0);
        possibilities.Remove(possibility);

        if (possibility.Neighbors().Any(c => Minion.Battlefield.StatusAt(c) == Status.Hidden))
          continue;

        Minion.Battlefield.Update(possibility, Status.Ignored);
        Minion.RemoveBattlePlanItem(possibility);
      }
    }

    private void EliminateLatitudes(int longitude, int smallestEnemy)
    {
      Coordinate test;
      List<Coordinate> candidates = new List<Coordinate>();
      List<Coordinate> ignoreList = new List<Coordinate>();

      test = new Coordinate(World.Boundaries.West, longitude);

      while (test.Latitude <= World.Boundaries.East)
      {
        if (Minion.Battlefield.StatusAt(test) != Status.Hidden)
        {
          if (candidates.Count > 1 && candidates.Count < smallestEnemy)
          {
            if (!candidates.Any(c => Minion.Battlefield.StatusAt(c.Add(Coordinate.GetHeading(Compass.North))) == Status.Hidden || Minion.Battlefield.StatusAt(c.Add(Coordinate.GetHeading(Compass.South))) == Status.Hidden))
              ignoreList.AddRange(candidates);

            candidates.Clear();
          }
        }
        else
          candidates.Add(new Coordinate(test.Latitude, test.Longitude));

        test.Move(Coordinate.GetHeading(Compass.East));
      }

      if (ignoreList.Count > 0)
      {
        ignoreList.ForEach(c => Minion.Battlefield.Update(c, Status.Ignored));
        ignoreList.ForEach(c => Minion.RemoveBattlePlanItem(c));
      }
    }

    private void EliminateLongitudes(int latitude, int smallestEnemy)
    {
      Coordinate test;
      List<Coordinate> candidates = new List<Coordinate>();
      List<Coordinate> ignoreList = new List<Coordinate>();

      test = new Coordinate(latitude, World.Boundaries.South);

      while (test.Longitude <= World.Boundaries.North)
      {
        if (Minion.Battlefield.StatusAt(test) != Status.Hidden)
        {
          if (candidates.Count > 1 && candidates.Count < smallestEnemy)
          {
            if (!candidates.Any(c => Minion.Battlefield.StatusAt(c.Add(Coordinate.GetHeading(Compass.East))) == Status.Hidden || Minion.Battlefield.StatusAt(c.Add(Coordinate.GetHeading(Compass.West))) == Status.Hidden))
              ignoreList.AddRange(candidates);

            candidates.Clear();
          }
        }
        else
          candidates.Add(new Coordinate(test.Latitude, test.Longitude));

        test.Move(Coordinate.GetHeading(Compass.North));
      }

      if (ignoreList.Count > 0)
      {
        ignoreList.ForEach(c => Minion.Battlefield.Update(c, Status.Ignored));
        ignoreList.ForEach(c => Minion.RemoveBattlePlanItem(c));
      }
    }

    private void UseDivinePower()
    {
      Coordinate ignore;

      _killHistory = _killHistory.OrderBy(cX => cX.Latitude).ThenBy(cY => cY.Longitude).ToList();

      foreach (Coordinate coordinate in _killHistory)
        _minion.Battlefield.Update(coordinate, Status.Destroyed);

      foreach (Coordinate coordinate in _killHistory)
      {
        for (int latitude = -1; latitude <= 1; latitude++)
          for (int longitude = -1; longitude <= 1; longitude++)
          {
            ignore = new Coordinate(coordinate.Latitude + latitude, coordinate.Longitude + longitude);

            if (World.Map.IsOutside(ignore))
              continue;

            if (Minion.Battlefield.StatusAt(ignore) != Status.Hidden)
              continue;

            Minion.RemoveBattlePlanItem(ignore);
            Minion.Battlefield.Update(ignore, Status.Ignored);
          }
      }

      _killHistory.Clear();
    }
  }
}
