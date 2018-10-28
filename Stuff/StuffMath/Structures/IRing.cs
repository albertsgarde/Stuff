using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public interface IRing<T> : IGroup<T>
    {
        T Multiply(T t);

        /// <summary>
        /// This ring's multiplicative identity. Should be constant across all instances.
        /// </summary>
        T ONE
        {
            get;
        }
    }
}
