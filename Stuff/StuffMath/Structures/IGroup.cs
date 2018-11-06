using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public interface IGroup<G> where G : IGroup<G>
    {
        G Add(G t);

        G AdditiveInverse();

        /// <summary>
        /// This ring's additive identity. Should be constant across all instances.
        /// </summary>
        G ZERO
        {
            get;
        }

        bool EqualTo(G t);
    }

    public static class GroupExtensions
    {
        public static G Subtract<G>(this G t1, G t2) where G : IGroup<G>
        {
            return t1.Add(t2.AdditiveInverse());
        }
    }
}
