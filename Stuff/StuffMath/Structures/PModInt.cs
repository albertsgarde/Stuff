using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public class PModInt : IField<PModInt>
    {
        public int N { get; }

        public Integer Value { get; }

        public PModInt ONE => new PModInt(N, 1);

        public PModInt ZERO => new PModInt(N, 0);

        public PModInt(int n, int value)
        {
            if (N <= 0)
                throw new ArgumentException("N must be positive.");
            if (!Basic.IsPrime(value))
                throw new ArgumentException("N must be prime.");
            N = n;
            Value = Basic.Mod(value, n);
        }

        public PModInt(int n, Integer value) : this(n, value.Value)
        {

        }

        public static PModInt operator *(PModInt mi, Integer i)
        {
            return mi.Multiply(new PModInt(mi.N, i));
        }

        public PModInt Add(PModInt i)
        {
            if (i.N != N)
                throw new ArgumentException("In order to add two ModInts, they must have the same N");
            return new PModInt(N, Value + i.Value);
        }

        public bool EqualTo(PModInt i)
        {
            return N == i.N && Value == i.Value;
        }

        public PModInt MultiplicativeInverse()
        {
            if (Value == 0)
                throw new InvalidOperationException("0 has no mulitplicative inverse");
            for (int i = 1; i < N; ++i)
            {
                if ((this * i).Value == 1)
                    return new PModInt(N, i);
            }
            throw new Exception("Program should never get here.");
        }

        public PModInt Multiply(PModInt i)
        {
            if (i.N != N)
                throw new ArgumentException("In order to multiply two ModInts, they must have the same N");
            return new PModInt(N, Value * i.Value);
        }

        public PModInt AdditiveInverse()
        {
            return new PModInt(N, 0);
        }
    }
}
