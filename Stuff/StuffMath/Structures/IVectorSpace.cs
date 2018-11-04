using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    /// <summary>
    /// A mathematical vector space.
    /// </summary>
    /// <typeparam name="T">The vectorspace.</typeparam>
    /// <typeparam name="F">The field by which scalar multiplication is defined.</typeparam>
    public interface IVectorSpace<T, F> : IGroup<T> where F : IField<F>
    {
        T Multiply(F s);
        
        /// <summary>
        /// This vector space's field's multiplicative identity. Should be constant across all instances.
        /// </summary>
        F ONE
        {
            get;
        }

        Vector<F> ToVector();
    }

    public static class VectorSpaceExtensions
    {
        public static T Subtract<T, F>(this T t1, T t2) where F : IField<F> where T : IVectorSpace<T, F>
        {
            return t1.Add(t2.AdditiveInverse());
        }

        public static T Divide<T, F>(this IVectorSpace<T, F> t, F s) where T : IVectorSpace<T, F> where F : IField<F>
        {
            return t.Multiply(s.MultiplicativeInverse());
        }
    }
}
