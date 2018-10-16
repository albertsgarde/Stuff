using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Vector : IEnumerable<double>
    {
        private readonly double[] vector;

        public Vector(params double[] vector)
        {
            this.vector = vector;
        }

        public Vector(IEnumerable<double> vector) : this(vector.ToArray())
        {

        }

        public Vector(LEMatrix.MatrixRow mr)
        {
            vector = new double[mr.Length];
            for (int i = 0; i < mr.Length; ++i)
                vector[i] = mr[i];
        }

        public double this[int i]
        {
            get
            {
                return vector[i];
            }
        }

        /// <summary>
        /// The number of dimensions.
        /// </summary>
        public int Size
        {
            get
            {
                return vector.Length;
            }
        }

        /// <summary>
        /// The geometric length of the vector.
        /// </summary>
        public double Length
        {
            get
            {
                return Math.Sqrt(vector.Aggregate((total, x) => total + Math.Pow(x, 2)));
            }
        }
        
        /// <summary>
        /// The squared geometric length of the vector.
        /// </summary>
        public double LengthSquared
        {
            get
            {
                return vector.Aggregate((total, x) => total + Math.Pow(x, 2));
            }
        }

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The total of the two vectors.</returns>
        public static Vector operator +(Vector vecA, Vector vecB)
        {
            if (vecA.Size != vecB.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            return new Vector(vecA.Zip(vecB, (x, y) => x + y).ToArray());
        }

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The right hand subtracted from the left hand.</returns>
        public static Vector operator -(Vector vecA, Vector vecB)
        {
            if (vecA.Size != vecB.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            return new Vector(vecA.Zip(vecB, (x, y) => x - y).ToArray());
        }

        /// <returns>The vector multiplied by the double.</returns>
        public static Vector operator *(Vector vec, double d)
        {
            return new Vector(vec.Select(x => x * d).ToArray());
        }

        /// <returns>The vector divided by the double.</returns>
        public static Vector operator /(Vector vec, double d)
        {
            return new Vector(vec.Select(x => x / d).ToArray());
        }

        /// <returns>The vector multiplied by the double.</returns>
        public static Vector operator *(double d, Vector vec)
        {
            return new Vector(vec.Select(x => x * d).ToArray());
        }

        /// <returns>The vector divided by the double.</returns>
        public static Vector operator /(double d, Vector vec)
        {
            return new Vector(vec.Select(x => x / d).ToArray());
        }

        /// <returns>The modulus is applied to every element.</returns>
        public static Vector operator %(Vector vec, double d)
        {
            return new Vector(vec.Select(x => x % d).ToArray());
        }

        public double DotSum(Vector vec)
        {
            if (Size != vec.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            return this.Zip(vec, (x, y) => x * y).Sum();
        }

        /// <summary>
        /// Projects this vector onto the specified vector.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public Vector Project(Vector vec)
        {
            throw new Exception("Doesn't work");
            return vec * (DotSum(vec) / vec.LengthSquared);
        }

        public double ProjectionLength(Vector vec)
        {
            return Basic.Norm(DotSum(vec) / vec.Length);
        }

        public IEnumerator<double> GetEnumerator()
        {
            foreach (double d in vector)
                yield return d;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(Vector vec)
        {
            return this.Zip(vec, (x, y) => x == y).Count(x => !x) == 0;
        }

        public LEMatrix.MatrixRow ToMatrixRow()
        {
            return new LEMatrix.MatrixRow(this);
        }

        public override string ToString()
        {
            string result = "(";
            foreach (var d in vector)
                result += d + ", ";
            return result.Substring(0, result.Length - 2) + ")";
        }
    }
}
