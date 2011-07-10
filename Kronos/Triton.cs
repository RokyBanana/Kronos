using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleShip.Interface;

namespace Kronos
{
  public class Triton
  {
    #region Fields

    private List<Coordinate> _pathOfMinion;
    private Poseidon         _poseidon;

    #endregion

    #region Properties

    #endregion

    public Triton( Poseidon father )
    {
      _poseidon = father;
    }

    #region Methods

    internal void TrackPrey( int hits )
    {
      if ( hits > 0 )
        _poseidon.ZoneStatus.Last().State = State.Destroyed;
    }

    public Coordinate HuntingAt()
    {
      Coordinate coordinate;

      coordinate = _poseidon.ZoneStatus.Last().Coordinate;
      coordinate.X++;

      if ( coordinate.X > _poseidon.XMax )
      {
        coordinate.X = 1;
        coordinate.Y++;
      }

      if ( coordinate.Y > _poseidon.YMax )
        coordinate.Y = 1;

      _poseidon.ZoneStatus.Add( new World( coordinate, State.Visited ) );

      return coordinate;
    }

    #endregion
  }
}
