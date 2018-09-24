using Stuff.StuffMath.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public interface IPolynomial
    {
        /// <param name="x">The point at which to calculate the value.</param>
        /// <returns>Returns the polynomials value at the specified point.</returns>
        double Y(double x);

        /// <returns>Returns the derivative of the polynomial</returns>
        IPolynomial Differentiate();

        /// <summary>
        /// Adds a constant with a value of double.NaN
        /// </summary>
        /// <returns>The indefinite integral of the polynomial.</returns>
        IPolynomial Integrate();

        /// <param name="a">The lower bound of the definite integral.</param>
        /// <param name="b">The upper bound of the definite integral.</param>
        /// <returns>The definite integral of the polynomial over the interval between minX and maxX.</returns>
        double Integrate(double a, double b);

        /// <param name="exponent">The exponent whose coefficient is to be returned.</param>
        /// <returns>The coefficient of the specified exponent.</returns>
        double Coefficient(int exponent);

        /// <returns>Returns a copy of the IPolynomial moved the specified amount amount up. (added the amount to the constant)</returns>
        IPolynomial MoveVertical(double k);

        /// <returns>Returns a copy of the IPolynomial moved the specified amount to the right.</returns>
        IPolynomial MoveHoriz(double k);

        /// <returns>The polynomial as an instance of the generic Polynomial class.</returns>
        Polynomial AsPolynomial();

        Expression ToExpression(string variableName);

        string ToString();
    }
}
