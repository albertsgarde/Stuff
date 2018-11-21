using Stuff.StuffMath.Complex;
using Stuff.StuffMath.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class MatrixRow : IEnumerable<Complex2D>
    {
        public IReadOnlyList<Complex2D> Data { get; }

        public int Length => Data.Count;

        public MatrixRow(params Complex2D[] data)
        {
            Data = data.Copy();
        }

        public MatrixRow(int length, Complex2D value)
        {
            var data = new Complex2D[length];
            for (int i = 0; i < length; ++i)
                data[i] = value;
            Data = data;
        }

        public MatrixRow(Vector v)
        {
            Data = v.ToArray();
        }

        public Complex2D this[int index] => Data[index];

        public MatrixRow Scale(Complex2D scalar)
        {
            var result = new Complex2D[Length];
            for (int i = 0; i < Length; ++i)
                result[i] = Data[i].Multiply(scalar);
            return new MatrixRow(result);
        }

        public MatrixRow Add(MatrixRow mr)
        {
            if (Length != mr.Length)
                throw new ArgumentException("In order to add two rows, they must have the same length.");
            var result = new Complex2D[Length];
            for (int i = 0; i < Length; ++i)
                result[i] = Data[i].Add(mr[i]);
            return new MatrixRow(result);
        }

        public Vector ToVector()
        {
            return new Vector(this);
        }

        /*public LinearEquation ToLinearEquation()
        {
            return new LinearEquation(Data.Last(), Data.Take(Data.Count - 1));
        }*/

        public IEnumerator<Complex2D> GetEnumerator()
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
