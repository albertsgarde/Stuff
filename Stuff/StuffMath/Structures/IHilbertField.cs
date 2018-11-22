using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public interface IHilbertField<T> : IField<T> where T : IHilbertField<T>, new()
    {
        T AbsSqrt();

        T Conjugate();

        FDouble RealPart();

        T Multiply(FDouble d);
    }

    public static class HilbertFieldExtensions
    {
        public static T Divide<T>(this T t, FDouble d) where T : IHilbertField<T>, new()
        {
            return t.Multiply(d.MultiplicativeInverse());
        }
    }
}
