using Stuff.StuffMath.Expressions;
using Stuff.StuffMath.Expressions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Polynomial : IPolynomial
    {
        private readonly Dictionary<int, double> coefficients;

        /// <summary>
        /// Creates a new Polynomial with all coefficients set to 0.
        /// </summary>
        public Polynomial()
        {
            coefficients = new Dictionary<int, double>();
        }

        public Polynomial(params KeyValuePair<int, double>[] coefs)
        {
            coefficients = new Dictionary<int, double>();
            foreach (var coef in coefs)
            {
                if (coef.Key < 0)
                    throw new Exception("Polynomials cannot contain exponents below 0.");
                coefficients[coef.Key] = coef.Value;
            }
        }

        public Polynomial(Dictionary<int, double> coefs)
        {
            coefficients = new Dictionary<int, double>();
            foreach (var coef in coefs)
            {
                if (coef.Key < 0)
                    throw new Exception("Polynomials cannot contain exponents below 0.");
                coefficients[coef.Key] = coef.Value;
            }
        }

        /// <summary>
        /// Creates a new Polynomial from a string.
        /// If the string isn't perfect, bad things might happen.
        /// Sorry.
        /// Format: bx^a
        /// Where b is the coefficient, "x^" is fixed, and a is the exponent.
        /// Parts are seperated by either '+' or '-' and no spaces.
        /// </summary>
        /// <param name="s">The string from which to create the Polynomial.</param>
        public Polynomial(string s)
        {
            coefficients = new Dictionary<int, double>();
            string part = "";
            foreach (char c in s)
            {
                if (c == '+' || c == '-')
                {
                    coefficients[int.Parse(part.Substring(part.IndexOf('^') + 1))] = double.Parse(part.Substring(0, part.IndexOf('x')));
                    part = "";
                }
                part += c;
            }
            coefficients[int.Parse(part.Substring(part.IndexOf('^') + 1))] = double.Parse(part.Substring(0, part.IndexOf('x')));
        }

        public static Polynomial operator+(Polynomial pol1, Polynomial pol2)
        {
            var result = new Dictionary<int, double>();

            foreach (var coef in pol1.coefficients)
                result[coef.Key] = coef.Value;

            foreach (var coef in pol2.coefficients)
            {
                if (result.ContainsKey(coef.Key))
                    result[coef.Key] += coef.Value;
                else
                    result[coef.Key] = coef.Value;
            }

            return new Polynomial(result);
        }

        public double Y(double x)
        {
            return coefficients.Sum(coef => Math.Pow(x, coef.Key) * coef.Value);
        }

        public IPolynomial Differentiate()
        {
            var result = new Dictionary<int, double>();
            foreach (var coef in coefficients)
            {
                if (coef.Key != 0)
                    result[coef.Key - 1] = coef.Value == double.NaN ? double.NaN : coef.Value * coef.Key;
            }
            return new Polynomial(result);
        }

        public IPolynomial Integrate()
        {
            var result = new Dictionary<int, double>();
            foreach (var coef in coefficients)
            {
                result[coef.Key + 1] = coef.Value == double.NaN ? double.NaN : coef.Value / (coef.Key + 1);
            }
            result[0] = double.NaN;
            return new Polynomial(result);
        }

        public double Integrate(double a, double b)
        {
            return coefficients.Sum(coef => Math.Pow(b, coef.Key + 1) * (coef.Value / (coef.Key + 1)) - Math.Pow(b, coef.Key + 1) * (coef.Value / (coef.Key + 1)));
        }

        public double Coefficient(int exponent)
        {
            return coefficients.ContainsKey(exponent) ? coefficients[exponent] : 0;
        }

        public Polynomial AsPolynomial()
        {
            return this;
        }

        public static IPolynomial Taylor(Expression exp, int degree, string variableName, double point = 0)
        {
            Dictionary<int, double> result = new Dictionary<int, double>();
            Dictionary<string, double> vars = new Dictionary<string, double>();
            vars[variableName] = point;

            result[0] = exp.Evaluate(vars);
            long factorial = 1;
            for (int i = 1; i <= degree; i++)
                result[i] = (exp = exp.Differentiate(variableName)).Evaluate(vars) / (factorial *= i);
            return new Polynomial(result).MoveHoriz(point);
        }

        public IPolynomial MoveVertical(double k)
        {
            var result = coefficients.Copy();

            if (coefficients.ContainsKey(0))
                result[0] += k;
            else
                result[0] = k;
            return new Polynomial(result);
        }

        public IPolynomial MoveHoriz(double k)
        {
            var result = new Polynomial();

            foreach (var coef in coefficients)
            {
                var coefPol = new Dictionary<int, double>();
                for (int i = coef.Key; i >= 0; i--)
                    coefPol[i] = Basic.Factorial(coef.Key)/(Basic.Factorial(i)*Basic.Factorial(coef.Key-i))*coef.Value * Math.Pow(-k, coef.Key - i);
                result += new Polynomial(coefPol);
            }
            return result;
        }

        public IPolynomial Transform(Vector2D v)
        {
            return MoveVertical(v.Y).MoveHoriz(v.X);
        }

        public Expression ToExpression(string variableName)
        {
            return coefficients.Select(c => c.Value * new Power(new Variable(variableName), c.Key)).Aggregate((coef, result) => result + coef);
        }

        public override string ToString()
        {
            string result = "";
            foreach (var coef in coefficients.OrderByDescending(coef => coef.Key))
            {
                if (coef.Value != 0)
                    result += " " + (coef.Value > 0 ? "+ " + coef.Value : "- " + -coef.Value) + "x^" + coef.Key;
            }

            return "y =" + (result.Length > 0 ? result.Substring(2) : " 0");
        }
    }
}