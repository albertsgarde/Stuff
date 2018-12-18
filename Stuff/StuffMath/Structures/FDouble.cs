using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public struct FDouble : IRealHilbertField<FDouble>
    {
        public double Value { get; }

        public FDouble ZERO => zero;

        public FDouble ONE => one;

        private static readonly FDouble zero = new FDouble(0);

        private static readonly FDouble one = new FDouble(1);

        public FDouble(double d)
        {
            Value = d;
        }

        public static implicit operator FDouble(double d)
        {
            return new FDouble(d);
        }

        public static implicit operator double(FDouble r)
        {
            return r.Value;
        }

        public static implicit operator FDouble(int i) => new FDouble(i);

        public FDouble Add(FDouble r)
        {
            return new FDouble(Value + r.Value);
        }

        public FDouble AdditiveInverse()
        {
            return new FDouble(-Value);
        }

        public FDouble Multiply(FDouble r)
        {
            return new FDouble(Value * r.Value);
        }

        public FDouble MultiplicativeInverse()
        {
            if (Value == 0)
                throw new InvalidOperationException("0 has no mulitplicative inverse");
            return new FDouble(1d / Value);
        }

        public FDouble Sqrt()
        {
            return new FDouble(Math.Sqrt(Value));
        }

        public FDouble AbsSqrt()
        {
            return Math.Sqrt(Value);
        }

        public FDouble Conjugate() => Value;

        public FDouble RealPart() => Value;

        public bool EqualTo(FDouble r)
        {
            return Value == r.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is FDouble r && EqualTo(r);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return "" + Value;
        }
    }
}
