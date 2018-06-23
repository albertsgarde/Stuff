﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;
using System.Collections;

namespace Stuff.StuffMath
{
    /// <summary>
    /// A complex vector
    /// </summary>
    public class CVector : IEnumerable<Complex>
    {
        private readonly Complex[] vector;

        public CVector(params Complex[] vector)
        {
            this.vector = vector;
        }

        public Complex this[int i]
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
        /*
        /// <summary>
        /// The geometric length of the vector.
        /// </summary>
        public Complex Length
        {
            get
            {
                return Math.Sqrt(vector.Aggregate((total, x) => total + Math.Pow(x, 2)));
            }
        }

        /// <summary>
        /// The squared geometric length of the vector.
        /// </summary>
        public Complex LengthSquared
        {
            get
            {
                return vector.Aggregate((total, x) => total + Math.Pow(x, 2));
            }
        }*/

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The total of the two vectors.</returns>
        public static CVector operator +(CVector vecA, CVector vecB)
        {
            if (vecA.Size != vecB.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            return new CVector(vecA.Zip(vecB, (x, y) => x + y).ToArray());
        }

        /// <summary>
        /// The vectors must have the same number of dimensions.
        /// </summary>
        /// <returns>The right hand subtracted from the left hand.</returns>
        public static CVector operator -(CVector vecA, CVector vecB)
        {
            if (vecA.Size != vecB.Size)
                throw new ArgumentException("The vectors must be have the same number of dimensions.");
            return new CVector(vecA.Zip(vecB, (x, y) => x - y).ToArray());
        }

        /// <returns>The vector multiplied by the Complex.</returns>
        public static CVector operator *(CVector vec, Complex c)
        {
            return new CVector(vec.Select(x => x * c).ToArray());
        }

        /// <returns>The vector divided by the Complex.</returns>
        public static CVector operator /(CVector vec, Complex c)
        {
            return new CVector(vec.Select(x => x / c).ToArray());
        }

        /// <returns>The vector multiplied by the Complex.</returns>
        public static CVector operator *(Complex c, CVector vec)
        {
            return new CVector(vec.Select(x => x * c).ToArray());
        }

        /// <returns>The vector divided by the Complex.</returns>
        public static CVector operator /(Complex c, CVector vec)
        {
            return new CVector(vec.Select(x => x / c).ToArray());
        }

        public Complex InnerProduct(CVector vec)
        {
            if (Size != vec.Size)
                throw new ArgumentException("The vectors must have the same number of dimensions.");
            Complex result = Complex.NULL;
            for (int i = 0; i < Size; i++)
                result += vector[i] * vec[i].Conjugate();
            return result;
        }

        public CVector Conjugate()
        {
            return new CVector(vector.Select(z => z.Conjugate()).ToArray());
        }

        public bool Orthogonal(CVector vec)
        {
            return InnerProduct(vec) == Complex.NULL;
        }

        public IEnumerator<Complex> GetEnumerator()
        {
            foreach (Complex d in vector)
                yield return d;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(CVector vec)
        {
            return this.Zip(vec, (x, y) => x == y).Count(x => !x) == 0;
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
