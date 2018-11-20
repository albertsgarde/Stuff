using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public interface IHilbertSpace<T, F> : IVectorSpace<T, F> where T : IHilbertSpace<T, F> where F : IHilbertField<F>, new()
    {
        F DotSum(T hs);
    }

    public static class HilbertSpaceExtensions
    {
        public static F LengthSquared<T, F>(this T hs) where T : IHilbertSpace<T, F> where F : IHilbertField<F>, new()
        {
            return hs.DotSum(hs);
        }
    }
}
