using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class LEMatrix
    {
        public IReadOnlyList<MatrixRow> Rows { get; }

        public IReadOnlyList<Vector> Columns
        {
            get
            {
                var result = new double[N][];
                for (int i = 0; i < N; ++i)
                    result[i] = new double[M];
                for (int j = 0; j < M; ++j)
                {
                    for (int i = 0; i < N; ++i)
                        result[i][j] = Rows[j][i];
                }
                return result.Select(v => new Vector(v)).ToList();
            }
        }

        public int M => Rows.Count;

        public int N { get; }

        public class MatrixRow : IEnumerable<double>
        {
            public IReadOnlyList<double> Data { get; }

            public int Length => Data.Count;

            public MatrixRow(params double[] data)
            {
                Data = data.Copy();
            }

            public MatrixRow(int length, double value = 0)
            {
                var data = new double[length];
                for (int i = 0; i < length; ++i)
                    data[i] = value;
                Data = data;
            }

            public MatrixRow(Vector v)
            {
                Data = v.ToArray();
            }

            public double this[int index] => Data[index];

            public MatrixRow Scale(double scalar)
            {
                var result = new double[Length];
                for (int i = 0; i < Length; ++i)
                    result[i] = Data[i] * scalar;
                return new MatrixRow(result);
            }

            public MatrixRow Add(MatrixRow mr)
            {
                if (Length != mr.Length)
                    throw new ArgumentException("In order to add two rows, they must have the same length.");
                var result = new double[Length];
                for (int i = 0; i < Length; ++i)
                    result[i] = Data[i] + mr[i];
                return new MatrixRow(result);
            }

            public Vector ToVector()
            {
                return new Vector(this);
            }

            public LinearEquation ToLinearEquation()
            {
                return new LinearEquation(Data.Last(), Data.Take(Data.Count - 1));
            }

            public IEnumerator<double> GetEnumerator()
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

        public LEMatrix(params MatrixRow[] rows)
        {
            N = rows.First().Length;
            for (int i = 1; i < rows.Length; ++i)
            {
                if (rows[i].Length != N)
                    throw new ArgumentException($"All rows must have the same length. The first row has length {N}, but row {i} has length {rows[i].Length}.");
            }
            Rows = rows.Copy();
        }

        public LEMatrix(IEnumerable<MatrixRow> rows) : this(rows.ToArray())
        {

        }

        public LEMatrix(params Vector[] columns)
        {
            N = columns.Length;

            var height = columns.First().Size;
            for (int i = 1; i < N; ++i)
            {
                if (columns[i].Size != height)
                    throw new ArgumentException($"All columns must have the same length. The first column has length {height}, but column {i} has length {columns[i].Size}.");
            }

            var result = new MatrixRow[columns.First().Size];
            for (int j = 0; j < height; ++j)
            {
                var row = new double[N];
                for (int i = 0; i < N; ++i)
                    row[i] = columns[i][j];
                result[j] = new MatrixRow(row);
            }
            Rows = result;
        }

        public LEMatrix(IEnumerable<Vector> columns) : this(columns.ToArray())
        {
        }

        public LEMatrix(IEnumerable<double> values, int rows)
        {
            if (values.Count() % rows != 0)
                throw new ArgumentException($"The number of values({values.Count()}) must be divisible by the number of rows({rows}).");
            N = values.Count() / rows;
            var result = new MatrixRow[rows];

            int j = -1;
            int i = 0;
            var row = new double[N];
            foreach (var value in values)
            {
                row[i] = value;
                if (++i >= N)
                {
                    i -= N;
                    result[++j] = new MatrixRow(row);
                }
            }
            Rows = result;
        }

        public MatrixRow this[int row] => Rows[row];

        public Vector Column(int column)
        {
            var result = new double[M];
            for (int j = 0; j < M; ++j)
                result[j] = Rows[j][column];
            return new Vector(result);
        }

        public IEnumerable<double> Values()
        {
            for (int j = 0; j < M; ++j)
            {
                for (int i = 0; i < N; ++i)
                    yield return this[j][i];
            }
        }

        public static bool operator ==(LEMatrix m1, LEMatrix m2)
        {
            if (m1.N != m2.N || m1.M != m2.M)
                return false;
            for (int j = 0; j < m1.M; ++j)
            {
                for (int i = 0; i < m1.N; ++i)
                    if (m1[j][i] != m2[j][i])
                        return false;
            }
            return true;
        }

        public static bool operator !=(LEMatrix m1, LEMatrix m2)
        {
            if (m1.N != m2.N || m1.M != m2.M)
                return true;
            for (int j = 0; j < m1.M; ++j)
            {
                for (int i = 0; i < m1.N; ++i)
                    if (m1[j][i] != m2[j][i])
                        return true;
            }
            return false;
        }

        public static LEMatrix operator+(LEMatrix m1, LEMatrix m2)
        {
            if (m1.M != m2.M || m1.N != m2.N)
                throw new ArgumentException("In order to add to matrixes, they must have the same dimensions.");
            var result = new MatrixRow[m1.M];
            for (int j = 0; j < m1.M; ++j)
            {
                var row = new double[m1.N];
                for (int i = 0; i < m1.N; ++i)
                    row[i] = m1[j][i] + m2[j][i];
                result[j] = new MatrixRow(row);
            }
            return new LEMatrix(result);
        }

        public static LEMatrix operator -(LEMatrix m1, LEMatrix m2)
        {
            if (m1.M != m2.M || m1.N != m2.N)
                throw new ArgumentException("In order to add to matrixes, they must have the same dimensions.");
            var result = new MatrixRow[m1.M];
            for (int j = 0; j < m1.M; ++j)
            {
                var row = new double[m1.N];
                for (int i = 0; i < m1.N; ++i)
                    row[i] = m1[j][i] - m2[j][i];
                result[j] = new MatrixRow(row);
            }
            return new LEMatrix(result);
        }

        public static LEMatrix operator *(LEMatrix m, double d)
        {
            var result = new MatrixRow[m.M];
            for (int j = 0; j < m.M; ++j)
            {
                var row = new double[m.N];
                for (int i = 0; i < m.N; ++i)
                    row[i] = m[j][i] * d;
                result[j] = new MatrixRow(row);
            }
            return new LEMatrix(result);
        }

        public static LEMatrix operator *(double d, LEMatrix m)
        {
            var result = new MatrixRow[m.M];
            for (int j = 0; j < m.M; ++j)
            {
                var row = new double[m.N];
                for (int i = 0; i < m.N; ++i)
                    row[i] = m[j][i] * d;
                result[j] = new MatrixRow(row);
            }
            return new LEMatrix(result);
        }

        public static Vector operator *(LEMatrix m, Vector v)
        {
            if (m.N != v.Count())
                throw new ArgumentException("The vector must have m.N size.");
            var result = new double[m.M];
            for (int j = 0; j < m.M; ++j)
                result[j] = m[j].ToVector().DotSum(v);
            return new Vector(result);
        }

        public static LEMatrix operator*(LEMatrix m1, LEMatrix m2)
        {
            if (m1.N != m2.M)
                throw new ArgumentException("m1.N must equal m2.N.");
            var result = new MatrixRow[m1.M];
            for (int j = 0; j < m1.M; ++j)
            {
                var row = new double[m2.N];
                for (int i = 0; i < m2.N; ++i)
                    row[i] = m1[j].ToVector().DotSum(m2.Column(i));
                result[j] = new MatrixRow(row);
            }
            return new LEMatrix(result);
        }

        public bool IsSquare()
        {
            return N == M;
        }

        public static LEMatrix UnitMatrix(int dim)
        {
            var result = new MatrixRow[dim];
            for (int j = 0; j < dim; ++j)
            {
                var row = new double[dim];
                for (int i = 0; i < dim; ++i)
                    row[i] = i == j ? 1 : 0;
                result[j] = new MatrixRow(row);
            }
            return new LEMatrix(result);
        }

        public LEMatrix SwapRows(int row1, int row2)
        {
            var result = new List<MatrixRow>(M);
            for (int i = 0; i < M; ++i)
            {
                if (i == row1)
                    result.Add(Rows[row2]);
                else if (i == row2)
                    result.Add(Rows[row1]);
                else
                    result.Add(Rows[i]);
            }
            return new LEMatrix(result);
        }

        public LEMatrix ScaleRow(int row, double scalar)
        {
            var result = new List<MatrixRow>(M);
            for (int i = 0; i < M; ++i)
            {
                if (i == row)
                    result.Add(Rows[i].Scale(scalar));
                else
                    result.Add(Rows[i]);
            }
            return new LEMatrix(result);
        }

        public LEMatrix AddScaledRow(int source, double scalar, int dest)
        {
            var result = new List<MatrixRow>(M);
            for (int i = 0; i < M; ++i)
            {
                if (i == dest)
                    result.Add(Rows[i].Add(Rows[source].Scale(scalar)));
                else
                    result.Add(Rows[i]);
            }
            return new LEMatrix(result);
        }

        public LEMatrix Transpose()
        {
            var result = new MatrixRow[N];
            for (int i = 0; i < N; ++i)
            {
                var row = new double[M];
                for (int j = 0; j < M; ++j)
                {
                    row[j] = this[j][i];
                }
                result[i] = new MatrixRow(row);
            }
            return new LEMatrix(result);
        }

        public bool IsTrap() // Optimize?
        {
            var leadingOnes = new List<(int j, int i)>();
            {
                var prevLoc = 0;
                var leaded = false;
                int j = 0;
                foreach (var mr in Rows)
                {
                    for (int i = 0; i < mr.Length; ++i)
                    {
                        if (mr[i] != 0)
                        {
                            if (mr[i] != 1)
                                return false;
                            else
                            {
                                if (i < prevLoc)
                                    return false;
                                leadingOnes.Add((j, i));
                                leaded = true;
                            }
                            break;
                        }
                    }
                    if (!leaded)
                        prevLoc = N;
                    leaded = false;
                    ++j;
                }
            }
            foreach (var lead in leadingOnes)
            {
                for (int j = 0; j < lead.j; ++j)
                {
                    if (this[j][lead.i] != 0)
                        return false;
                }
            }
            return true;
        }

        public LEMatrix ToTrap() // Optimize?
        {
            // TEST!
            var result = this;
            int m = 0;
            for (int n = 0; n < N && m < M; ++n)
            {
                //Check for zero column
                while (result.Column(n).Skip(m).All(d => d == 0))
                    continue;
                //Put one in space (m,n) 
                if (result[m][n] != 1)
                {
                    if (result.Column(n).Skip(m).Contains(1))
                    {
                        result = result.SwapRows(m, result.Column(n).Skip(m).FirstIndexOf(1) + m);
                    }
                    else
                    {
                        result = result.SwapRows(m, result.Column(n).Skip(m).FirstIndexOf(d => d != 0) + m);
                        result = result.ScaleRow(m, 1 / result[m][n]);
                    }
                }
                //Run through all rows above and set to zero.
                for (int j = 0; j < m; ++j)
                {
                    if (result[j][n] != 0)
                        result = result.AddScaledRow(m, -result[j][n], j);
                }
                //Run through all rows below and set to zero.
                for (int j = m + 1; j < M; ++j)
                {
                    if (result[j][n] != 0)
                        result = result.AddScaledRow(m, -result[j][n], j);
                }
                ++m;
            }
            return result;
        }

        public LEMatrix RemoveRow(int m)
        {
            var result = new MatrixRow[M - 1];
            for (int j = 0; j < M - 1; ++j)
            {
                result[j] = j < m ? this[j] : this[j + 1];
            }
            return new LEMatrix(result);
        }

        public LEMatrix RemoveColumn(int n)
        {
            var result = new Vector[N - 1];
            for (int i = 0; i < N - 1; ++i)
            {
                result[i] = i < n ? Column(i) : Column(i+1);
            }
            return new LEMatrix(result);
        }

        public LEMatrix MinorMatrix(int m, int n)
        {
            return RemoveColumn(n).RemoveRow(m);
        }

        public double Determinant()
        {
            var matrix = this;
            var result = 1d;
            if (!IsSquare())
                throw new InvalidOperationException("Can only find the determinant of square matrices.");
            for (int i = 0; i < N; ++i)
            {
                for (int j = i + 1; j < M; ++j)
                    matrix = matrix.AddScaledRow(i, -matrix[j][i] / (matrix[i][i]), j);
                result *= matrix[i][i];
            }
            Console.WriteLine(matrix);
            return result;
        }

        public LEMatrix Join(LEMatrix m)
        {
            return new LEMatrix(Columns.Concat(m.Columns));
        }

        public (LEMatrix left, LEMatrix right) Split(int column)
        {
            var columns = Columns;
            return (new LEMatrix(columns.Take(column)), new LEMatrix(columns.Skip(column)));
        }

        public LEMatrix Inverse()
        {
            if (!IsSquare())
                throw new ArgumentException("Only square matrices can be inverted.");
            var t1 = Join(UnitMatrix(N));
            var t2 = t1.ToTrap();
            var (left, right) = t2.Split(N);
            if (left != UnitMatrix(N))
                throw new ArgumentException("Only regular matrices can be inverted.");
            return right;
        }

        public LinearEquationSystem ToLinearEquationSystem()
        {
            return new LinearEquationSystem(Rows.Select(r => r.ToLinearEquation()));
        }

        public override string ToString()
        {
            var result = "";
            for (int j = 0; j < M; ++j)
            {
                result += "|";
                for (int i = 0; i < N; ++i)
                {
                    result += this[j][i] + " ";
                }
                result += "|";
                result += Environment.NewLine;
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj is LEMatrix m)
            {
                return this == m;
            }
            else if (obj is Vector v)
            {
                return Equals(new LEMatrix(v));
            }
            else if (obj is MatrixRow r)
            {
                return Equals(new LEMatrix(r));
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Misc.HashCode(17, 23, Values());
        }
    }
}
