using Stuff.StuffMath.Expressions;
using Stuff.StuffMath.Expressions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.StuffMath.Complex;
using Stuff.StuffMath.Structures;
using System.Collections;

namespace Stuff.StuffMath
{
    public class QuadraticFunction : IPolynomial
    {
        public double A { get; private set; }

        public double B { get; private set; }

        public double C { get; private set; }

        public int Degree => 2;

        public IPolynomial ZERO => NULL;

        private static readonly QuadraticFunction NULL = new QuadraticFunction(0, 0, 0);

        public Real ONE => 1;

        public double this[int exponent] => Coefficient(exponent);

        public QuadraticFunction(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        public static bool operator ==(QuadraticFunction qf1, QuadraticFunction qf2)
        {
            return qf1.A == qf2.A && qf1.B == qf2.B && qf1.C == qf2.C;
        }

        public static bool operator !=(QuadraticFunction qf1, QuadraticFunction qf2)
        {
            return qf1.A != qf2.A || qf1.B != qf2.B || qf1.C != qf2.C;
        }

        public double Y(double x)
        {
            return A * x * x + B * x + C;
        }

        public  IPolynomial Differentiate()
        {
            return new LinearFunction(A * 2, B);
        }

        public IPolynomial Integrate()
        {
            return new CubicFunction(A/3, B/2, C, double.NaN);
        }

        public double Integrate(double a, double b)
        {
            return Math.Pow(a, 3) * A / 3 - Math.Pow(b, 3) * A / 3 + Math.Pow(a, 2) * B / 2 - Math.Pow(b, 2) * B / 2 + a * C - b * C;
        }

        public double Coefficient(int exponent)
        {
            switch(exponent)
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

        public Polynomial AsPolynomial()
        {
            return new Polynomial(new KeyValuePair<int, double>(2, A), new KeyValuePair<int, double>(1, B), new KeyValuePair<int, double>(0, C));
        }

        public IPolynomial MoveVertical(double k)
        {
            return new QuadraticFunction(A, B, C + k);
        }

        public IPolynomial MoveHoriz(double k)
        {
            k = -k;
            return new QuadraticFunction(A, 2 * A * k + B, A * k * k + B * k + C);
        }

        public IPolynomial Transform(Vector2D v)
        {
            return MoveVertical(v.Y).MoveHoriz(v.X);
        }

        public Expression ToExpression(string variableName)
        {
            return A * new Power(new Variable(variableName), 2) + B * new Variable(variableName) + C;
        }

        public double[] Roots()
        {
            double d = Math.Pow(B, 2) - 4 * A * C;
            if (d < 0)
                return new double[0];
            else if (d == 0)
                return new double[] { (Math.Sqrt(d) - B) / (2 * A)};
            else
                return new double[] { (Math.Sqrt(d) - B) / (2 * A), (-Math.Sqrt(d) - B) / (2 * A) };
        }

        public Complex2D[] Complex2DRoots()
        {
            double d = Math.Pow(B, 2) - 4 * A * C;
            return new Complex2D[] { (Complex2D.Sqrt(Math.Pow(B, 2) - 4 * A * C) - B) / (2 * A), (-Complex2D.Sqrt(Math.Pow(B, 2) - 4 * A * C) - B) / (2 * A) };
        }

        public Location2D TopPoint()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "y = " + A + "x^2 " + (B >= 0 ? "+ " + B : "- " + B * -1) + "x " + (C >= 0 ? "+ " + C : "- " + C * -1); 
        }

        public override bool Equals(object obj)
        {
            return obj is IPolynomial p && AsPolynomial() == p;
        }

        public override int GetHashCode()
        {
            return Misc.HashCode(17, 23, A, B, C);
        }

        public IEnumerator<double> GetEnumerator()
        {
            yield return C;
            yield return B;
            yield return A;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IPolynomial Add(IPolynomial t)
        {
            return AsPolynomial() + t.AsPolynomial();
        }

        public IPolynomial AdditiveInverse()
        {
            return new QuadraticFunction(-A, -B, -C);
        }

        public IPolynomial Multiply(Real s)
        {
            return new QuadraticFunction(A * (double)s, B * (double)s, C * (double)s);
        }

        public bool EqualTo(IPolynomial t)
        {
            return Equals(t);
        }
    }
}
