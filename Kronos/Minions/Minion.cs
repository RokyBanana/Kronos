using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kronos.Minions
{
  public abstract class Minion
  {
    public abstract string Name { get; set; }
  }

  public enum Status
  {
    Hunting,
    Tracking
  }
}
