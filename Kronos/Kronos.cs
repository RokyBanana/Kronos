using System;
using System.Text;
using System.Collections.Generic;

using BattleShip.Interface;

namespace Kronos
{
  public class Poseidon : IPlayer
  {
    #region Fields

    private int              _xMax;
    private int              _yMax;
    private int              _shot;
    private List<Coordinate> _pathOfPoseidon;
    private List<World>       _zoneStatus;
    private static Random    _rand;
    private Triton           _minion;
    private Viewer           _viewer;

    #endregion

    #region Properties

    internal int XMax { get { return _xMax; } }
    internal int YMax { get { return _yMax; } }

    public bool MinionIsHunting { get { return _minion != null; } }
    public Coordinate RandomCoordinate { get { return new Coordinate( _rand.Next( _xMax ) + 1, _rand.Next( _yMax ) + 1 ); } }
    public List<World> ZoneStatus { get { return _zoneStatus; } }
    public string Name { get { return "Poseidon"; } }
    public int Shots { get { return _shot; } }

    #endregion

    #region Interface implementation

    public void PlaceShips( IPlayerView playerView, System.Collections.Generic.ICollection<IVessel> ships )
    {
      Coordinate  coordinate;
      Orientation orientation = Orientation.Horizontal;

      PoseidonWakesUp( playerView );

      coordinate = RandomCoordinate;

      foreach ( IVessel ship in ships )
      {
        orientation = _rand.Next( 2 ) == 1 ? Orientation.Horizontal : Orientation.Vertical;

        if ( orientation == Orientation.Horizontal && coordinate.X + ship.Length > _xMax )
          coordinate.X -= coordinate.X + ship.Length - _xMax;

        if ( orientation == Orientation.Vertical && coordinate.Y - ship.Length < 1 )
          coordinate.Y = ship.Length;

        int _try = 0;

        while ( !playerView.PutShip( ship.SailTo( coordinate, orientation ) ) )
        {
          if ( ++coordinate.X > _xMax )
            coordinate.X = 1;

          if ( --coordinate.Y > 1 )
            coordinate.Y = _yMax;

          if ( ++_try > 10 )
          {
            _try = 0;
            coordinate.X = _rand.Next( _xMax ) + 1;
            coordinate.Y = _rand.Next( _yMax ) + 1;
          }
        }
      }
    }

    public void ShotFeedback( int hits, int sunkShips )
    {
      if ( sunkShips > 0 )
        MinionReturnsWithPrey();
      else if ( MinionIsHunting )
        _minion.TrackPrey( hits );
      else if ( hits > 0 )
        SendMinion();

      _viewer.Show();
    }

    public Shot YourTurn( IPlayerView playerView )
    {
      Coordinate coordinate;

      _shot++;

      if ( MinionIsHunting )
        coordinate = _minion.HuntingAt();
      else
        coordinate = PoseidonLooksAt();

      return new Shot( coordinate.X, coordinate.Y );
    }

    #endregion

    #region Methods

    private void SendMinion()
    {
      _minion = new Triton( this );
    }

    private void MinionReturnsWithPrey()
    {

    }

    private Coordinate PoseidonLooksAt()
    {
      Coordinate coordinate;
      World       zone;

      if ( _pathOfPoseidon.Count == 0 )
        coordinate = RandomCoordinate;
      else
        coordinate = _pathOfPoseidon[ 0 ];

      zone = new World( coordinate );
      zone.State = State.Visited;

      _pathOfPoseidon.Remove( zone.Coordinate );
      _zoneStatus.Add( zone );

      return zone.Coordinate;
    }

    private void PoseidonWakesUp( IPlayerView playerView )
    {
      _xMax = playerView.GetXMax();
      _yMax = playerView.GetYMax();
      _viewer = new Viewer( this );
      _rand = new Random( Environment.TickCount );

      CreatePath();

      _zoneStatus = new List<World>();
    }

