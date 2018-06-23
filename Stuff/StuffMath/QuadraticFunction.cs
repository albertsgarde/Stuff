using Stuff.StuffMath.Expressions;
using Stuff.StuffMath.Expressions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class QuadraticFunction : IPolynomial
    {
        public double A { get; private set; }

        public double B { get; private set; }

        public double C { get; private set; }

        public QuadraticFunction(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
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
                return new double[] { (Math.Sqrt(Math.Pow(B, 2) - 4 * A * C) - B) / (2 * A)};
            else
                return new double[] { (Math.Sqrt(Math.Pow(B, 2) - 4 * A * C) - B) / (2 * A), (-Math.Sqrt(Math.Pow(B, 2) - 4 * A * C) - B) / (2 * A) };
        }

        public Complex[] ComplexRoots()
        {
            double d = Math.Pow(B, 2) - 4 * A * C;
            return new Complex[] { (Complex.Sqrt(Math.Pow(B, 2) - 4 * A * C) - B) / (2 * A), (-Complex.Sqrt(Math.Pow(B, 2) - 4 * A * C) - B) / (2 * A) };

        }

        public override string ToString()
        {
            return "y = " + A + "x^2 " + (B >= 0 ? "+ " + B : "- " + B * -1) + "x " + (C >= 0 ? "+ " + C : "- " + C * -1); 
        }
    }
}
