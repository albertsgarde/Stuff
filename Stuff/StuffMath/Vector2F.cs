using System;

namespace Stuff.StuffMath
{
    public class Vector2F
    {
        public static Vector2F NullVector { get; private set; } = new Vector2F(0, 0);

        public float X { get; private set; }
        
        public float Y { get; private set; }

        private bool radiansFound;
        private float radians;

        private bool lengthFound;
        private float length;

        public Vector2F(float x, float y)
        {
            X = x;
            Y = y;
            lengthFound = false;
            radiansFound = false;
        }

        /// <summary>
        /// Creates a new vector with the same length, but with the specified angle in radians.
        /// </summary>
        /// <param name="radians"></param>
        public Vector2F(Vector2F vec, float radians)
        {
            lengthFound = true;
            length = vec.Length;
            radiansFound = true;
            this.radians = radians;
            X = (float)Math.Cos(radians) * length;
            Y = (float)Math.Sin(radians) * length;
        }

        /// <summary>
        /// Creates a new Vector from locA to locB
        /// </summary>
        /// <param name="locA">Start location</param>
        /// <param name="locB">End location</param>
        public Vector2F(Location2F locA, Location2F locB)
        {
            X = locB.X - locA.X;
            Y = locB.Y - locA.Y;
            lengthFound = false;
            radiansFound = false;
        }

        public Vector2F(Location2F coords)
        {
            X = coords.X;
            Y = coords.Y;
            lengthFound = false;
            radiansFound = false;
        }

        public float Degrees
        {
            get
            {
                return Radians / ((float)Math.PI / 180);
            }
        }

        public float Radians
        {
            get
            {
                return radiansFound ? radians : radians = Y < 0 ? 2 * (float)Math.PI - (float)Math.Acos(X / Length) : (float)Math.Acos(X / Length);
            }
        }

        public float Length
        {
            get
            {
                return lengthFound ? length : length = (float)Math.Sqrt(X * X + Y * Y);
            }
        }

        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        public static Vector2F operator +(Vector2F vecA, Vector2F vecB)
        {
            return new Vector2F(vecA.X + vecB.X, vecA.Y + vecB.Y);
        }

        public static Vector2F operator -(Vector2F vecA, Vector2F vecB)
        {
            return new Vector2F(vecA.X - vecB.X, vecA.Y - vecB.Y);
        }

        public static Vector2F operator * (Vector2F vec, float multiplier)
        {
            return new Vector2F(vec.X * multiplier, vec.Y * multiplier);
        }

        public static Vector2F operator /(Vector2F vec, float divisor)
        {
            return new Vector2F(vec.X / divisor, vec.Y / divisor);
        }

        public static Vector2F operator *(float multiplier, Vector2F vec)
        {
            return new Vector2F(vec.X * multiplier, vec.Y * multiplier);
        }

        public static Vector2F operator /(float divisor, Vector2F vec)
        {
            return new Vector2F(vec.X / divisor, vec.Y / divisor);
        }

        //add this code to class ThreeDPoint as defined previously
        //
        public static bool operator ==(Vector2F a, Vector2F b)
        {
            // If both are null, or both are same instance, return true.
            if (Object.ReferenceEquals(a, b))
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

        public static bool operator !=(Vector2F a, Vector2F b)
        {
            return !(a == b);
        }

        public bool IsParallel(Vector2F vec)
        {
            return X * Y == vec.X * vec.Y;
        }

        public Vector2F OrthogonalVector()
        {
            return new Vector2F(-Y, X);
        }

        /*
         * dotSum(vector) == 0;
         * @param vector - the other Vector.
         * @return whether or not the Vectors are orthogonal.
        */
        public bool IsOrthogonal(Vector2F vec)
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
        public float CosAngle(Vector2F vec)
        {
            return (DotSum(vec)) / (Length * vec.Length);
        }

        /*
         * @param vector - the Vector to find the angle to.
         * @return the angle between the two Vectors in radians.
         */
        public float Angle(Vector2F vec)
        {
            return (float)Math.Acos(CosAngle(vec));
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
        public Vector2F Project(Vector2F vec)
        {
            return vec * ((DotSum(vec)) / (vec.LengthSquared()));
        }

        public float ProjectionLength(Vector2F vec)
        {
            return Basic.Norm(DotSum(vec)) / vec.Length;
        }

        public float DotSum(Vector2F vec)
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
        public float Determinant(Vector2F vec)
        {
            return X*vec.Y - Y*vec.X;
        }

        public Vector2F Scale(float length)
        {
            return new Vector2F(length * X / Length, length / Length * Y);
        }

        public Location2F Location()
        {
            return new Location2F(X, Y);
        }

        public bool IsNullVector()
        {
            return X == 0 && Y == 0;
        }

        public Vector2F Turn(float rad)
        {
            return IsNullVector() ? this : new Vector2F(this, (Radians + rad) % ((float)Math.PI * 2));
        }

        public static implicit operator Location2F(Vector2F vec)
        {
            return vec.Location();
        }

        public static implicit operator Vector2D(Vector2F vec)
        {
            return new Vector2D(vec.X, vec.Y);
        }

        public override bool Equals(object vec)
        {
            Vector2F vecVec;
            return vec != null && ((vecVec = vec as Vector2F) != null) && Misc.FloatEquals(Length, vecVec.Length) && Misc.FloatEquals(Radians, vecVec.Radians);
        }

        public override string ToString()
        {
            return "(" + X + "," + Y + ")";
        }

        public override int GetHashCode()
        {
            return Misc.HashCode(17, 23, X, Y);
        }
    }
}
