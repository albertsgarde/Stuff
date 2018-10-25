using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    /// <summary>
    /// A rational number.
    /// </summary>
    public struct Rational : IField<Rational>
    {
        public Integer Numerator { get; }

        public Integer Denominator { get; }

        public Rational ZERO => zero;

        public Rational ONE => one;

        private static readonly Rational zero = new Rational(0, 1);

        private static readonly Rational one = new Rational(1, 1);

        public Rational(Integer numerator, Integer denominator)
        {
            Numerator = numerator;
            Denominator = denominator;
        }

        public static explicit operator double(Rational r)
        {
            return (double)r.Numerator / (double)r.Denominator;
        }

        private Rational ScaleUp(Integer i)
        {
            return new Rational(Numerator * i, Denominator * i);
        }

        private Rational ScaleDown(Integer i)
        {
            if (Numerator.Value % i.Value != 0)
                throw new ArgumentException($"Numerator {Numerator} not divisible by i {i}");
            else if (Denominator.Value % i.Value != 0)
                throw new ArgumentException($"Denominator {Denominator} not divisible by i {i}");
            else
                return new Rational(Numerator.Value / i.Value, Denominator.Value / i.Value);
        }

        private Rational Reduce()
        {
            var gcd = new Integer(Basic.GCD((int)Numerator, (int)Denominator));
            return new Rational(Numerator / gcd, Denominator / gcd);
        }

        public Rational Add(Rational r)
        {
            return new Rational(Numerator * r.Denominator + r.Numerator * Denominator, Denominator*r.Denominator).Reduce();
        }

        public Rational AdditiveInverse()
        {
            return new Rational(-Numerator, Denominator);
        }

        public Rational Multiply(Rational r)
        {
            return new Rational(Numerator * r.Numerator, Denominator * r.Denominator).Reduce();
        }

        public Rational MultiplicativeInverse()
        {
            return new Rational(Denominator, Numerator);
        }

        public bool EqualTo(Rational r)
        {
            return Numerator == r.Numerator;
        }

        public override bool Equals(object obj)
        {
            return obj is Rational r && EqualTo(r);
        }

        public override int GetHashCode()
        {
            return Misc.HashCode(17, 23, Numerator, Denominator);
        }
    }
}
