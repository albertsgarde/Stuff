using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.StuffMath.Structures;

namespace Stuff.StuffMath
{
    public class Vector<F> : IEnumerable<F>, IHilbertSpace<Vector<F>, F> where F : IHilbertField<F>, new()
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
                return DotSum(Conjugate());
            }
        }

        public F Length => LengthSquared.AbsSqrt();

        public Vector<F> ZERO => new Vector<F>();

        public F ONE => new F().ONE;

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

        /// <returns>The vector multiplied by the F.</returns>
        public static Vector<F> operator *(F d, Vector<F> vec)
        {
            return new Vector<F>(vec.Select(x => x.Multiply(d)).ToArray());
        }

        /// <returns>The vector divided by the F.</returns>
        public static Vector<F> operator /(Vector<F> vec, F d)
        {
            return vec.Divide(d);
        }

        public static bool operator ==(Vector<F> vec1, Vector<F> vec2)
        {
            foreach(var (f1, f2) in vec1.Zip(vec2, (f1, f2) => (f1, f2)))
            {
                if (!f1.EqualTo(f2))
                    return false;
            }
            return true;
        }

        public static bool operator !=(Vector<F> vec1, Vector<F> vec2)
        {
            foreach (var (f1, f2) in vec1.Zip(vec2, (f1, f2) => (f1, f2)))
            {
                if (!f1.EqualTo(f2))
                    return true;
            }
            return false;
        }

        public Vector<F> ToVector() => this;
        
        /// <param name="dim">The size of the vector.</param>
        /// <param name="axis">Which axis this is the unit vector of.</param>
        public static Vector<F> UnitVector(int dim, int axis)
        {
            var result = new List<F>();
            for (int i = 0; i < axis; ++i)
                result.Add(new F());
            result.Add(new F().ONE);
            for (int i = axis + 1; i < dim; ++i)
                result.Add(new F());
            return new Vector<F>(result);
        }

        public bool IsUnitVector()
        {
            return vector.Count(f => f.IsOne()) == 1 && vector.Count(f => f.IsZero()) == vector.Count() - 1;
        }

        public Vector<F> Normalize()
        {
            return this * Length.MultiplicativeInverse(); 
        }

        public static Vector<F> NullVector(int dim)
        {
            return new Vector<F>(ContainerUtils.UniformArray(new F(), dim));
        }

        public bool IsNull()
        {
            return vector.Count(f => !f.IsZero()) == 0;
        }

        public Vector<F> Conjugate()
        {
            return new Vector<F>(this.Select(f => f.Conjugate()));
        }

        public F DotSum(Vector<F> vec)
        {
            if (Size != vec.Size)
                throw new ArgumentException("The vectors must have the same number of dimensions.");
            var total = vector[0].Multiply(vec[0].Conjugate());
            for (int i = 1; i < Size; ++i)
                total = total.Add(vector[i].Multiply(vec[i].Conjugate()));
            return total;
        }

        /// <summary>
        /// Projects this vector onto the specified vector.
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public Vector<F> Project(Vector<F> vec)
        {
            return vec * DotSum(vec).Divide(Length.Multiply(vec.Length));
        }

        public F ProjectionLengthSquared(Vector<F> vec)
        {
            return DotSum(vec).Square().Divide(vec.LengthSquared);
        }

        public F Total()
        {
            return vector.Aggregate(new F().ZERO, (total, x) => total.Add(x));
        }

        public bool LinearlyDependent(Vector<F> v)
        {
            if (v.Size != Size)
                throw new ArgumentException("A vector must be the same size as another to be linearly dependent with it.");
            var factor = v[0].Divide(this[0]);
            foreach(var (f1, f2) in this.Zip(v, (f1, f2) => (f1, f2)))
            {
                if (!f2.Divide(f1).EqualTo(factor))
                    return false;
            }
            return true;
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

        public override bool Equals(object obj)
        {
            if (obj is Vector<F> v)
                return v.Equals(this);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Misc.HashCode(17, 23, vector);
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
