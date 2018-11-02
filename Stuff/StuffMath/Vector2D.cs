using System;
using Stuff.StuffMath.Structures;

namespace Stuff.StuffMath
{
    public class Vector2D : IVectorSpace<Vector2D, Real>
    {
        public double X { get; }

        public double Y { get; }

        public double Radians { get; }

        public double Length { get;  }

        public Vector2D(double radians)
        {
            X = Math.Cos(radians);
            Y = Math.Sin(radians);
            Length = 1;
            Radians = radians;
        }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
            Length = Math.Sqrt(LengthSquared());
            Radians = y < 0 ? 2 * Math.PI - Math.Acos(x / Length) : Math.Acos(x / Length);
        }

        /// <summary>
        /// Creates a new vector with the same angle, but with the specified length.
        /// </summary>
        /// <param name="length"></param>
        public Vector2D(Vector2D vec, double length)
        {
            X = vec.X / vec.Length * length;
            Y = vec.Y / vec.Length * length;
            Length = length;
            Radians = vec.Radians;
        }

        /// <summary>
        /// Creates a new Vector from locA to locB
        /// </summary>
        /// <param name="locA">Start location</param>
        /// <param name="locB">End location</param>
        public Vector2D(Location2D locA, Location2D locB)
        {
            X = locB.X - locA.X;
            Y = locB.Y - locA.Y;
            Length = Math.Sqrt(LengthSquared());
            Radians = Y < 0 ? 2 * Math.PI - Math.Acos(X / Length) : Math.Acos(X / Length);
        }

        public Vector2D(Location2D coords)
        {
            X = coords.X;
            Y = coords.Y;
            Length = Math.Sqrt(LengthSquared());
            Radians = Y < 0 ? 2 * Math.PI - Math.Acos(X / Length) : Math.Acos(X / Length);
        }

        public Vector ToVector() => new Vector(X, Y);

        public static Vector2D AngularVector(double radians, double length)
        {
            return new Vector2D(radians) * length;
        }

        public double Degrees
        {
            get => Radians / (Math.PI / 180);
        }

        public double LengthSquared()
        {
            return X * X + Y * Y;
        }

        public static Vector2D operator +(Vector2D vecA, Vector2D vecB)
        {
            return new Vector2D(vecA.X + vecB.X, vecA.Y + vecB.Y);
        }

        public static Vector2D operator -(Vector2D vecA, Vector2D vecB)
        {
            return new Vector2D(vecA.X - vecB.X, vecA.Y - vecB.Y);
        }

        public static Vector2D operator -(Vector2D vec)
        {
            return new Vector2D(-vec.X, -vec.Y);
        }

        public static Vector2D operator * (Vector2D vec, double multiplier)
        {
            return new Vector2D(vec.X * multiplier, vec.Y * multiplier);
        }

        public static Vector2D operator /(Vector2D vec, double divisor)
        {
            return new Vector2D(vec.X / divisor, vec.Y / divisor);
        }

        public static Vector2D operator *(double multiplier, Vector2D vec)
        {
            return new Vector2D(vec.X * multiplier, vec.Y * multiplier);
        }


        //add this code to class ThreeDPoint as defined previously
        //
        public static bool operator ==(Vector2D a, Vector2D b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(Vector2D a, Vector2D b)
        {
            return !(a == b);
        }

        public static implicit operator Vector(Vector2D v)
        {
            return new Vector(v.X, v.Y);
        }

        public static Vector2D UnitX { get; } = new Vector2D(1, 0);

        public static Vector2D UnitY { get; } = new Vector2D(0, 1);

        public Vector2D ZERO => throw new NotImplementedException();

        public Real ONE => throw new NotImplementedException();

        public bool IsParallel(Vector2D vec)
        {
            return X * Y == vec.X * vec.Y;
        }

        public Vector2D OrthogonalVector()
        {
            return new Vector2D(-Y, X);
        }

        /*
         * dotSum(vector) == 0;
         * @param vector - the other Vector.
         * @return whether or not the Vectors are orthogonal.
        */
        public bool IsOrthogonal(Vector2D vec)
        {
            return -Y / vec.X == X / vec.Y;
        }

        /*
         * a*b/(|a||b|
         * Draw last leg of triangle between vectors.
         * Cosine relation.
         * @param vector
         * @return
         */
        public double CosAngle(Vector2D vec)
        {
            return (DotSum(vec)) / (Length * vec.Length);
        }

        /*
         * @param vector - the Vector to find the angle to.
         * @return the angle between the two Vectors in radians.
         */
        public double Angle(Vector2D vec)
        {
            return Math.Acos(CosAngle(vec));
        }

        /// <summary>
        /// Projects this Vector onto the specified Vector.
        /// 
        /// BEVIS:
        /// Projektion af b ligger på b, og er derfor parallel med.
        /// Vectoren c er a - a.project(b)
        /// c.dotSum(b) = 0 da de er ortogonale.
        /// a.project(b) = b*k <=>
        /// b = a.project(b)/k
        /// 
        /// a = a.project(b) + c
        /// (a-a.project(b))*b = 0 <=>
        /// a.dotSum(b) - a.project(b).dotSum(b) = 0 <=>
        /// a.dotSum(b)-b.dotSum(k*b) = 0 <=>
        /// b.dotSum(k*b) = a.dotSum(b) <=>
        /// k = a.dotSum(b)/b.dotSum(b) <=>
        /// k = a.dotSum(b)/|b|*|b| 		Da v.dotSum(v) = |v|*|v|
        /// 
        /// c = b*(a.dotSum(b)/(|b|*|b|))
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public Vector2D Project(Vector2D vec)
        {
            return vec * ((DotSum(vec)) / vec.LengthSquared());
        }

        public double ProjectionLength(Vector2D vec)
        {
            return Basic.Norm(DotSum(vec)) / vec.Length;
        }

        public double DotSum(Vector2D vec)
        {
            return X*vec.X + Y *vec.Y;
        }

        /// <summary>
        /// Angiver arealet af det parallelogram de to vektorer danner.
        /// DotSum(Tværvector til a, b)
        /// 
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public double Determinant(Vector2D vec)
        {
            return X*vec.Y - Y*vec.X;
        }

        public Vector2D Scale(double length)
        {
            return new Vector2D(length * X / Length, length / Length * Y);
        }


        public Location2D Location()
        {
            return new Location2D(X, Y);
        }

        public static implicit operator Location2D(Vector2D vec)
        {
            return vec.Location();
        }

        public static explicit operator Vector2F(Vector2D vec)
        {
            return new Vector2F((float)vec.X, (float)vec.Y);
        }

        public override bool Equals(object vec)
        {
            Vector2D vecVec;
            return vec != null && ((vecVec = vec as Vector2D) != null) && Misc.DoubleEquals(Length, vecVec.Length) && Misc.DoubleEquals(Radians, vecVec.Radians);
        }

        public override int GetHashCode()
        {
            return Misc.HashCode(17, 23, X, Y);
        }

        public string ToLatex()
        {
            return "\\begin{ pmatrix}" + X + "\\\\" + Y + "\\end{ pmatrix}";
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }

        public Vector2D Add(Vector2D t)
        {
            return this + t;
        }

        public Vector2D AdditiveInverse()
        {
            return -this;
        }

        public Vector2D Multiply(Real s)
        {
            return this * (double)s;
        }

        public bool EqualTo(Vector2D t)
        {
            return X == t.X && Y == t.Y;
        }
    }
}
