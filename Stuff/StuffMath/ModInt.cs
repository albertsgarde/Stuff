using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public struct ModInt
    {
        public int Value { get; }

        public int N { get; }

        public ModInt(int value, int n)
        {
            N = n;
            Value = value % n;
        }

        public static ModInt operator+(ModInt a, ModInt b)
        {
            if (a.N != b.N)
                throw new ModClassException(a.N, b.N, "Can only add modInts with equal N's");
            return new ModInt(a.Value + b.Value, a.N);
        }

        public static ModInt operator+(ModInt a, int b)
        {
            return new ModInt(a.Value + b, a.N);
        }

        public static ModInt operator++(ModInt a)
        {
            return new ModInt(a.Value + 1, a.N);
        }

        public static ModInt operator -(ModInt a, ModInt b)
        {
            if (a.N != b.N)
                throw new ModClassException(a.N, b.N, "Can only subtract modInts with equal N's");
            return new ModInt(a.Value - b.Value, a.N);
        }

        public static ModInt operator -(ModInt a, int b)
        {
            return new ModInt(a.Value - b, a.N);
        }

        public static ModInt operator --(ModInt a)
        {
            return new ModInt(a.Value - 1, a.N);
        }

        public static ModInt operator *(ModInt a, ModInt b)
        {
            if (a.N != b.N)
                throw new ModClassException(a.N, b.N, "Can only multiply modInts with equal N's");
            return new ModInt(a.Value * b.Value, a.N);
        }

        public static ModInt operator *(ModInt a, int b)
        {
            return new ModInt(a.Value * b, a.N);
        }

        public static ModInt operator /(ModInt a, ModInt b)
        {
            return a * b.MultiplicativeInverse();
        }

        public ModInt AdditiveInverse()
        {
            return new ModInt(N - Value, N);
        }

        public ModInt MultiplicativeInverse()
        {
            if (Basic.GCD(Value, N) != 0)
                throw new InvalidOperationException("Numbers that aren't coprime with N don't have a multiplicative inverse.");
            for (int i = 0; i < N; ++i)
            {
                if ((this * i).Value == 1)
                    return new ModInt(i, N);
            }
            throw new Exception("Should never get here.");
        }
    }

    public class ModClassException : Exception
    {
        public int N1 { get; }

        public int N2 { get; }

        public ModClassException(int n1, int n2) : base()
        {
            N2 = n2;
            N1 = n1;
        }

        public ModClassException(int n1, int n2, string message) : base(message)
        {
            N2 = n2;
            N1 = n1;
        }
    }

}
