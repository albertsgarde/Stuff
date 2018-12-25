using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public struct Real : IHilbertField<Real>
    {
        public double Value { get; }

        public Real ZERO => zero;

        public Real ONE => one;

        private static readonly Real zero = new Real(0);

        private static readonly Real one = new Real(1);

        public Real(double d)
        {
            Value = d;
        }

        public static implicit operator Real(double d)
        {
            return new Real(d);
        }

        public static explicit operator double(Real r)
        {
            return r.Value;
        }

        public Real Add(Real r)
        {
            return new Real(Value + r.Value);
        }

        public Real AdditiveInverse()
        {
            return new Real(-Value);
        }

        public Real Multiply(Real r)
        {
            return new Real(Value * r.Value);
        }

        public Real Multiply(FDouble d) => new Real(Value * d);

        public Real MultiplicativeInverse()
        {
            if (Value == 0)
                throw new InvalidOperationException("0 has no mulitplicative inverse");
            return new Real(1d / Value);
        }

        public Real AbsSqrt() => Math.Sqrt(Value);

        public Real Conjugate() => Value;

        public FDouble RealPart() => Value;

        public bool EqualTo(Real r)
        {
            return Value == r.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is Real r && EqualTo(r);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() => Value.ToString();

        public string ToLatex() => ToString();
    }
}
