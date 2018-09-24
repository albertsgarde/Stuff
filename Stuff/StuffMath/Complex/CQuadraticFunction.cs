using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Complex
{
    public class CQuadraticFunction : ICPolynomial
    {
        public Complex2D A { get; }

        public Complex2D B { get; }

        public Complex2D C { get; }

        public CQuadraticFunction(Complex2D a, Complex2D b, Complex2D c)
        {
            A = a;
            B = b;
            C = c;
        }

        public CQuadraticFunction(QuadraticFunction qd)
        {
            A = qd.A;
            B = qd.B;
            C = qd.C;
        }

        public Complex2D this[int exponent]
        {
            get
            {
                return Coefficient(exponent);
            }
        }

        public Complex2D Y(Complex2D z)
        {
            return z.Square() * A + z * B + C;
        }

        public int Degree
        {
            get
            {
                return 2;
            }
        }

        public (Complex2D, Complex2D) Roots()
        {
            var d = B.Square() - 4 * A * C;
            return ((-B + d.Root(2)) / (2 * A), (-B - d.Root(2)) / (2 * A));
        }

        public CPolynomial AsPolynomial()
        {
            return new CPolynomial((2, A), (1, B), (0, C));
        }

        public Complex2D Coefficient(int exponent)
        {
            switch (exponent)
            {
                case 0:
                    return C;
                case 1:
                    return B;
                case 2:
                    return A;
                default:
                    return 0;
            }
        }

        public IEnumerator<(int exponent, Complex2D coefficient)> GetEnumerator()
        {
            yield return (2, A);
            yield return (1, B);
            yield return (0, C);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return AsPolynomial().ToString();
        }
    }
}
