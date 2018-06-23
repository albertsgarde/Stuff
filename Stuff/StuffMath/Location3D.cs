using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Location3D
    {
        public double X { get; private set; }

        public double Y { get; private set; }

        public double Z { get; private set; }

        public Location3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Location3D(Vector3D vec)
        {
            X = vec.X;
            Y = vec.Y;
            Z = vec.Z;
        }

        public double DistanceToSquared(Location3D loc)
        {
            return Math.Pow(loc.X - X, 2) + Math.Pow(loc.Y - Y, 2) + Math.Pow(loc.Z - Z, 2);
        }

        public double DistanceTo(Location3D loc)
        {
            return Math.Sqrt(DistanceToSquared(loc));
        }

        public double DistanceToSquared(Plane p)
        {
            return Math.Pow(X * p.A + Y * p.B + Z * p.C + p.D, 2) / (p.A * p.A + p.B * p.B + p.C * p.C);
        }

        public double DistanceTo(Plane p)
        {
            return Math.Abs(X * p.A + Y * p.B + Z * p.C+p.D) / Math.Sqrt(p.A * p.A + p.B * p.B + p.C * p.C);
        }

        public Vector3D VectorToPlane(Plane p)
        {
            return new Vector3D(this, p.ZLocation(0, 0)).Project(p.OrthogonalVector);
        }

        public double AreaOfTriangle(Location3D loc1, Location3D loc2)
        {
            return new Vector3D(this, loc1).CrossProduct(new Vector3D(this, loc2)).Length/2;
        }

        public Vector3D LocationVector()
        {
            return new Vector3D(X, Y, Z);
        }

        public static implicit operator Vector3D(Location3D loc)
        {
            return loc.LocationVector();
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }
    }
}
