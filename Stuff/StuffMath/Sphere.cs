using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    /// <summary>
    /// (x-a)^2+(y-b)^2+(z-c)^2=r^2
    /// (a,b,c) is the sphere's centre. r is the sphere's radius.
    /// </summary>
    public class Sphere
    {
        public double A { get; private set; }

        public double B { get; private set; }

        public double C { get; private set; }

        public double R { get; private set; }

        public Sphere(Location3D centre, double radius)
        {
            A = centre.X;
            B = centre.Y;
            C = centre.Z;
            R = radius;
        }

        public Sphere(Location3D centre, Location3D point)
        {
            A = centre.X;
            B = centre.Y;
            C = centre.Z;
            R = centre.DistanceTo(point);
        }

        public Sphere(Location3D centre, Plane tangent)
        {
            A = centre.X;
            B = centre.Y;
            C = centre.Z;
            R = centre.DistanceTo(tangent);
        }

        public Sphere(double a, double b, double c, double radius)
        {
            A = a;
            B = b;
            C = c;
            R = radius;
        }

        public Location3D Centre
        {
            get
            {
                return new Location3D(A, B, C);
            }
        }

        public static Sphere operator *(Sphere c, double d)
        {
            return new Sphere(c.A, c.B, c.C, c.R * d);
        }

        public static Sphere operator /(Sphere c, double d)
        {
            return new Sphere(c.A, c.B, c.C, c.R / d);
        }

        public bool PointIsPartOf(Location3D loc)
        {
            return Math.Pow(loc.X - A, 2) + Math.Pow(loc.Y - B, 2) + Math.Pow(loc.Z - C, 2) == Math.Pow(R, 2);
        }

        public Plane Tangent(Location3D loc)
        {
            if (PointIsPartOf(loc))
            {
                Vector3D vecPC = new Vector3D(loc, new Location3D(A, B, C));
                return new Plane(loc, vecPC);
            }
            else
                throw new Exception("The point is not part of the sphere");
        }

        public bool IsTangent(Plane plane)
        {
            return Centre.DistanceToSquared(plane) == R * R;
        }

        public Location3D[] Intersections(LinearParamater3D lp)
        {
            double a = Math.Pow(lp.A.X, 2) + Math.Pow(lp.A.Y, 2) + Math.Pow(lp.A.Z, 2);
            double b = -2 * (A * lp.A.X + B * lp.A.Y + C * lp.A.Z - lp.B.X * lp.A.X - lp.B.Y * lp.A.Y - lp.B.Z * lp.A.Z);
            double c = -Math.Pow(R, 2) + Math.Pow(A, 2) - 2 * A * lp.B.X + Math.Pow(B, 2) - 2 * B * lp.B.Y + Math.Pow(C, 2) - 2 * C * lp.B.Z + Math.Pow(lp.B.X, 2) + Math.Pow(lp.B.Y, 2) + Math.Pow(lp.B.Z, 2);
            QuadraticFunction qf = new QuadraticFunction(a, b, c);
            return qf.Roots().Select(d => lp.Point(d)).ToArray();
        }

        public override string ToString()
        {
            return "(x " + (A < 0 ? "+ " + -A : "- " + A) + ")^2 + (y " + (B < 0 ? "+ " + -B : "- " + B) + ")^2 + (z " + (C < 0 ? "+ " + -C : "- " + C) + ")^2 = " + R + "^2";
        }
    }
}
