using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Complex
{
    public class CPolynomial : ICPolynomial
    {
        public ImmutableDictionary<int, Complex2D> Coefficients { get; }

        public CPolynomial()
        {
            Coefficients = new Dictionary<int, Complex2D>().ToImmutableDictionary();
        }

        public CPolynomial(params (int power, Complex2D coef)[] coefficients)
        {
            
            Coefficients = coefficients.ToImmutableDictionary(c => c.power, c => c.coef);
        }

        public CPolynomial(IReadOnlyDictionary<int, Complex2D> coefficients)
        {
            Coefficients = coefficients.ToImmutableDictionary(); ;
        }

        public Complex2D Y(Complex2D x)
        {
            return Coefficients.Select(coef => coef.Value * x.Power(coef.Key)).Sum();
        }

        public Complex2D this[int exponent] => Coefficient(exponent);

        public int Degree => Coefficients.Keys.Max();

        public ICPolynomial Add(ICPolynomial cp)
        {
            var result = new Dictionary<int, Complex2D>(Coefficients);
            foreach (var coef in cp)
                result[coef.exponent] = Coefficients[coef.exponent] + coef.coefficient;
            return new CPolynomial(result);
        }

        public static ICPolynomial operator+(CPolynomial cp1, ICPolynomial cp2)
        {
            return cp1.Add(cp2);
        }

        public static ICPolynomial operator +(ICPolynomial cp1, CPolynomial cp2)
        {
            return cp2.Add(cp1);
        }

        public static ICPolynomial operator *(CPolynomial cp1, ICPolynomial cp2)
        {
            return cp1.Multiply(cp2);
        }

        public static ICPolynomial operator *(ICPolynomial cp1, CPolynomial cp2)
        {
            return cp2.Multiply(cp1);
        }

        public Complex2D Coefficient(int exponent)
        {
            return Coefficients.ContainsKey(exponent) ? Coefficients[exponent] : 0;
        }

        public CPolynomial AsPolynomial()
        {
            return this;
        }

        public CPolynomial StepDown(Complex2D root)
        {
            var result = new Dictionary<int, Complex2D>();
            result[Degree - 1] = this[Degree];
            for (int i = Degree - 2; i >= 0; --i)
            {
                result[i] = this[i + 1] + root * result[i + 1];
            }
            return new CPolynomial(result);
        }

        public IEnumerator<(int exponent, Complex2D coefficient)> GetEnumerator()
        {
            return Coefficients.Select(kvp => (kvp.Key, kvp.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            string result = "";
            foreach (var coef in Coefficients.OrderByDescending(coef => coef.Key))
            {
                if (coef.Value != new Complex2D(0, 0))
                {
                    if (coef.Key == 0)
                        result += " + " + coef.Value;
                    else if (coef.Key == 1)
                        result += " + " + (coef.Value == 1 ? "" : "" + coef.Value) + "z";
                    else
                        result += " + " + (coef.Value == 1 ? "" : "" + coef.Value) + "z^" + coef.Key;
                }
            }

            return (result.Length > 0 ? result.Substring(3) : " 0");
        }
    }
}
