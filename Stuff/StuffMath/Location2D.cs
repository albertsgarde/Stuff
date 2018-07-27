using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public struct Location2D
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public Location2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Location2D(Location2D loc)
        {
            X = loc.X;
            Y = loc.Y;
        }

        public Location2D(Vector2D v)
        {
            X = v.X;
            Y = v.Y;
        }

        public static Location2D operator +(Location2D loc, Vector2D vec)
        {
            return new Location2D(loc.X + vec.X, loc.Y + vec.Y);
        }

        public static Location2D operator -(Location2D loc, Vector2D vec)
        {
            return new Location2D(loc.X - vec.X, loc.Y - vec.Y);
        }

        //add this code to class ThreeDPoint as defined previously
        //
        public static bool operator ==(Location2D a, Location2D b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Location2D a, Location2D b)
        {
            return !(a == b);
        }

        public double DistanceToSquared(Location2D loc)
        {
            return Math.Pow(loc.X - X, 2) + Math.Pow(loc.Y - Y, 2);
        }

        public double DistanceTo(Location2D loc)
        {
            return Math.Sqrt(DistanceToSquared(loc));
        }

        public Location2D Midpoint(Location2D loc)
        {
            return new Location2D((loc.X + X) / 2, (loc.Y + Y) / 2);
        }

        /// <summary>
        /// BEVISLIGNENDE TING:
        /// Lav en ortogonal vector til linien.
        /// Lav en vektor fra et vilkårligt punkt på linien til punktet og projekter den på den ortogonale vektor.
        /// </summary>
        /// <param name="lf"></param>
        /// <returns></returns>
        public Vector2D VectorToLine(LinearFunction lf)
        {
            return new Vector2D(lf.PointOnLine(), this).Project(lf.OrthogonalVector());
        }

        public double DistanceTo(LinearFunction lf)
        {
            double valueAtCoord = lf.A * X + lf.B * Y + lf.C;
            valueAtCoord = Math.Abs(valueAtCoord);
            return valueAtCoord / (Math.Sqrt(lf.A * lf.A + lf.B * lf.B));
        }

        public Location2D ProjectToLine(LinearFunction lf)
        {
            return lf.PointX(0) + lf.DirectionVector().Project((new Vector2D(this, lf.PointX(0))));
        }

        public Location2D ProjectToLine(LinearParameter lp)
        {
            return lp.Point(0) + lp.DirectionVector().Project((new Vector2D(this, lp.Point(0))));
        }

        public Vector2D LocationVector()
        {
            return new Vector2D(X, Y);
        }

        public static implicit operator Vector2D(Location2D loc)
        {
            return loc.LocationVector();
        }

        public static explicit operator Location2F(Location2D loc)
        {
            return new Location2F((float)loc.X, (float)loc.Y);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public override bool Equals(object loc)
        {
            if (loc is Location2D)
                return ((Location2D)loc) == this;
            else
                return false;
        }
    }
}