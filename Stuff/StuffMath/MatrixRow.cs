using Stuff.StuffMath.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class MatrixRow<F> : IEnumerable<F> where F : IHilbertField<F>, new()
    {
        public IReadOnlyList<F> Data { get; }

        public int Length => Data.Count;

        public MatrixRow(params F[] data)
        {
            Data = data.Copy();
        }

        public MatrixRow(int length, F value)
        {
            var data = new F[length];
            for (int i = 0; i < length; ++i)
                data[i] = value;
            Data = data;
        }

        public MatrixRow(Vector<F> v)
        {
            Data = v.ToArray();
        }

        public F this[int index] => Data[index];

        public MatrixRow<F> Scale(F scalar)
        {
            var result = new F[Length];
            for (int i = 0; i < Length; ++i)
                result[i] = Data[i].Multiply(scalar);
            return new MatrixRow<F>(result);
        }

        public MatrixRow<F> Add(MatrixRow<F> mr)
        {
            if (Length != mr.Length)
                throw new ArgumentException("In order to add two rows, they must have the same length.");
            var result = new F[Length];
            for (int i = 0; i < Length; ++i)
                result[i] = Data[i].Add(mr[i]);
            return new MatrixRow<F>(result);
        }

        public Vector<F> ToVector()
        {
            return new Vector<F>(this);
        }

        /*public LinearEquation ToLinearEquation()
        {
            return new LinearEquation(Data.Last(), Data.Take(Data.Count - 1));
        }*/

        public IEnumerator<F> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            var result = "[";
            for (int i = 0; i < Length - 1; ++i)
            {
                result += Data[i] + ",";
            }
            return result + Data[Length - 1] + "]";
        }
    }
}
