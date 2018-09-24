using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.StuffMath.Complex;

namespace Stuff.StuffMath
{
    public static class Extensions
    {
        /// <returns>Returns a copy of the IPolynomial moved by the specified vector.</returns>
        public static IPolynomial Transform(this IPolynomial polynomial, Vector2D vec)
        {
            return polynomial.MoveHoriz(vec.X).MoveVertical(vec.Y);
        }

        public static Complex2D Sum(this IEnumerable<Complex2D> source)
        {
            return source.Aggregate(new Complex2D(0, 0), (sum, c) => sum + c);
        }
    }
}
