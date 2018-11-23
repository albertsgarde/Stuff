using Stuff.StuffMath.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Matrix<F> where F : IHilbertField<F>, new()
    {
        private readonly F F0 = new F();

        private readonly F F1 = new F().ONE;

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

        public Matrix(F[][] rows)
        {
            if (rows.Length == 0)
                throw new ArgumentException("Matrices cannot have 0 dimensions.");
            N = rows[0].Length;
            if (N == 0)
                throw new ArgumentException("Matrices cannot have 0 dimensions.");
            for (int i = 1; i < rows.Length; ++i)
            {
                if (rows[i].Length != N)
                    throw new ArgumentException($"All rows must have the same length. The first row has length {N}, but row {i} has length {rows[i].Length}.");
            }
            Rows = rows.Select(r => new MatrixRow<F>(r)).ToList();
        }

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
            {
                result[j] = m[j].ToVector().Zip(v, (a, b) => (a, b)).Aggregate(new F(), (a, f) => a.Add(f.a.Multiply(f.b)));
            }
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
                    row[i] = m1[j].ToVector().Zip(m2.Column(i), (a, b) => (a, b)).Aggregate(new F(), (a, f) => a.Add(f.a.Multiply(f.b)));
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
            var result = new MatrixRow<F>[dim];
            for (int j = 0; j < dim; ++j)
            {
                var row = new F[dim];
                for (int i = 0; i < dim; ++i)
                    row[i] = i == j ? new F().ONE : new F();
                result[j] = new MatrixRow<F>(row);
            }
            return new Matrix<F>(result);
        }

        public Matrix<F> UnitMatrix()
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

        public static Matrix<F> DiagonalMatrix(IEnumerable<F> values)
        {
            var dim = values.Count();
            var result = ContainerUtils.UniformArray(ContainerUtils.UniformArray(new F(), dim), dim);
            int i = 0;
            foreach (var f in values)
            {
                result[i][i] = f;
                ++i;
            }
            return new Matrix<F>(result);
        }

        public static Matrix<F> DiagonalMatrix(params F[] values) => DiagonalMatrix(values.AsEnumerable());

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
                    if (!this[j][lead.i].EqualTo(new F()))
                        return false;
                }
            }
            return true;
        }

        public Matrix<F> ToTrap() // Optimize?
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
                    if (result.Column(n).Skip(m).Contains(F1))
                    {
                        result = result.SwapRows(m, result.Column(n).Skip(m).FirstIndexOf(F1) + m);
                    }
                    else
                    {
                        result = result.SwapRows(m, result.Column(n).Skip(m).FirstIndexOf(d => !d.IsZero()) + m);
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
            var result = new F();
            if (!IsSquare())
                throw new InvalidOperationException("Can only find the determinant of square matrices.");
            if (N == 2)
                return this[0][0].Multiply(this[1][1]).Subtract(this[0][1].Multiply(this[1][0]));
            for (int i = 0; i < N; ++i)
            {
                if (!this[0][i].IsZero())
                {
                    if (i % 2 == 0)
                        result = result.Add(MinorMatrix(0, i).Determinant().Multiply(this[0][i]));
                    else
                        result = result.Add(MinorMatrix(0, i).Determinant().Multiply(this[0][i]).AdditiveInverse());
                }
            }
            return result;
        }

        public F Trace()
        {
            if (!IsSquare())
                throw new InvalidOperationException("Only a square matrix has a trace.");
            var result = new F();
            for (int i = 0; i < N; ++i)
                result.Add(this[i][i]);
            return result;
        }

        public ParametricEquation<F> Kernel()
        {
            var result = new List<Vector<F>>();
            var trap = ToTrap();
            var rows = new List<int>();
            int m = 0;
            for (int n = 0; n < N; ++n)
            {
                if (trap.Column(n).IsNull())
                    result.Add(Vector<F>.UnitVector(N, n));
                else if (m < trap.M && trap[m][n].IsOne())
                {
                    rows.Add(n);
                    ++m;
                }
                else
                {
                    var vector = ContainerUtils.UniformArray(new F(), N);
                    vector[n] = new F().ONE;
                    for (int i = 0; i < rows.Count; ++i)
                        vector[rows[i]] = trap[i][n].AdditiveInverse();
                    result.Add(new Vector<F>(vector));
                }
            }
            return new ParametricEquation<F>(Vector<F>.NullVector(N), result);
        }

        public IEnumerable<Vector<F>> EigenVectors(F eigenValue)
        {
            if (!IsSquare())
                throw new InvalidOperationException("Only a square matrix has eigen values or vectors.");
            return (this - UnitMatrix() * eigenValue).Kernel().Coefficients;
        }

        public F EigenValue(Vector<F> eigenVector)
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
