using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BattleShip.Interface;

namespace Kronos.Minions
{
  public abstract class Minion
  {
    public abstract string Name { get; set; }
    public Action Status { get; set; }

    private List<Coordinate> _list;
  }

  public enum Action
  {
    Hunting,
    Tracking
  }
}
