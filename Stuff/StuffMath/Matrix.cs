using Stuff.StuffMath.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Matrix<F> where F : IField<F>
    {
        public IReadOnlyList<MatrixRow<F>> Rows { get; }

        public IReadOnlyList<Vector<F>> Columns
        {
            get
            {
                var result = new F[N][];
                for (int i = 0; i < N; ++i)
                    result[i] = new F[M];
                for (int j = 0; j < M; ++j)
                {
                    for (int i = 0; i < N; ++i)
                        result[i][j] = Rows[j][i];
                }
                return result.Select(v => new Vector<F>(v)).ToList();
            }
        }

        public int M => Rows.Count();

        public int N { get; }

        public Matrix(params MatrixRow<F>[] rows)
        {
            N = rows.First().Length;
            for (int i = 1; i < rows.Length; ++i)
            {
                if (rows[i].Length != N)
                    throw new ArgumentException($"All rows must have the same length. The first row has length {N}, but row {i} has length {rows[i].Length}.");
            }
            Rows = rows.Copy();
        }

        public Matrix(IEnumerable<MatrixRow<F>> rows) : this(rows.ToArray())
        {

        }

        public Matrix(params Vector<F>[] columns)
        {
            N = columns.Length;

            var height = columns.First().Size;
            for (int i = 1; i < N; ++i)
            {
                if (columns[i].Size != height)
                    throw new ArgumentException($"All columns must have the same length. The first column has length {height}, but column {i} has length {columns[i].Size}.");
            }

            var result = new MatrixRow<F>[columns.First().Size];
            for (int j = 0; j < height; ++j)
            {
                var row = new F[N];
                for (int i = 0; i < N; ++i)
                    row[i] = columns[i][j];
                result[j] = new MatrixRow<F>(row);
            }
            Rows = result;
        }

        public Matrix(IEnumerable<Vector<F>> columns) : this(columns.ToArray())
        {
        }

        public Matrix(IEnumerable<F> values, int rows)
        {
            if (values.Count() % rows != 0)
                throw new ArgumentException($"The number of values({values.Count()}) must be divisible by the number of rows({rows}).");
            N = values.Count() / rows;
            var result = new MatrixRow<F>[rows];

            int j = -1;
            int i = 0;
            var row = new F[N];
            foreach (var value in values)
            {
                row[i] = value;
                if (++i >= N)
                {
                    i -= N;
                    result[++j] = new MatrixRow<F>(row);
                }
            }
            Rows = result;
        }

        public MatrixRow<F> this[int row] => Rows[row];

        public Vector<F> Column(int column)
        {
            var result = new F[M];
            for (int j = 0; j < M; ++j)
                result[j] = Rows[j][column];
            return new Vector<F>(result);
        }

        public IEnumerable<F> Values()
        {
            for (int j = 0; j < M; ++j)
            {
                for (int i = 0; i < N; ++i)
                    yield return this[j][i];
            }
        }

        public static bool operator ==(Matrix<F> m1, Matrix<F> m2)
        {
            if (m1.N != m2.N || m1.M != m2.M)
                return false;
            for (int j = 0; j < m1.M; ++j)
            {
                for (int i = 0; i < m1.N; ++i)
                    if (!m1[j][i].EqualTo(m2[j][i]))
                        return false;
            }
            return true;
        }

        public static bool operator !=(Matrix<F> m1, Matrix<F> m2)
        {
            return !(m1 == m2);
        }

        public static Matrix<F> operator+(Matrix<F> m1, Matrix<F> m2)
        {
            if (m1.M != m2.M || m1.N != m2.N)
                throw new ArgumentException("In order to add to matrixes, they must have the same dimensions.");
            var result = new MatrixRow<F>[m1.M];
            for (int j = 0; j < m1.M; ++j)
            {
                var row = new F[m1.N];
                for (int i = 0; i < m1.N; ++i)
                    row[i] = m1[j][i].Add(m2[j][i]);
                result[j] = new MatrixRow<F>(row);
            }
            return new Matrix<F>(result);
        }

        public static Matrix<F> operator -(Matrix<F> m1, Matrix<F> m2)
        {
            if (m1.M != m2.M || m1.N != m2.N)
                throw new ArgumentException("In order to add to matrixes, they must have the same dimensions.");
            var result = new MatrixRow<F>[m1.M];
            for (int j = 0; j < m1.M; ++j)
            {
                var row = new F[m1.N];
                for (int i = 0; i < m1.N; ++i)
                    row[i] = m1[j][i].Subtract(m2[j][i]);
                result[j] = new MatrixRow<F>(row);
            }
            return new Matrix<F>(result);
        }

        public static Matrix<F> operator *(Matrix<F> m, F d)
        {
            var result = new MatrixRow<F>[m.M];
            for (int j = 0; j < m.M; ++j)
            {
                var row = new F[m.N];
                for (int i = 0; i < m.N; ++i)
                    row[i] = m[j][i].Multiply(d);
                result[j] = new MatrixRow<F>(row);
            }
            return new Matrix<F>(result);
        }

        public static Matrix<F> operator *(F d, Matrix<F> m)
        {
            var result = new MatrixRow<F>[m.M];
            for (int j = 0; j < m.M; ++j)
            {
                var row = new F[m.N];
                for (int i = 0; i < m.N; ++i)
                    row[i] = m[j][i].Multiply(d);
                result[j] = new MatrixRow<F>(row);
            }
            return new Matrix<F>(result);
        }

        public static Vector<F> operator *(Matrix<F> m, Vector<F> v)
        {
            if (m.N != v.Count())
                throw new ArgumentException("The vector must have m.N size.");
            var result = new F[m.M];
            for (int j = 0; j < m.M; ++j)
                result[j] = m[j].ToVector().DotSum(v);
            return new Vector<F>(result);
        }

        public static Matrix<F> operator*(Matrix<F> m1, Matrix<F> m2)
        {
            if (m1.N != m2.M)
                throw new ArgumentException("m1.N must equal m2.N.");
            var result = new MatrixRow<F>[m1.M];
            for (int j = 0; j < m1.M; ++j)
            {
                var row = new F[m2.N];
                for (int i = 0; i < m2.N; ++i)
                    row[i] = m1[j].ToVector().DotSum(m2.Column(i));
                result[j] = new MatrixRow<F>(row);
            }
            return new Matrix<F>(result);
        }

        public bool IsSquare()
        {
            return N == M;
        }

        public static Matrix<F> UnitMatrix(int dim)
        {
            throw new NotImplementedException();
            /*var result = new MatrixRow<F>[dim];
            for (int j = 0; j < dim; ++j)
            {
                var row = new F[dim];
                for (int i = 0; i < dim; ++i)
                    row[i] = i == j ? 1 : 0;
                result[j] = new MatrixRow<F>(row);
            }
            return new Matrix<F>(result);*/
        }

        public Matrix<F> SwapRows(int row1, int row2)
        {
            var result = new List<MatrixRow<F>>(M);
            for (int i = 0; i < M; ++i)
            {
                if (i == row1)
                    result.Add(Rows[row2]);
                else if (i == row2)
                    result.Add(Rows[row1]);
                else
                    result.Add(Rows[i]);
            }
            return new Matrix<F>(result);
        }

        public Matrix<F> ScaleRow(int row, F scalar)
        {
            var result = new List<MatrixRow<F>>(M);
            for (int i = 0; i < M; ++i)
            {
                if (i == row)
                    result.Add(Rows[i].Scale(scalar));
                else
                    result.Add(Rows[i]);
            }
            return new Matrix<F>(result);
        }

        public Matrix<F> AddScaledRow(int source, F scalar, int dest)
        {
            var result = new List<MatrixRow<F>>(M);
            for (int i = 0; i < M; ++i)
            {
                if (i == dest)
                    result.Add(Rows[i].Add(Rows[source].Scale(scalar)));
                else
                    result.Add(Rows[i]);
            }
            return new Matrix<F>(result);
        }

        public Matrix<F> Transpose()
        {
            var result = new MatrixRow<F>[N];
            for (int i = 0; i < N; ++i)
            {
                var row = new F[M];
                for (int j = 0; j < M; ++j)
                {
                    row[j] = this[j][i];
                }
                result[i] = new MatrixRow<F>(row);
            }
            return new Matrix<F>(result);
        }

        public bool IsTrap() // Optimize?
        {
            throw new NotImplementedException();
            /*
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
            return true;*/
        }

        public Matrix<F> ToTrap() // Optimize?
        {
            throw new NotImplementedException();
            /*
            // TEST!
            var result = this;
            int m = 0;
            for (int n = 0; n < N && m < M; ++n)
            {
                Console.WriteLine(n);
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
            return result;*/
        }

        public Matrix<F> RemoveRow(int m)
        {
            var result = new MatrixRow<F>[M - 1];
            for (int j = 0; j < M - 1; ++j)
            {
                result[j] = j < m ? this[j] : this[j + 1];
            }
            return new Matrix<F>(result);
        }

        public Matrix<F> RemoveColumn(int n)
        {
            var result = new Vector<F>[N - 1];
            for (int i = 0; i < N - 1; ++i)
            {
                result[i] = i < n ? Column(i) : Column(i+1);
            }
            return new Matrix<F>(result);
        }

        public Matrix<F> MinorMatrix(int m, int n)
        {
            return RemoveColumn(n).RemoveRow(m);
        }

        public F Determinant()
        {
            throw new NotImplementedException();
            /*
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
            return result;*/
        }

        public Matrix<F> Join(Matrix<F> m)
        {
            return new Matrix<F>(Columns.Concat(m.Columns));
        }

        public (Matrix<F> left, Matrix<F> right) Split(int column)
        {
            var columns = Columns;
            return (new Matrix<F>(columns.Take(column)), new Matrix<F>(columns.Skip(column)));
        }

        public Matrix<F> Inverse()
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
            if (obj is Matrix<F> m)
            {
                return this == m;
            }
            else if (obj is Vector<F> v)
            {
                return Equals(new Matrix<F>(v));
            }
            else if (obj is MatrixRow<F> r)
            {
                return Equals(new Matrix<F>(r));
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
