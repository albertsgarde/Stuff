using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public class ModInt : IRing<ModInt>
    {
        public int N { get; }

        public int Value { get; }

        public ModInt ONE => new ModInt(N, 1);

        public ModInt ZERO => new ModInt(N, 0);

        public ModInt(int n, int value)
        {
            if (n <= 0)
                throw new ArgumentException("N must be positive.");
            N = n;
            Value = Basic.Mod(value, n);
        }

        public ModInt(int n, Integer value) : this(n, value.Value)
        {

        }

        public static ModInt operator *(ModInt mi, Integer i)
        {
            return mi.Multiply(new ModInt(mi.N, i));
        }

        public ModInt Add(ModInt i)
        {
            if (i.N != N)
                throw new ArgumentException("In order to add two ModInts, they must have the same N");
            return new ModInt(N, Value + i.Value);
        }

        public ModInt AdditiveInverse()
        {
            return new ModInt(N, -Value);
        }

        public bool EqualTo(ModInt i)
        {
            return N == i.N && Value == i.Value;
        }

        public ModInt Multiply(ModInt i)
        {
            if (i.N != N)
                throw new ArgumentException("In order to multiply two ModInts, they must have the same N");
            return new ModInt(N, Value * i.Value);
        }

        public ModInt Power(int a)
        {
            var result = this;
            for (int i = 1; i < a; ++i)
            {
                result *= this.Value;
            }
            return result;
        }
    }
}
