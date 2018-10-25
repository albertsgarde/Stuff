using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public interface IRing<T>
    {
        T Add(T t);

        T AdditiveInverse();

        T Multiply(T t);

        /// <summary>
        /// This ring's additive identity. Should be constant across all instances.
        /// </summary>
        T ZERO
        {
            get;
        }

        /// <summary>
        /// This ring's multiplicative identity. Should be constant across all instances.
        /// </summary>
        T ONE
        {
            get;
        }

        bool EqualTo(T t);
    }

    public static class RingExtensions
    {
        public static T Subtract<T>(this T t1, T t2) where T : IRing<T>
        {
            return t1.Add(t2.AdditiveInverse());
        }
    }
}
