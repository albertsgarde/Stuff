using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Complex
{
    public static class Extensions
    {
        public static ICPolynomial Add(this ICPolynomial cp1, ICPolynomial cp2)
        {
            var result = new Dictionary<int, Complex2D>();
            foreach (var (exp, coef) in cp1)
                result[exp] = coef;
            foreach (var (exp, coef) in cp2)
                result[exp] = (result.ContainsKey(exp) ? result[exp] : 0) + coef;
            return new CPolynomial(result);
        }

        public static ICPolynomial Multiply(this ICPolynomial cp1, ICPolynomial cp2)
        {
            var result = new Dictionary<int, Complex2D>();
            foreach (var (exp1, coef1) in cp1)
            {
                foreach (var (exp2, coef2) in cp2)
                {
                    var exp = exp1 + exp2;
                    result[exp] = (result.ContainsKey(exp) ? result[exp] : 0) + coef1 * coef2;
                }
            }
            return new CPolynomial(result);
        }
    }
}
