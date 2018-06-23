﻿using Stuff.StuffMath.Expressions;
using Stuff.StuffMath.Expressions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class CubicFunction : IPolynomial
    {
        public double A { get; private set; }

        public double B { get; private set; }

        public double C { get; private set; }

        public double D { get; private set; }

        public CubicFunction(double a, double b, double c, double d)
        {
            A = a;
            B = b;
            C = c;
            D = d;
        }

        public double Y(double x)
        {
            return A * x * x * x + B * x * x + C * x + D;
        }

        public IPolynomial Differentiate()
        {
            return new QuadraticFunction(A * 3, B * 2, C);
        }
        
        public IPolynomial Integrate()
        {
            return new Polynomial(new KeyValuePair<int, double>(4, A / 4), new KeyValuePair<int, double>(3, B / 3), new KeyValuePair<int, double>(2, C / 2), new KeyValuePair<int, double>(1, D), new KeyValuePair<int, double>(0, double.NaN));
        }

        public double Integrate(double a, double b)
        {
            return Math.Pow(a, 4) * A / 4 - Math.Pow(b, 4) * A / 4 + Math.Pow(a, 3) * B / 3 - Math.Pow(b, 3) * B / 3 + Math.Pow(a, 2) * C / 2 - Math.Pow(b, 2) * C / 2 + a * D - b * D;
        }

        public double Coefficient(int exponent)
        {
            switch (exponent)
            {
                case 0:
                    return D;
                case 1:
                    return C;
                case 2:
                    return B;
                case 3:
                    return A;
                default:
                    return 0;
            }
        }

        public Polynomial AsPolynomial()
        {
            return new Polynomial(new KeyValuePair<int, double>(3, A), new KeyValuePair<int, double>(2, B), new KeyValuePair<int, double>(1, C), new KeyValuePair<int, double>(0, D));
        }

        public IPolynomial MoveVertical(double k)
        {
            return new CubicFunction(A, B, C, D + k);
        }

        public IPolynomial MoveHoriz(double k)
        {
            k = -k;
            return new CubicFunction(A, 3 * A * k + B, 3 * A * k * k + 2 * B * k + C, A * k * k * k + B * k * k + C * k + D);
        }

        public IPolynomial Transform(Vector2D v)
        {
            return MoveVertical(v.Y).MoveHoriz(v.X);
        }

        public Expression ToExpression(string variableName)
        {
            return A * new Power(new Variable(variableName), 3) + B * new Power(new Variable(variableName), 2) + C * new Variable(variableName) + D;
        }

        public override string ToString()
        {
            return "y = " + A + "x^3 " + (B >= 0 ? "+ " + B : "- " + B * -1) + "x^2 " + (C >= 0 ? "+ " + C : "- " + C * -1) + "x " + (D >= 0 ? "+ " + D : "- " + D * -1); 
        }
    }
}
