using Stuff.StuffMath;
using Stuff.StuffMath.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Recommendation
{
    static class Extensions
    {
        public static double ESimilarity<F>(this Vector<F> vec1, Vector<F> vec2) where F : IHilbertField<F>, new()
        {
            if (vec1.Size != vec2.Size)
                throw new ArgumentException("Only vectors of equal length can be correlated.");
            return 1d / (1 + new Vector<F>(vec1.Zip(vec2, (a, b) => (a.IsZero() || b.IsZero()) ? new F() : a.Subtract(b))).Length);
        }

        public static F Mean<F>(this Vector<F> vec) where F : IHilbertField<F>, new()
        {
            return vec.Total().Divide(vec.Count(f => !f.IsZero()));
        }

        public static double Variance<F>(this Vector<F> vec) where F : IHilbertField<F>, new()
        {
            var mean = vec.Mean();
            var zeroed = new Vector<F>(vec.Select(f => f.IsZero() ? f : f.Subtract(mean)));
            return zeroed.Aggregate(new F(), (a, f) => a.Add(f.Square())).Divide(vec.Count(f => !f.IsZero())).AbsSqrt().RealPart();
        }

        public static Vector<F> Standardize<F>(this Vector<F> vec) where F : IHilbertField<F>, new()
        {
            var mean = vec.Mean();
            var vari = vec.Variance();
            if (vari == 0)
                vari = 1;
            return new Vector<F>(vec.Select(f => f.IsZero() ? f : f.Subtract(mean).Divide(vari)));
        }

        public static double PCorrelation<F>(this Vector<F> vec1, Vector<F> vec2) where F : IHilbertField<F>, new()
        {
            if (vec1.Size != vec2.Size)
                throw new ArgumentException("Only vectors of equal length can be correlated.");
            var u1 = vec1.Standardize();
            var u2 = vec2.Standardize();
            
            return u1.DotSum(u2).Divide(vec1.Zip(vec2, (a, b) => (a, b)).Count(f => !f.a.IsZero() && !f.b.IsZero())).RealPart();
        }
    }
}
