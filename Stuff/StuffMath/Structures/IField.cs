using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public interface IField<T> : IRing<T>
    {
        T MultiplicativeInverse();
    }

    public static class FieldExtensions
    {
        public static T Divide<T>(this T t1, T t2) where T : IField<T>
        {
            return t1.Multiply(t2.MultiplicativeInverse());
        }
    }
}
