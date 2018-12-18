using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    /// <summary>
    /// Like an IHilbertField, but with some extra methods for interfacing with doubles.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRealHilbertField<T> : IHilbertField<T> where T : IRealHilbertField<T>, new()
    {
        FDouble RealPart();

        T Multiply(FDouble d);
    }

    public static class RealHilbertFieldExtensions
    {
        public static T Divide<T>(this T t, FDouble d) where T : IRealHilbertField<T>, new()
        {
            return t.Multiply(d.MultiplicativeInverse());
        }
    }
}
