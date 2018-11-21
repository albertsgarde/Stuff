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
    public class Matrix
    {
        private readonly Complex2D Complex2D0 = new Complex2D();

        private readonly Complex2D Complex2D1 = new Complex2D().ONE;

        public IReadOnlyList<MatrixRow> Rows { get; }

        public IReadOnlyList<Vector> Columns
        {
            get
            {
                var result = new Complex2D[N][];
                for (int i = 0; i < N; ++i)
                    result[i] = new Complex2D[M];
                for (int j = 0; j < M; ++j)
                {
                    for (int i = 0; i < N; ++i)
                        result[i][j] = Rows[j][i];
                }
                return result.Select(v => new Vector(v)).ToList();
            }
        }

        public int M => Rows.Count();

        public int N { get; }

        public Matrix(params MatrixRow[] rows)
        {
            N = rows.Complex2Dirst().Length;
            for (int i = 1; i < rows.Length; ++i)
            {
                if (rows[i].Length != N)
                    throw new ArgumentException($"All rows must have the same length. The first row has length {N}, but row {i} has length {rows[i].Length}.");
            }
            Rows = rows.Copy();
        }

        public Matrix(IEnumerable<MatrixRow> rows) : this(rows.ToArray())
        {

        }

        public Matrix(params Vector[] columns)
        {
            N = columns.Length;

            var height = columns.Complex2Dirst().Size;
            for (int i = 1; i < N; ++i)
            {
                if (columns[i].Size != height)
                    throw new ArgumentException($"All columns must have the same length. The first column has length {height}, but column {i} has length {columns[i].Size}.");
            }

            var result = new MatrixRow[columns.Complex2Dirst().Size];
            for (int j = 0; j < height; ++j)
            {
                var row = new Complex2D[N];
                for (int i = 0; i < N; ++i)
                    row[i] = columns[i][j];
                result[j] = new MatrixRow(row);
            }
            Rows = result;
        }

        public Matrix(IEnumerable<Vector> columns) : this(columns.ToArray())
        {
        }

        public Matrix(IEnumerable<Complex2D> values, int rows)
        {
            if (values.Count() % rows != 0)
                throw new ArgumentException($"The number of values({values.Count()}) must be divisible by the number of rows({rows}).");
            N = values.Count() / rows;
            var result = new MatrixRow[rows];

            int j = -1;
            int i = 0;
            var row = new Complex2D[N];
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
            var result = new Complex2D[M];
            for (int j = 0; j < M; ++j)
                result[j] = Rows[j][column];
            return new Vector(result);
        }

        public IEnumerable<Complex2D> Values()
        {
            for (int j = 0; j < M; ++j)
            {
                for (int i = 0; i < N; ++i)
                    yield return this[j][i];
            }
        }

        public static bool operator ==(Matrix m1, Matrix m2)
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

        public static bool operator !=(Matrix m1, Matrix m2)
        {
            return !(m1 == m2);
        }

        public static Matrix operator+(Matrix m1, Matrix m2)
        {
            if (m1.M != m2.M || m1.N != m2.N)
                throw new ArgumentException("In order to add to matrixes, they must have the same dimensions.");
            var result = new MatrixRow[m1.M];
            for (int j = 0; j < m1.M; ++j)
            {
                var row = new Complex2D[m1.N];
                for (int i = 0; i < m1.N; ++i)
                    row[i] = m1[j][i].Add(m2[j][i]);
                result[j] = new MatrixRow(row);
            }
            return new Matrix(result);
        }

        public static Matrix operator -(Matrix m1, Matrix m2)
        {
            if (m1.M != m2.M || m1.N != m2.N)
                throw new ArgumentException("In order to add to matrixes, they must have the same dimensions.");
            var result = new MatrixRow[m1.M];
            for (int j = 0; j < m1.M; ++j)
            {
                var row = new Complex2D[m1.N];
                for (int i = 0; i < m1.N; ++i)
                    row[i] = m1[j][i].Subtract(m2[j][i]);
                result[j] = new MatrixRow(row);
            }
            return new Matrix(result);
        }

        public static Matrix operator *(Matrix m, Complex2D d)
        {
            var result = new MatrixRow[m.M];
            for (int j = 0; j < m.M; ++j)
            {
                var row = new Complex2D[m.N];
                for (int i = 0; i < m.N; ++i)
                    row[i] = m[j][i].Multiply(d);
                result[j] = new MatrixRow(row);
            }
            return new Matrix(result);
        }

        public static Matrix operator *(Complex2D d, Matrix m)
        {
            var result = new MatrixRow[m.M];
            for (int j = 0; j < m.M; ++j)
            {
                var row = new Complex2D[m.N];
                for (int i = 0; i < m.N; ++i)
                    row[i] = m[j][i].Multiply(d);
                result[j] = new MatrixRow(row);
            }
            return new Matrix(result);
        }

        public static Vector operator *(Matrix m, Vector v)
        {
            if (m.N != v.Count())
                throw new ArgumentException("The vector must have m.N size.");
            var result = new Complex2D[m.M];
            for (int j = 0; j < m.M; ++j)
                result[j] = m[j].ToVector().DotSum(v);
            return new Vector(result);
        }

        public static Matrix operator*(Matrix m1, Matrix m2)
        {
            if (m1.N != m2.M)
                throw new ArgumentException("m1.N must equal m2.N.");
            var result = new MatrixRow[m1.M];
            for (int j = 0; j < m1.M; ++j)
            {
                var row = new Complex2D[m2.N];
                for (int i = 0; i < m2.N; ++i)
                    row[i] = m1[j].ToVector().DotSum(m2.Column(i));
                result[j] = new MatrixRow(row);
            }
            return new Matrix(result);
        }

        public bool IsSquare()
        {
            return N == M;
        }

        public static Matrix UnitMatrix(int dim)
        {
            var result = new MatrixRow[dim];
            for (int j = 0; j < dim; ++j)
            {
                var row = new Complex2D[dim];
                for (int i = 0; i < dim; ++i)
                    row[i] = i == j ? new Complex2D().ONE : new Complex2D();
                result[j] = new MatrixRow(row);
            }
            return new Matrix(result);
        }

        public Matrix UnitMatrix()
        {
            if (!IsSquare())
                throw new InvalidOperationException("Unit matrices must be square.");
            return UnitMatrix(N);
        }

        public bool IsUnitMatrix()
        {
            if (!IsSquare())
                return false;
            var result = true;
            for (int j = 0; j < N; ++j)
            {
                for (int i = 0; i < N; ++i)
                {
                    if (!(i == j && this[j][i].IsOne() || i != j && this[j][i].IsZero()))
                        return false;
                }
            }
            return result;
        }

        public Matrix SwapRows(int row1, int row2)
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
            return new Matrix(result);
        }

        public Matrix ScaleRow(int row, Complex2D scalar)
        {
            var result = new List<MatrixRow>(M);
            for (int i = 0; i < M; ++i)
            {
                if (i == row)
                    result.Add(Rows[i].Scale(scalar));
                else
                    result.Add(Rows[i]);
            }
            return new Matrix(result);
        }

        public Matrix AddScaledRow(int source, Complex2D scalar, int dest)
        {
            var result = new List<MatrixRow>(M);
            for (int i = 0; i < M; ++i)
            {
                if (i == dest)
                    result.Add(Rows[i].Add(Rows[source].Scale(scalar)));
                else
                    result.Add(Rows[i]);
            }
            return new Matrix(result);
        }

        public Matrix Transpose()
        {
            var result = new MatrixRow[N];
            for (int i = 0; i < N; ++i)
            {
                var row = new Complex2D[M];
                for (int j = 0; j < M; ++j)
                {
                    row[j] = this[j][i];
                }
                result[i] = new MatrixRow(row);
            }
            return new Matrix(result);
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
                        if (!(mr[i].IsZero()))
                        {
                            if (!(mr[i].IsOne()))
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
                    if (!this[j][lead.i].EqualTo(new Complex2D()))
                        return false;
                }
            }
            return true;
        }

        public Matrix ToTrap() // Optimize?
        {
            // TEST!
            var result = this;
            int m = 0;
            for (int n = 0; n < N && m < M; ++n)
            {

                //Check for zero column
                if (result.Column(n).Skip(m).All(d => d.IsZero()))
                    continue;
                //Put one in space (m,n) 
                if (!result[m][n].IsOne())
                {
                    if (result.Column(n).Skip(m).Contains(Complex2D1))
                    {
                        result = result.SwapRows(m, result.Column(n).Skip(m).Complex2DirstIndexOf(Complex2D1) + m);
                    }
                    else
                    {
                        result = result.SwapRows(m, result.Column(n).Skip(m).Complex2DirstIndexOf(d => !d.IsZero()) + m);
                        result = result.ScaleRow(m, result[m][n].MultiplicativeInverse());
                    }
                }
                //Run through all rows above and set to zero.
                for (int j = 0; j < m; ++j)
                {
                    if (!result[j][n].IsZero())
                        result = result.AddScaledRow(m, result[j][n].AdditiveInverse(), j);
                }
                //Run through all rows below and set to zero.
                for (int j = m + 1; j < M; ++j)
                {
                    if (!result[j][n].IsZero())
                        result = result.AddScaledRow(m, result[j][n].AdditiveInverse(), j);
                }
                ++m;
            }
            return result;
        }

        public Matrix RemoveRow(int m)
        {
            var result = new MatrixRow[M - 1];
            for (int j = 0; j < M - 1; ++j)
            {
                result[j] = j < m ? this[j] : this[j + 1];
            }
            return new Matrix(result);
        }

        public Matrix RemoveColumn(int n)
        {
            var result = new Vector[N - 1];
            for (int i = 0; i < N - 1; ++i)
            {
                result[i] = i < n ? Column(i) : Column(i+1);
            }
            return new Matrix(result);
        }

        public Matrix MinorMatrix(int m, int n)
        {
            return RemoveColumn(n).RemoveRow(m);
        }

        public Complex2D Determinant()
        {
            var result = new Complex2D();
            if (!IsSquare())
                throw new InvalidOperationException("Can only find the determinant of square matrices.");
            if (N == 2)
                return this[0][0].Multiply(this[1][1]).Subtract(this[0][1].Multiply(this[1][0]));
            for (int i = 0; i < N; ++i)
                result = result.Add(MinorMatrix(0, i).Determinant());
            return result;
        }

        public Complex2D Trace()
        {
            if (!IsSquare())
                throw new InvalidOperationException("Only a square matrix has a trace.");
            var result = new Complex2D();
            for (int i = 0; i < N; ++i)
                result.Add(this[i][i]);
            return result;
        }

        public ParametricEquation<Complex2D> Kernel()
        {
            var result = new List<Vector>();
            var trap = ToTrap();
            var rows = new List<int>();
            int m = 0;
            for (int n = 0; n < N; ++n)
            {
                if (trap.Column(n).IsNull())
                    result.Add(Vector.UnitVector(N, n));
                else if (m < trap.M && trap[m][n].IsOne())
                {
                    rows.Add(n);
                    ++m;
                }
                else
                {
                    var vector = ContainerUtils.UniformArray(new Complex2D(), N);
                    vector[n] = new Complex2D().ONE;
                    for (int i = 0; i < rows.Count; ++i)
                        vector[rows[i]] = trap[i][n].AdditiveInverse();
                    result.Add(new Vector(vector));
                }
            }
            return new ParametricEquation<Complex2D>(Vector.NullVector(N), result);
        }

        public IEnumerable<Vector> EigenVectors(Complex2D eigenValue)
        {
            if (!IsSquare())
                throw new InvalidOperationException("Only a square matrix has eigen values or vectors.");
            return (this - UnitMatrix() * eigenValue).Kernel().Coefficients;
        }

        public Complex2D EigenValue(Vector eigenVector)
        {
            if (!IsSquare())
                throw new InvalidOperationException("Only a square matrix has eigen values or vectors.");
            if (eigenVector.Size != N)
                throw new ArgumentException("An eigen vector always has the same dimensions as its matrix.");
            if (eigenVector.IsNull())
                throw new ArgumentException("Null vectors cannot be eigenvectors.");
            var result = this * eigenVector;
            if (!eigenVector.LinearlyDependent(result))
                throw new ArgumentException("Non-eigenvectors don't have eigenvalues.");
            return result[0].Divide(eigenVector[0]);
        }

        public Matrix Join(Matrix m)
        {
            return new Matrix(Columns.Concat(m.Columns));
        }

        public (Matrix left, Matrix right) Split(int column)
        {
            var columns = Columns;
            return (new Matrix(columns.Take(column)), new Matrix(columns.Skip(column)));
        }

        public Matrix Inverse()
        {
            if (!IsSquare())
                throw new ArgumentException("Only square matrices can be inverted.");
            var t1 = Join(UnitMatrix(N));
            var t2 = t1.ToTrap();
            var (left, right) = t2.Split(N);
            if (!left.IsUnitMatrix())
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
            if (obj is Matrix m)
            {
                return this == m;
            }
            else if (obj is Vector v)
            {
                return Equals(new Matrix(v));
            }
            else if (obj is MatrixRow r)
            {
                return Equals(new Matrix(r));
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