    private void CreatePath()
    {
      _pathOfPoseidon = new List<Coordinate>();

      _pathOfPoseidon.Add( new Coordinate( 1, 1 ) );
      _pathOfPoseidon.Add( new Coordinate( 1, 3 ) );
      _pathOfPoseidon.Add( new Coordinate( 1, 5 ) );
      _pathOfPoseidon.Add( new Coordinate( 1, 7 ) );
      _pathOfPoseidon.Add( new Coordinate( 1, 9 ) );
      _pathOfPoseidon.Add( new Coordinate( 10, 1 ) );
      _pathOfPoseidon.Add( new Coordinate( 10, 3 ) );
      _pathOfPoseidon.Add( new Coordinate( 10, 5 ) );
      _pathOfPoseidon.Add( new Coordinate( 10, 7 ) );
      _pathOfPoseidon.Add( new Coordinate( 10, 9 ) );
      _pathOfPoseidon.Add( new Coordinate( 2, 1 ) );
      _pathOfPoseidon.Add( new Coordinate( 2, 3 ) );
      _pathOfPoseidon.Add( new Coordinate( 2, 5 ) );
      _pathOfPoseidon.Add( new Coordinate( 2, 7 ) );
      _pathOfPoseidon.Add( new Coordinate( 2, 9 ) );
      _pathOfPoseidon.Add( new Coordinate( 9, 1 ) );
      _pathOfPoseidon.Add( new Coordinate( 9, 3 ) );
      _pathOfPoseidon.Add( new Coordinate( 9, 5 ) );
      _pathOfPoseidon.Add( new Coordinate( 9, 7 ) );
      _pathOfPoseidon.Add( new Coordinate( 9, 9 ) );
      _pathOfPoseidon.Add( new Coordinate( 3, 1 ) );
      _pathOfPoseidon.Add( new Coordinate( 3, 3 ) );
      _pathOfPoseidon.Add( new Coordinate( 3, 5 ) );
      _pathOfPoseidon.Add( new Coordinate( 3, 7 ) );
      _pathOfPoseidon.Add( new Coordinate( 3, 9 ) );
      _pathOfPoseidon.Add( new Coordinate( 8, 1 ) );
      _pathOfPoseidon.Add( new Coordinate( 8, 3 ) );
      _pathOfPoseidon.Add( new Coordinate( 8, 5 ) );
      _pathOfPoseidon.Add( new Coordinate( 8, 7 ) );
      _pathOfPoseidon.Add( new Coordinate( 8, 9 ) );
      _pathOfPoseidon.Add( new Coordinate( 4, 1 ) );
      _pathOfPoseidon.Add( new Coordinate( 4, 3 ) );
      _pathOfPoseidon.Add( new Coordinate( 4, 5 ) );
      _pathOfPoseidon.Add( new Coordinate( 4, 7 ) );
      _pathOfPoseidon.Add( new Coordinate( 4, 9 ) );
      _pathOfPoseidon.Add( new Coordinate( 7, 1 ) );
      _pathOfPoseidon.Add( new Coordinate( 7, 3 ) );
      _pathOfPoseidon.Add( new Coordinate( 7, 5 ) );
      _pathOfPoseidon.Add( new Coordinate( 7, 7 ) );
      _pathOfPoseidon.Add( new Coordinate( 7, 9 ) );
      _pathOfPoseidon.Add( new Coordinate( 5, 1 ) );
      _pathOfPoseidon.Add( new Coordinate( 5, 3 ) );
      _pathOfPoseidon.Add( new Coordinate( 5, 5 ) );
      _pathOfPoseidon.Add( new Coordinate( 5, 7 ) );
      _pathOfPoseidon.Add( new Coordinate( 5, 9 ) );
      _pathOfPoseidon.Add( new Coordinate( 6, 1 ) );
      _pathOfPoseidon.Add( new Coordinate( 6, 3 ) );
      _pathOfPoseidon.Add( new Coordinate( 6, 5 ) );
      _pathOfPoseidon.Add( new Coordinate( 6, 7 ) );
      _pathOfPoseidon.Add( new Coordinate( 6, 9 ) );
    }

    #endregion
  }
}
