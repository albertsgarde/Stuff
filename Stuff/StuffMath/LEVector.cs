using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.StuffMath.Structures;

namespace Stuff.StuffMath
{
    public class LEVector : IEnumerable<double>, IVectorSpace<LEVector, FDouble>
    {
        private readonly double[] vector;

        public LEVector(params double[] vector)
        {
            this.vector = vector;
        }

        public LEVector(IEnumerable<double> vector) : this(vector.ToArray())
        {

        }

        public LEVector(LEMatrix.MatrixRow mr)
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

        public LEVector ZERO => new LEVector(ContainerUtils.UniformArray(0d, Size));

        public FDouble ONE => new FDouble(1);

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The total of the two vectors.</returns>
        public static LEVector operator +(LEVector vecA, LEVector vecB)
        {
            if (vecA.Size != vecB.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            return new LEVector(vecA.Zip(vecB, (x, y) => x + y).ToArray());
        }

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The right hand subtracted from the left hand.</returns>
        public static LEVector operator -(LEVector vecA, LEVector vecB)
        {
            if (vecA.Size != vecB.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            return new LEVector(vecA.Zip(vecB, (x, y) => x - y).ToArray());
        }

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The right hand subtracted from the left hand.</returns>
        public static LEVector operator -(LEVector vec)
        {
            return new LEVector(vec.Select(d => -d).ToArray());
        }

        /// <returns>The vector multiplied by the double.</returns>
        public static LEVector operator *(LEVector vec, double d)
        {
            return new LEVector(vec.Select(x => x * d).ToArray());
        }

        /// <returns>The vector divided by the double.</returns>
        public static LEVector operator /(LEVector vec, double d)
        {
            return new LEVector(vec.Select(x => x / d).ToArray());
        }

        /// <returns>The vector multiplied by the double.</returns>
        public static LEVector operator *(double d, LEVector vec)
        {
            return new LEVector(vec.Select(x => x * d).ToArray());
        }

        /// <returns>The modulus is applied to every element.</returns>
        public static LEVector operator %(LEVector vec, double d)
        {
            return new LEVector(vec.Select(x => x % d).ToArray());
        }

        public LEVector ToLEVector() => this;

        public double DotSum(LEVector vec)
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
        public LEVector Project(LEVector vec)
        {
            throw new Exception("Doesn't work");
            return vec * (DotSum(vec) / vec.LengthSquared);
        }

        public double ProjectionLength(LEVector vec)
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

        public bool Equals(LEVector vec)
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

        public LEVector Add(LEVector t)
        {
            return this + t;
        }

        public LEVector AdditiveInverse()
        {
            return -this;
        }

        public LEVector Multiply(FDouble s)
        {
            return this * s;
        }

        public bool EqualTo(LEVector t)
        {
            return this == t;
        }
    }
}
