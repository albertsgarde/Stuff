using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Matrix2D
    {
        /// <summary>
        /// [row][column]
        /// </summary>
        public readonly double[][] data;

        public static readonly Matrix2D IDENTITY = new Matrix2D(new Vector2D(1, 0), new Vector2D(0, 1));

        public Matrix2D(Vector2D basisVectorX, Vector2D basisVectorY)
        {
            data = new double[][] { new double[] { basisVectorX.X, basisVectorY.X }, new double[] { basisVectorX.Y, basisVectorY.Y } };
        }

        public Matrix2D(double[][] data)
        {
            this.data = data;
        }

        public double[][] Data
        {
            get
            {
                return data;
            }
        }

        public Vector2D BasisVectorX
        {
            get
            {
                return new Vector2D(data[0][0], data[1][0]);
            }
        }

        public Vector2D BasisVectorY
        {
            get
            {
                return new Vector2D(data[0][1], data[1][1]);
            }
        }

        public static Matrix2D operator*(Matrix2D m1, Matrix2D m2)
        {
            return new Matrix2D(m2.BasisVectorX.ApplyMatrix(m1), m2.BasisVectorY.ApplyMatrix(m1));
        }

        public double Determinant()
        {
            return BasisVectorX.Determinant(BasisVectorY);
        }

        public static Matrix2D Rotation(double radians)
        {
            var i = new Vector2D(Math.Cos(radians), Math.Sin(radians));
            var j = new Vector2D(-Math.Sin(radians), Math.Cos(radians));
            return new Matrix2D(i, j);
        }
    }
}
