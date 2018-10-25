using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Vector3D
    {
        public double X { get; private set; }

        public double Y { get; private set; }

        public double Z { get; private set; }

        private bool lengthFound;

        private double length;

        public Vector3D()
        {
            X = 0;
            Y = 0;
            Z = 0;
            length = 0;
        }

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
            lengthFound = false;
        }

        public Vector3D(Vector3D vec, double length)
        {
            X = vec.X * length / vec.length;
            Y = vec.Y * length / vec.length;
            Z = vec.Z * length / vec.length;
            lengthFound = false;
        }

        public Vector3D(Location3D loc1, Location3D loc2)
        {
            X = loc2.X - loc1.X;
            Y = loc2.Y - loc1.Y;
            Z = loc2.Z - loc1.Z;
            lengthFound = false;
        }

        public Vector3D(Location3D loc)
        {
            X = loc.X;
            Y = loc.Y;
            Z = loc.Z;
            lengthFound = false;
        }

        public static Vector3D NullVector
        {
            get
            {
                return new Vector3D(0, 0, 0);
            }
        }

        public double Length
        {
            get
            {
                if (lengthFound)
                    return length;
                else
                {
                    lengthFound = true;
                    return length = Math.Sqrt(X * X + Y * Y + Z * Z);
                }
            }
        }

        public double LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        public static Vector3D operator +(Vector3D vecA, Vector3D vecB)
        {
            return new Vector3D(vecA.X + vecB.X, vecA.Y + vecB.Y, vecA.Z + vecB.Z);
        }

        public static Vector3D operator -(Vector3D vecA, Vector3D vecB)
        {
            return new Vector3D(vecA.X - vecB.X, vecA.Y - vecB.Y, vecA.Z - vecB.Z);
        }

        public static Vector3D operator *(Vector3D vec, double multiplier)
        {
            return new Vector3D(vec.X * multiplier, vec.Y * multiplier, vec.Z * multiplier);
        }

        public static Vector3D operator *(double multiplier, Vector3D vec)
        {
            return new Vector3D(vec.X * multiplier, vec.Y * multiplier, vec.Z * multiplier);
        }

        public static Vector3D operator /(Vector3D vec, double divisor)
        {
            return new Vector3D(vec.X / divisor, vec.Y / divisor, vec.Z / divisor);
        }

        public static double operator *(Vector3D vec1, Vector3D vec2)
        {
            return vec1.DotSum(vec2);
        }

        public static implicit operator Vector(Vector3D vec)
        {
            return new Vector(vec.X, vec.Y, vec.Z);
        }

        public double DotSum(Vector3D vec)
        {
            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public Vector3D CrossProduct(Vector3D vec)
        {
            return new Vector3D(Y*vec.Z-Z*vec.Y, Z*vec.X-X*vec.Z, X*vec.Y-Y*vec.X);
        }

        public bool IsParallel(Vector3D vec)
        {
            return CrossProduct(vec).Equals(NullVector);
        }

        public Vector3D Project(Vector3D vec)
        {
            return vec * (DotSum(vec) / vec.LengthSquared());
        }

        public double ProjectionLength(Vector3D vec)
        {
            return Basic.Norm(DotSum(vec) / vec.Length);
        }

        public double Angle(Vector3D vec)
        {
            return Math.Acos(DotSum(vec) / (Length * vec.Length));
        }

        public Location3D Location()
        {
            return new Location3D(X, Y, Z);
        }

        public static implicit operator Location3D(Vector3D vec)
        {
            return vec.Location();
        }

        public bool Equals(Vector3D vec)
        {
            return X == vec.X && Y == vec.Y && Z == vec.Z;
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + "," + Z + ")";
        }
    }
}
