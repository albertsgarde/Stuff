using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.StuffMath.Structures;

namespace Stuff.StuffMath
{
    public class Vector<F> : IEnumerable<F>, IVectorSpace<Vector<F>, F> where F : IField<F>, new()
    {
        private readonly F[] vector;

        public Vector()
        {
            vector = new F[0];
        }

        public Vector(params F[] vector)
        {
            this.vector = vector;
        }

        public Vector(IEnumerable<F> vector) : this(vector.ToArray())
        {

        }

        public Vector(MatrixRow<F> mr)
        {
            vector = new F[mr.Length];
            for (int i = 0; i < mr.Length; ++i)
                vector[i] = mr[i];
        }

        public F this[int i]
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
        /// The squared geometric length of the vector.
        /// </summary>
        public F LengthSquared
        {
            get
            {
                return vector.Aggregate((total, x) => total.Add(x.Multiply(x)));
            }
        }

        public Vector<F> ZERO => new Vector<F>();

        public F ONE => throw new NotImplementedException();

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The total of the two vectors.</returns>
        public static Vector<F> operator +(Vector<F> vecA, Vector<F> vecB)
        {
            if (vecA.Size != vecB.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            return new Vector<F>(vecA.Zip(vecB, (x, y) => x.Add(y)).ToArray());
        }

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The right hand subtracted from the left hand.</returns>
        public static Vector<F> operator -(Vector<F> vecA, Vector<F> vecB)
        {
            if (vecA.Size != vecB.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            return new Vector<F>(vecA.Zip(vecB, (x, y) => x.Subtract(y)).ToArray());
        }

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The right hand subtracted from the left hand.</returns>
        public static Vector<F> operator -(Vector<F> vec)
        {
            return new Vector<F>(vec.Select(d => d.AdditiveInverse()).ToArray());
        }

        /// <returns>The vector multiplied by the F.</returns>
        public static Vector<F> operator *(Vector<F> vec, F d)
        {
            return new Vector<F>(vec.Select(x => x.Multiply(d)).ToArray());
        }

        /// <returns>The vector divided by the F.</returns>
        public static Vector<F> operator /(Vector<F> vec, F d)
        {
            return vec.Divide(d);
        }

        /// <returns>The vector multiplied by the F.</returns>
        public static Vector<F> operator *(F d, Vector<F> vec)
        {
            return new Vector<F>(vec.Select(x => x.Multiply(d)).ToArray());
        }

        public Vector<F> ToVector() => this;

        public F DotSum(Vector<F> vec)
        {
            if (Size != vec.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            var total = vector[0].Multiply(vec[0]);
            for (int i = 1; i < Size; ++i)
                total = total.Add(vector[i].Multiply(vec[i]));
            return total;
        }

        /// <summary>
        /// Projects this vector onto the specified vector.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public Vector<F> Project(Vector<F> vec)
        {
            throw new Exception("Doesn't work");
            //return vec * (DotSum(vec) / vec.LengthSquared);
        }

        public F ProjectionLengthSquared(Vector<F> vec)
        {
            return DotSum(vec).Multiply(DotSum(vec)).Divide(vec.LengthSquared);
        }

        public IEnumerator<F> GetEnumerator()
        {
            foreach (F d in vector)
                yield return d;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(Vector<F> vec)
        {
            return this.Zip(vec, (x, y) => x.EqualTo(y)).Count(x => !x) == 0;
        }

        public MatrixRow<F> ToMatrixRow()
        {
            return new MatrixRow<F>(this);
        }

        public override string ToString()
        {
            string result = "(";
            foreach (var d in vector)
                result += d + ", ";
            return result.Substring(0, result.Length - 2) + ")";
        }

        public Vector<F> Add(Vector<F> t)
        {
            return this + t;
        }

        public Vector<F> AdditiveInverse()
        {
            return -this;
        }

        public Vector<F> Multiply(F s)
        {
            return this * s;
        }

        public bool EqualTo(Vector<F> t)
        {
            return this == t;
        }
    }
}
