using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public interface IGroup<T>
    {
        T Add(T t);

        T AdditiveInverse();

        /// <summary>
        /// This ring's additive identity. Should be constant across all instances.
        /// </summary>
        T ZERO
        {
            get;
        }

        bool EqualTo(T t);
    }

    public static class GroupExtensions
    {
        public static T Subtract<T>(this T t1, T t2) where T : IGroup<T>
        {
            return t1.Add(t2.AdditiveInverse());
        }
    }
}
