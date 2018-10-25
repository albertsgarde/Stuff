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
    /// <typeparam name="S">The field by which scalar multiplication is defined.</typeparam>
    public interface IVectorSpace<T, S> where S : IField<S>
    {
        T Add(T t);

        T AdditiveInverse();

        T Multiply(S s);

        /// <summary>
        /// This vector space's additive identity. Should be constant across all instances.
        /// </summary>
        T ZERO
        {
            get;
        }
        
        /// <summary>
        /// This vector space's field's multiplicative identity. Should be constant across all instances.
        /// </summary>
        S ONE
        {
            get;
        }

        bool EqualTo(T t);
    }

    public static class VectorSpaceExtensions
    {
        public static T Subtract<T, S>(this T t1, T t2) where S : IField<S> where T : IVectorSpace<T, S>
        {
            return t1.Add(t2.AdditiveInverse());
        }

        public static T Divide<T, S>(this T t, S s) where S : IField<S> where T : IVectorSpace<T, S>
        {
            return t.Multiply(s.MultiplicativeInverse());
        }
    }
}
