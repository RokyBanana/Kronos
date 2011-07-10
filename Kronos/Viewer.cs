using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleShip.Interface;

namespace Kronos
{
  public class Viewer
  {
    private Poseidon _poseidon;

    public Viewer( Poseidon father )
    {
      _poseidon = father;
    }

    public void Show()
    {
      Console.Clear();

      string        _mark = "";
      StringBuilder viewArea = new StringBuilder();
      World          zone;
      State         state;
      Coordinate    lastShot;

      for ( int _y = _poseidon.YMax; _y >= 1; _y-- )
      {
        viewArea.Append( "    " );

        if ( _y == _poseidon.YMax )
          for ( int _x = 1; _x <= _poseidon.XMax; _x++ )
            viewArea.Append( _x ).Append( " " );

        viewArea.AppendLine();
        viewArea.Append( _y.ToString().PadLeft( 2 ) ).Append( ": " );

        for ( int _x = 1; _x <= _poseidon.XMax; _x++ )
        {
          zone = _poseidon.ZoneStatus.Find( z => z.Coordinate.X == _x && z.Coordinate.Y == _y );

          if ( zone == null )
            state = State.Hidden;
          else
            state = zone.State;

          switch ( state )
          {
            case State.Hidden    : _mark = ". "; break;
            case State.Visited   : _mark = "o "; break;
            case State.Destroyed : _mark = "X "; break;
          }

          viewArea.Append( _mark );
        }
          
        viewArea.AppendLine();
        viewArea.AppendLine();
      }

      lastShot = _poseidon.ZoneStatus.Last().Coordinate;

      viewArea.Append( "Last shot: " ).Append( lastShot.X ).Append( "," ).Append( lastShot.Y ).AppendLine();
      viewArea.AppendLine( string.Concat( "Shots fired: ", _poseidon.Shots ) );

      Console.Write( viewArea.ToString() );
      Console.ReadKey();
    }
  }
}
