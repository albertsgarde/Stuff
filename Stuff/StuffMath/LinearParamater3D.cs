using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class LinearParamater3D
    {
        public Vector3D A { get; private set; }

        public Vector3D B { get; private set; }

        public LinearParamater3D(double xA, double xB, double yA, double yB, double zA, double zB)
        {
            A = new Vector3D(xA, yA, zA);
            B = new Vector3D(xB, yB, zB);
        }

        public LinearParamater3D(Vector3D b, Vector3D a)
        {
            A = a;
            B = b;
        }

        public LinearParamater3D(Location3D loc1, Location3D loc2)
        {
            B = new Vector3D(loc1);
            A = new Vector3D(loc1, loc2);
        }

        public LinearParamater3D(Location3D loc, Vector3D vec)
        {
            B = new Vector3D(loc);
            A = vec;
        }

        public bool Intersect(LinearParamater3D lp)
        {
            return (lp.B - B) * A.CrossProduct(lp.B) == 0;
        }

        public double PlaneIntersection(Plane plane)
        {
            return -((B * plane.OrthogonalVector) + plane.D) / (A * plane.OrthogonalVector);
        }

        public Location3D PlaneIntersectionPoint(Plane plane)
        {
            return Point(PlaneIntersection(plane));
        }

        /// <summary>
        /// </summary>
        /// <returns>The t value for the LinearParameter3D's intersection with the XY-plane.</returns>
        public double XYPlaneIntersection(double z = 0)
        {
            return (z-B.Z) / A.Z;
        }

        public Location3D XYPlaneIntersectionPoint()
        {
            return new Location3D(B.X - (B.Z * A.X / A.Z), B.Y - (B.Z * A.Y / A.Z), 0);
        }

        public double X(double t)
        {
            return A.X * t + B.X;
        }

        public double Y(double t)
        {
            return A.Y * t + B.Y;
        }

        public double Z(double t)
        {
            return A.Z * t + B.Z;
        }

        public Location3D Point(double t)
        {
            return new Location3D(B + A * t);
        }

        public Vector3D DirectionVector()
        {
            return A;
        }

        public override string ToString()
        {
            return "{" + B + " + t" + A + "}";
        }
    }
}
