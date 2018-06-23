using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Matrix
    {
        public readonly double[] data;

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            data = new double[Rows * Columns];
        }

        public Matrix(Matrix matrix)
        {
            Rows = matrix.Rows;
            Columns = matrix.Columns;

            data = new double[Rows * Columns];
            for (int i = 0; i < data.Length; i++)
                data[i] = matrix[(i - (i % Columns))/Columns][i % Columns];
        }

        public Matrix(double[][] data)
        {
            Rows = data.Length;
            Columns = data[0].Length;

            this.data = new double[Rows * Columns];

            for (int row = 0; row < Rows; ++row)
            {
                if (data[row].Length != data[0].Length)
                    throw new ArgumentException("All rows must have the same length.");
            }

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                    this.data[row * Columns + column] = data[row][column];
            }
        }

        public Matrix(double[] data, int columns)
        {
            if (data.Length % columns != 0)
                throw new ArgumentException("Length of data array must be divisible by the number of columns.");
            Rows = data.Length/columns;
            Columns = columns;

            this.data = new double[data.Length];
            for (int i = 0; i < data.Length; i++)
                this.data[i] = data[i];
        }

        public Matrix(params MatrixRow[] matrixRows)
        {
            Rows = matrixRows.Length;
            Columns = matrixRows[0].Length;

            data = new double[Rows * Columns];

            for (int row = 0; row < Rows; ++row)
            {
                if (matrixRows[row].Length != Columns)
                    throw new ArgumentException("All MatrixRows must have the same length.");
                for (int column = 0; column < Columns; column++)
                    data[row * Columns + column] = matrixRows[row].Data[column];
            }
        }

        public double[] this[int row]
        {
            get
            {
                return data.Skip(row * Columns).Take(Columns).ToArray();
            }
        }

        public static Matrix operator +(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Rows != matrix2.Rows)
                throw new ArgumentException("Number of rows in the matrixes must be the same.");
            if (matrix1.Columns != matrix2.Columns)
                throw new ArgumentException("Number of columns in the matrixes must be the same.");
            Matrix result = new Matrix(matrix1.Rows, matrix1.Columns);
            for (int row = 0; row < matrix1.Rows; row++)
            {
                for (int column = 0; column < matrix1.Columns; column++)
                    result[row][column] = matrix1[row][column] + matrix2[row][column];
            }
            return result;
        }

        public static Matrix operator -(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Rows != matrix2.Rows)
                throw new ArgumentException("Number of rows in the matrixes must be the same.");
            if (matrix1.Columns != matrix2.Columns)
                throw new ArgumentException("Number of columns in the matrixes must be the same.");
            Matrix result = new Matrix(matrix1.Rows, matrix1.Columns);
            for (int row = 0; row < matrix1.Rows; row++)
            {
                for (int column = 0; column < matrix1.Columns; column++)
                    result[row][column] = matrix1[row][column] - matrix2[row][column];
            }
            return result;
        }

        public static Matrix operator *(Matrix matrix1, Matrix matrix2)
        {
            if (matrix1.Columns != matrix2.Rows)
                throw new ArgumentException("Number of columns in the first matrix must equal the number of rows in the second.");
            Matrix result = new Matrix(matrix1.Rows, matrix2.Columns);
            for (int row = 0; row < matrix1.Rows; row++)
            {
                for (int column = 0; column < matrix2.Columns; column++)
                {
                    double value = 0;
                    for (int i = 0; i < matrix1.Columns; i++)
                        value += matrix1[row][i] * matrix2[i][column];
                    result[row][column] = value;
                }
            }
            return result;
        }

        public static Vector operator *(Matrix matrix, Vector vec)
        {
            if (matrix.Columns != vec.Size)
                throw new ArgumentException("Number of columns in the matrix must equal the size of the vector.");
            double[] result = new double[matrix.Rows];
            for (int row = 0; row < matrix.Rows; row++)
            {
                double value = 0;
                for (int i = 0; i < matrix.Columns; i++)
                    value += matrix[row][i] * vec[i];
                result[row] = value;
            }
            return new Vector(result);
        }

        public static BitArray operator *(Matrix matrix, BitArray bits)
        {
            if (matrix.Columns != bits.Length)
                throw new ArgumentException("Number of columns in the matrix must equal the size of the BitArray.");
            for (int i = 0; i < matrix.data.Length; i++)
            {
                if (matrix.data[i] % 1 != 0)
                    throw new Exception("A cross product between a BitArray and a Matrix is only valid when the Matrix constists of only whole numbers.");
            }
            bool[] result = new bool[matrix.Rows];
            for (int row = 0; row < matrix.Rows; row++)
            {
                double value = 0;
                for (int i = 0; i < matrix.Columns; i++)
                    value += bits[i] ? matrix[row][i] : 0;
                result[row] = value % 2 == 1;
            }
            return new BitArray(result);
        }

        public static Matrix operator *(Matrix matrix, double d)
        {
            Matrix result = new Matrix(matrix.Rows, matrix.Columns);
            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int column = 0; column < matrix.Columns; column++)
                    result[row][column] = matrix[row][column] * d;
            }
            return result;
        }

        public static Matrix operator /(Matrix matrix, double d)
        {
            Matrix result = new Matrix(matrix.Rows, matrix.Columns);
            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int column = 0; column < matrix.Columns; column++)
                    result[row][column] = matrix[row][column] / d;
            }
            return result;
        }

        public double Determinant()
        {
            if (Rows != Columns)
                throw new ArgumentException("To find the determinant of a matrix, it must be square.");

            double result = 0;
            for(int column = 0; column < Columns; column++)
            {
                double posColumnTotal = 1;
                for (int row = 0; row < Rows; row++)
                    posColumnTotal *= data[(column + row) % Columns];
                result += posColumnTotal;

                double negColumnTotal = 1;
                for (int row = 0; row < Rows; row++)
                    negColumnTotal *= data[row * Columns + (column - row + Columns) % Columns];
                result -= negColumnTotal;
            }
            return result;
        }

        public Matrix RemoveRows(params int[] rows)
        {
            Matrix result = new Matrix(Rows - rows.Length, Columns);
            int resultRow = 0;
            for (int row = 0; row < Rows; row++)
            {
                if (!rows.Contains(row))
                {
                    for (int column = 0; column < Columns; column++)
                        result[resultRow][column] = data[row * Columns + column];
                    resultRow++;
                }
            }
            return result;
        }

        public Matrix RemoveColumns(params int[] columns)
        {
            Matrix result = new Matrix(Rows, Columns - columns.Length);
            int resultColumn = 0;
            for (int row = 0; row < Rows; row++)
            {
                    for (int column = 0; column < Columns; column++)
                    {
                        if (!columns.Contains(column))
                        {
                            result[row][resultColumn] = data[row * Columns + column];
                            resultColumn++;
                        }
                    }
            }
            return result;
        }

        public Matrix MinorMatrix(int removeRow, int removeColumn)
        {
            Matrix result = new Matrix(Rows - 1, Columns - 1);
            int resultRow = 0;
            for (int row = 0; row < result.Rows; row++)
            {
                if (row != removeRow)
                {
                    int resultColumn = 0;
                    for (int column = 0; column < result.Columns; column++)
                    {
                        if (column != removeColumn)
                        {
                            result[resultRow][resultColumn] = data[row * Columns + column];
                            resultColumn++;
                        }
                    }
                    resultRow++;
                }
            }
            return result;
        }

        public double Minor(int row, int column)
        {
            if (Rows != Columns)
                throw new ArgumentException("To find the minor of a matrix, it must be square.");
            return MinorMatrix(row, column).Determinant();
        }

        public Matrix MatrixOfMinors()
        {
            if (Rows != Columns)
                throw new ArgumentException("To find the matrix of minors of a matrix, it must be square.");
            Matrix result = new Matrix(Rows, Columns);
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                    result[row][column] = Minor(row, column);
            }
            return result;
        }

        public Matrix MatrixOfCofactors()
        {
            Matrix result = new Matrix(Rows, Columns);
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                    result[row][column] = data[row * Columns + column] * (row + column%2 == 1 ? -1 : 1);
            }
            return result;
        }

        public Matrix SwapRows(int row1, int row2)
        {
            double[][] resultData = new double[Rows][];
            for (int i = 0; i < Rows; i++)
            {
                if (i == row1)
                    resultData[i] = this[row2];
                else if (i == row2)
                    resultData[i] = this[row1];
                else
                    resultData[i] = this[i];
            }
            return new Matrix(resultData);
        }

        public Matrix AddByMultiple(int sourceRow, double multiple, int dest)
        {
            double[] resultData = data;
            for (int i = 0; i < Columns; i++)
                resultData[dest * Columns + i] = resultData[dest * Columns + i] + resultData[sourceRow * Columns + i] * multiple;
            return new Matrix(resultData, Columns);
        }

        public Matrix MultiplyRow(int row, double multiple)
        {
            double[] resultData = data;
            for (int i = 0; i < Columns; i++)
                resultData[row * Columns + i] = resultData[row * Columns + i] * multiple;
            return new Matrix(resultData, Columns);
        }

        public bool IsEqual(Matrix m)
        {
            if (m.Rows != Rows || m.Columns != Columns)
                return false;
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != m.data[i])
                    return false;
            }
            return true;
        }

        /*public Matrix Inverse()
        {
            if (Rows != Columns)
                throw new ArgumentException("To invert a matrix, it must be square.");


        }*/
    }
}
