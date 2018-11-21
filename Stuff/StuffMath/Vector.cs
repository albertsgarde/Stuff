using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.StuffMath.Complex;
using Stuff.StuffMath.Structures;

namespace Stuff.StuffMath
{
    public class Vector : IEnumerable<Complex2D>, IHilbertSpace<Vector, Complex2D>
    {
        private readonly Complex2D[] vector;

        public Vector()
        {
            vector = new Complex2D[0];
        }

        public Vector(params Complex2D[] vector)
        {
            this.vector = vector;
        }

        public Vector(IEnumerable<Complex2D> vector) : this(vector.ToArray())
        {

        }

        public Vector(MatrixRow mr)
        {
            vector = new Complex2D[mr.Length];
            for (int i = 0; i < mr.Length; ++i)
                vector[i] = mr[i];
        }

        public Complex2D this[int i]
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
        public Complex2D LengthSquared
        {
            get
            {
                return DotSum(Conjugate());
            }
        }

        public Complex2D Length => LengthSquared.AbsSqrt();

        public Vector ZERO => new Vector();

        public Complex2D ONE => new Complex2D().ONE;

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The total of the two vectors.</returns>
        public static Vector operator +(Vector vecA, Vector vecB)
        {
            if (vecA.Size != vecB.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            return new Vector(vecA.Zip(vecB, (x, y) => x.Add(y)).ToArray());
        }

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The right hand subtracted from the left hand.</returns>
        public static Vector operator -(Vector vecA, Vector vecB)
        {
            if (vecA.Size != vecB.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            return new Vector(vecA.Zip(vecB, (x, y) => x.Subtract(y)).ToArray());
        }

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The right hand subtracted from the left hand.</returns>
        public static Vector operator -(Vector vec)
        {
            return new Vector(vec.Select(d => d.AdditiveInverse()).ToArray());
        }

        /// <returns>The vector multiplied by the Complex2D.</returns>
        public static Vector operator *(Vector vec, Complex2D d)
        {
            return new Vector(vec.Select(x => x.Multiply(d)).ToArray());
        }

        /// <returns>The vector multiplied by the Complex2D.</returns>
        public static Vector operator *(Complex2D d, Vector vec)
        {
            return new Vector(vec.Select(x => x.Multiply(d)).ToArray());
        }

        /// <returns>The vector divided by the Complex2D.</returns>
        public static Vector operator /(Vector vec, Complex2D d)
        {
            return vec.Divide(d);
        }

        public static bool operator ==(Vector vec1, Vector vec2)
        {
            foreach(var (f1, f2) in vec1.Zip(vec2, (f1, f2) => (f1, f2)))
            {
                if (!f1.EqualTo(f2))
                    return false;
            }
            return true;
        }

        public static bool operator !=(Vector vec1, Vector vec2)
        {
            foreach (var (f1, f2) in vec1.Zip(vec2, (f1, f2) => (f1, f2)))
            {
                if (!f1.EqualTo(f2))
                    return true;
            }
            return false;
        }

        public Vector ToVector() => this;
        
        /// <param name="dim">The size of the vector.</param>
        /// <param name="axis">Which axis this is the unit vector of.</param>
        public static Vector UnitVector(int dim, int axis)
        {
            var result = new List<Complex2D>();
            for (int i = 0; i < axis; ++i)
                result.Add(new Complex2D());
            result.Add(new Complex2D().ONE);
            for (int i = axis + 1; i < dim; ++i)
                result.Add(new Complex2D());
            return new Vector(result);
        }

        public bool IsUnitVector()
        {
            return vector.Count(f => f.IsOne()) == 1 && vector.Count(f => f.IsZero()) == vector.Count() - 1;
        }

        public Vector Normalize()
        {
            return this * Length.MultiplicativeInverse(); 
        }

        public static Vector NullVector(int dim)
        {
            return new Vector(ContainerUtils.UniformArray(new Complex2D(), dim));
        }

        public bool IsNull()
        {
            return vector.Count(f => !f.IsZero()) == 0;
        }

        public Vector Conjugate()
        {
            return new Vector(this.Select(f => f.Conjugate()));
        }

        public Complex2D DotSum(Vector vec)
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
        public Vector Project(Vector vec)
        {
            return vec * DotSum(vec).Divide(Length.Multiply(vec.Length));
        }

        public Complex2D ProjectionLengthSquared(Vector vec)
        {
            return DotSum(vec).Square().Divide(vec.LengthSquared);
        }

        public Complex2D Total()
        {
            return vector.Aggregate(new Complex2D().ZERO, (total, x) => total.Add(x));
        }

        public bool LinearlyDependent(Vector v)
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

        public IEnumerator<Complex2D> GetEnumerator()
        {
            foreach (Complex2D d in vector)
                yield return d;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(Vector vec)
        {
            return this.Zip(vec, (x, y) => x.EqualTo(y)).Count(x => !x) == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector v)
                return v.Equals(this);
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Misc.HashCode(17, 23, vector);
        }

        public MatrixRow ToMatrixRow()
        {
            return new MatrixRow(this);
        }

        public override string ToString()
        {
            string result = "(";
            foreach (var d in vector)
                result += d + ", ";
            return result.Substring(0, result.Length - 2) + ")";
        }

        public Vector Add(Vector t)
        {
            return this + t;
        }

        public Vector AdditiveInverse()
        {
            return -this;
        }

        public Vector Multiply(Complex2D s)
        {
            return this * s;
        }

        public bool EqualTo(Vector t)
        {
            return this == t;
        }
    }
}
