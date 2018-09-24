using Stuff.StuffMath.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Complex
{
    public interface ICPolynomial : IEnumerable<(int exponent, Complex2D coefficient)>
    {
        /// <param name="x">The point at which to calculate the value.</param>
        /// <returns>Returns the polynomials value at the specified point.</returns>
        Complex2D Y(Complex2D x);

        Complex2D this[int exponent]
        {
            get;
        }

        int Degree { get; }

        /// <param name="exponent">The exponent whose coefficient is to be returned.</param>
        /// <returns>The coefficient of the specified exponent.</returns>
        Complex2D Coefficient(int exponent);

        /// <returns>The polynomial as an instance of the generic Polynomial class.</returns>
        CPolynomial AsPolynomial();

        string ToString();
    }
}
