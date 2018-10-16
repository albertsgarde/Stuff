using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class LinearEquation
    {
        public IReadOnlyList<double> Coefficients { get; }

        public double B { get; }

        public LinearEquation(double b, params double[] coefs)
        {
            Coefficients = coefs.Copy();
            B = b;
        }

        public LinearEquation(double b, IEnumerable<double> coefs)
        {
            Coefficients = coefs.Copy();
            B = b;
        }

        public bool IsSolution(IEnumerable<double> x)
        {
            if (x.Count() != Coefficients.Count)
                throw new ArgumentException("x must have the same length as coefficients.");
            return Coefficients.Zip(x, (a, var) => (a, var)).Sum(c => c.a * c.var) == B;
        }

        public LEMatrix.MatrixRow CoefficientMatrixRow()
        {
            return new LEMatrix.MatrixRow(Coefficients.ToArray());
        }

        public LEMatrix.MatrixRow BMatrixRow()
        {
            return new LEMatrix.MatrixRow(B);
        }

        public LEMatrix.MatrixRow ToMatrixRow()
        {
            var result = new double[Coefficients.Count + 1];
            for (int i = 0; i < Coefficients.Count; ++i)
                result[i] = Coefficients[i];
            result[Coefficients.Count] = B;
            return new LEMatrix.MatrixRow(result);
        }

        // Ligningssystem
            // Matrix
            // Parameterfremstilling
    }
}
