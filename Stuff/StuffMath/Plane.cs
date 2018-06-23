using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Plane
    {
        /// <summary>
        /// ax+by+cz+d=0
        /// </summary>
        public double A { get; private set; }

        /// <summary>
        /// ax+by+cz+d=0
        /// </summary>
        public double B { get; private set; }

        /// <summary>
        /// ax+by+cz+d=0
        /// </summary>
        public double C { get; private set; }

        /// <summary>
        /// ax+by+cz+d=0
        /// </summary>
        public double D { get; private set; }

        public Vector3D OrthogonalVector { get; private set; }

        /// <summary>
        /// ax+by+cz+d=0
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        public Plane(double a, double b, double c, double d = 0)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            OrthogonalVector = new Vector3D(A, B, C);
        }

        /// <summary>
        /// Plane from órthogonal vector and point.
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="loc"></param>
        public Plane(Location3D loc, Vector3D vector)
        {
            OrthogonalVector = vector;
            A = OrthogonalVector.X;
            B = OrthogonalVector.Y;
            C = OrthogonalVector.Z;
            D = -(vector * loc);
        }

        /// <summary>
        /// Plane from two vectors that lie in the plane and a point
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="vec2"></param>
        /// <param name="loc"></param>
        public Plane(Vector3D vec1, Vector3D vec2, Location3D loc)
        {
            OrthogonalVector = vec1.CrossProduct(vec2);
            A = OrthogonalVector.X;
            B = OrthogonalVector.Y;
            C = OrthogonalVector.Z;
            D = -(A * loc.X + B * loc.Y + C * loc.Z);
        }

        /// <summary>
        /// Creates two vectors that lie in the plane, and then uses the above.
        /// </summary>
        /// <param name="loc1"></param>
        /// <param name="loc2"></param>
        /// <param name="loc3"></param>
        public Plane(Location3D loc1, Location3D loc2, Location3D loc3)
        {
            Vector3D vec1 = new Vector3D(loc1, loc2);
            Vector3D vec2 = new Vector3D(loc1, loc3);
            OrthogonalVector = vec1.CrossProduct(vec2);
            A = OrthogonalVector.X;
            B = OrthogonalVector.Y;
            C = OrthogonalVector.Z;
            D = -(A * loc1.X + B * loc1.Y + C * loc1.Z);
        }

        public double X(double y, double z)
        {
            return -(B * y + C * z + D) / A;
        }

        public double Y(double x, double z)
        {
            return -(A * x + C * z + D) / B;
        }

        public double Z(double x, double y)
        {
            return -(A * x + B * y + D) / C;
        }

        public Location3D XLocation(double y, double z)
        {
            return new Location3D(X(y, z), y, z);
        }

        public Location3D YLocation(double x, double z)
        {
            return new Location3D(x, Y(x,z), z);
        }

        public Location3D ZLocation(double x, double y)
        {
            return new Location3D(x, y, Z(x, y));
        }

        public double Angle(Plane plane)
        {
            return OrthogonalVector.Angle(plane.OrthogonalVector);
        }

        public double OtherAngle(Plane plane)
        {
            return Math.PI - OrthogonalVector.Angle(plane.OrthogonalVector);
        }

        public bool IsEqual(Plane plane)
        {
            return A * plane.B == B * plane.A && B * plane.C == C * plane.B && C * plane.D == D * plane.C;
        }

        /// <summary>
        /// Only useful if the planes are equal.
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public double Relation(Plane plane)
        {
            return A / plane.A;
        }

        public override string ToString()
        {
            return "" + A + "x " + (B >= 0 ? "+ " + B : "- " + B * -1) + "y " + (C >= 0 ? "+ " + C : "- " + C * -1) + "z " + (D >= 0 ? "+ " + D : "- " + D * -1) + " = 0";
        }
    }
}