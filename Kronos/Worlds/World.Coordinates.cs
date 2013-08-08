using System;
using System.Collections.Generic;

using Kronos.Worlds.Directions;
using System.Collections.ObjectModel;

namespace Kronos.Worlds
{
    public class Coordinate : IEqualityComparer<Coordinate>, IEquatable<Coordinate>
    {
        public int Latitude { get; set; }
        public int Longitude { get; set; }

        public Coordinate() { }
        public Coordinate(int latitude, int longitude) { Latitude = latitude; Longitude = longitude; }
        public Coordinate(Coordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");

            Latitude = coordinate.Latitude;
            Longitude = coordinate.Longitude;
        }

        public Coordinate Add(Coordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");

            return new Coordinate(Latitude + coordinate.Latitude, Longitude + coordinate.Longitude);
        }

        public void Move(Coordinate coordinate)
        {
            if (coordinate == null)
                throw new ArgumentNullException("coordinate");

            Latitude += coordinate.Latitude;
            Longitude += coordinate.Longitude;
        }

        public static Compass GetDirection(Coordinate first, Coordinate second)
        {
            Compass direction = new Compass();
            Coordinate heading = first - second;

            if (heading.Latitude == 0)
                direction = heading.Longitude < 0 ? Compass.North : Compass.South;
            if (heading.Longitude == 0)
                direction = heading.Latitude < 0 ? Compass.East : Compass.West;

            return direction;
        }

        public static Coordinate GetHeading(Compass direction)
        {
            Coordinate coordinate = new Coordinate();

            switch (direction)
            {
                case Compass.North:
                    coordinate.Latitude = 0;
                    coordinate.Longitude = 1;
                    break;
                case Compass.East:
                    coordinate.Latitude = 1;
                    coordinate.Longitude = 0;
                    break;
                case Compass.South:
                    coordinate.Latitude = 0;
                    coordinate.Longitude = -1;
                    break;
                case Compass.West:
                    coordinate.Latitude = -1;
                    coordinate.Longitude = 0;
                    break;
            }

            return coordinate;
        }

        public ReadOnlyCollection<Coordinate> Neighbors()
        {
            List<Coordinate> neighbors = new List<Coordinate>();

            neighbors.Add(this.Add(GetHeading(Compass.North)));
            neighbors.Add(this.Add(GetHeading(Compass.East)));
            neighbors.Add(this.Add(GetHeading(Compass.South)));
            neighbors.Add(this.Add(GetHeading(Compass.West)));

            return neighbors.AsReadOnly();
        }

        public BattleShip.Interface.Coordinate ToInterfaceCoordinate()
        {
            return new BattleShip.Interface.Coordinate(Latitude, Longitude);
        }

        public static Coordinate operator +(Coordinate first, Coordinate second)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            if (second == null)
                throw new ArgumentNullException("second");

            return new Coordinate(first.Latitude + second.Latitude, first.Longitude + second.Longitude);
        }

        public static Coordinate operator -(Coordinate first, Coordinate second)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            if (second == null)
                throw new ArgumentNullException("second");

            return new Coordinate(first.Latitude - second.Latitude, first.Longitude - second.Longitude);
        }

        public static bool operator ==(Coordinate left, Coordinate right)
        {
            if (System.Object.ReferenceEquals(left, right))
                return true;

            if ((object)left == null || (object)right == null)
                return false;

            return left.Latitude == right.Latitude && left.Longitude == right.Longitude;
        }

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            if (left == null || right == null)
                return false;

            return left.Latitude != right.Latitude || left.Longitude != right.Longitude;
        }

        #region IEqualityComparer<Coordinate> Members

        public bool Equals(Coordinate x, Coordinate y)
        {
            if (x == null || y == null)
                return false;

            return x.Latitude == y.Latitude && x.Longitude == y.Longitude;
        }

        public int GetHashCode(Coordinate obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return obj.Latitude ^ obj.Longitude;
        }

        #endregion

        #region IEquatable<Coordinate> Members

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Coordinate)obj);
        }

        public bool Equals(Coordinate other)
        {
            if (other == null)
                return false;

            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            return Latitude ^ Longitude;
        }

        #endregion
    }
}
