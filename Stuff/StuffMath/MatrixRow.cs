using System;
using System.Collections.Generic;
using System.Linq;

namespace Stuff.StuffMath
{
    public class MatrixRow : IEnumerable<double>
    {
        public double[] Data { get; private set; }

        public MatrixRow(params double[] data)
        {
            Data = data;
        }

        public int Length
        {
            get
            {
                return Data.Length;
            }
        }

        public double this[int column]
        {
            get
            {
                return Data[column];
            }
            private set
            {
                Data[column] = value;
            }
        }

        public static bool operator ==(MatrixRow matrixRow1, MatrixRow matrixRow2)
        {
            foreach (var row in matrixRow1.Zip(matrixRow2, Tuple.Create))
            {
                if (row.Item1 != row.Item2)
                    return false;
            }
            return true;
        }

        public static bool operator !=(MatrixRow matrixRow1, MatrixRow matrixRow2)
        {
            return !(matrixRow1 == matrixRow2);
        }

        public IEnumerator<double> GetEnumerator()
        {
            foreach (double d in Data)
                yield return d;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
