using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public interface IField<T> : IRing<T> where T : IField<T>, new()
    {
        T MultiplicativeInverse();
    }

    public static class FieldExtensions
    {
        public static F Divide<F>(this F f1, F f2) where F : IField<F>, new()
        {
            return f1.Multiply(f2.MultiplicativeInverse());
        }
    }
}
