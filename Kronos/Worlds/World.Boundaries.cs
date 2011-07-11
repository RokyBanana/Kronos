using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kronos.Worlds
{
  public class Boundaries
  {
    public int North { get; set; }
    public int South { get; set; }
    public int East { get; set; }
    public int West { get; set; }

    public Boundaries() { North = 10; South = 1; East = 10; West = 1; }
  }
}
