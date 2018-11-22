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
            return 1 / (1 + (vec1 - vec2).Length.RealPart());
        }

        public static F Mean<F>(this Vector<F> vec) where F : IHilbertField<F>, new()
        {
            return vec.Aggregate(new F(), (a, f) => a.Add(f)).Divide(vec.Length);
        }

        public static double Variance<F>(this Vector<F> vec) where F : IHilbertField<F>, new()
        {
            var mean = vec.Mean();
            return (vec.Aggregate(new F(), (a, f) => a.Add(f.Subtract(mean).Square())).Divide(vec.Length)).AbsSqrt().RealPart();
        }

        public static Vector<F> Standardize<F>(this Vector<F> vec) where F : IHilbertField<F>, new()
        {
            var mean = vec.Mean();
            var vari = vec.Variance();
            return new Vector<F>(vec.Select(f => f.Subtract(mean).Divide(vari)));
        }

        public static double PCorrelation<F>(this Vector<F> vec1, Vector<F> vec2) where F : IHilbertField<F>, new()
        {
            if (vec1.Size != vec2.Size)
                throw new ArgumentException("Only vectors of equal length can be correlated.");
            var u1 = vec1.Standardize();
            var u2 = vec2.Standardize();
            
            return u1.DotSum(u2).Divide(vec1.Size).RealPart();
        }
    }
}
