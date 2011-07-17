namespace Kronos.Worlds
{
  public class Boundaries
  {
    public int North { get; set; }
    public int South { get; set; }
    public int East { get; set; }
    public int West { get; set; }

    public Boundaries() : this(10, 1, 10, 1) { }
    public Boundaries(int north, int south, int east, int west) { North = north; South = south; East = east; West = west; }
  }
}
