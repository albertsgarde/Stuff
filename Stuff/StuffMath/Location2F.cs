using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Location2F
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public Location2F(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Location2F(Location2F loc)
        {
            X = loc.X;
            Y = loc.Y;
        }

        public Location2F(Vector2F v)
        {
            X = v.X;
            Y = v.Y;
        }

        public static Location2F operator +(Location2F loc, Vector2F vec)
        {
            return new Location2F(loc.X + vec.X, loc.Y + vec.Y);
        }

        public static Location2F operator -(Location2F loc, Vector2F vec)
        {
            return new Location2F(loc.X - vec.X, loc.Y - vec.Y);
        }

        //add this code to class ThreeDPoint as defined previously
        //
        public static bool operator ==(Location2F a, Location2F b)
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

        public static bool operator !=(Location2F a, Location2F b)
        {
            return !(a == b);
        }

        public float DistanceToSquared(Location2F loc)
        {
            return (float)Math.Pow(loc.X - X, 2) + (float)Math.Pow(loc.Y - Y, 2);
        }

        public float DistanceTo(Location2F loc)
        {
            return (float)Math.Sqrt(DistanceToSquared(loc));
        }

        public Location2F Midpoint(Location2F loc)
        {
            return new Location2F((loc.X + X) / 2, (loc.Y + Y) / 2);
        }

        /// <summary>
        /// BEVISLIGNENDE TING:
        /// Lav en ortogonal vector til linien.
        /// Lav en vektor fra et vilkårligt punkt på linien til punktet og projekter den på den ortogonale vektor.
        /// </summary>
        /// <param name="lf"></param>
        /// <returns></returns>
        public Vector2F VectorToLine(LinearFunction lf)
        {
            return new Vector2F((Location2F)lf.PointOnLine(), this).Project((Vector2F)lf.OrthogonalVector());
        }

        public float DistanceTo(LinearFunction lf)
        {
            float valueAtCoord = (float)lf.A * X + (float)lf.B * Y + (float)lf.C;
            valueAtCoord = Math.Abs(valueAtCoord);
            return valueAtCoord / (float)(Math.Sqrt(lf.A * lf.A + lf.B * lf.B));
        }

        public Location2F ProjectToLine(LinearFunction lf)
        {
            return (Location2F)lf.PointX(0) + (Vector2F)lf.DirectionVector().Project((new Vector2F(this, (Location2F)lf.PointX(0))));
        }

        public Location2F ProjectToLine(LinearParameter2D lp)
        {
            return (Location2F)lp.Point(0) + (Vector2F)lp.DirectionVector().Project((new Vector2F(this, (Location2F)lp.Point(0))));
        }

        public Vector2F LocationVector()
        {
            return new Vector2F(X, Y);
        }

        public static implicit operator Vector2F(Location2F loc)
        {
            return loc.LocationVector();
        }

        public static implicit operator Location2D(Location2F loc)
        {
            return new Location2D(loc.X, loc.Y);
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
            if (loc is Location2F)
                return ((Location2F)loc) == this;
            else
                return false;
        }
    }
}