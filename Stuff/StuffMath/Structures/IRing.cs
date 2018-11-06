using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public interface IRing<R> : IGroup<R> where R : IRing<R>, new()
    {
        R Multiply(R t);

        /// <summary>
        /// This ring's multiplicative identity. Should be constant across all instances.
        /// </summary>
        R ONE
        {
            get;
        }
    }

    public static class RingExtensions
    {
        public static bool IsZero<R>(this R r) where R : IRing<R>, new()
        {
            return r.EqualTo(new R());
        }

        public static bool IsOne<R>(this R r) where R : IRing<R>, new()
        {
            return r.EqualTo(new R().ONE);
        }
    }
}
