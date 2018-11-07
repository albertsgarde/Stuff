using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Structures
{
    public struct Integer : IRing<Integer>
    {
        public long Value { get; }

        private static readonly Integer zero = new Integer(0);

        private static readonly Integer one = new Integer(1);

        public Integer(long i)
        {
            Value = i;
        }

        public static implicit operator Integer(long i)
        {
            return new Integer(i);
        }

        public static explicit operator long(Integer i)
        {
            return i.Value;
        }

        public static explicit operator double(Integer i)
        {
            return i.Value;
        }

        public static Integer operator +(Integer i1, Integer i2)
        {
            return new Integer(i1.Value + i2.Value);
        }

        public static Integer operator -(Integer i1, Integer i2)
        {
            return new Integer(i1.Value - i2.Value);
        }

        public static Integer operator -(Integer i)
        {
            return new Integer(-i.Value);
        }

        public static Integer operator *(Integer i1, Integer i2)
        {
            return new Integer(i1.Value * i2.Value);
        }

        public static Integer operator /(Integer i1, Integer i2)
        {
            if (i1.Value % i2.Value != 0)
                throw new ArgumentException($"i1 {i1} not divisible by i2 {i2}");
            return new Integer(i1.Value / i2.Value);
        }

        public static Integer operator %(Integer i1, Integer i2)
        {
            return new Integer(i1.Value % i2.Value);
        }

        public static bool operator ==(Integer i1, Integer i2)
        {
            return i1.EqualTo(i2);
        }

        public static bool operator !=(Integer i1, Integer i2)
        {
            return !i1.EqualTo(i2);
        }

        public Integer ZERO => zero;

        public Integer ONE => one;

        public Integer Add(Integer t)
        {
            return new Integer(Value + t.Value);
        }

        public Integer AdditiveInverse()
        {
            return new Integer(-Value);
        }

        public Integer Multiply(Integer t)
        {
            return new Integer(Value * t.Value);
        }

        public bool EqualTo(Integer t)
        {
            return Value == t.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is Integer i && EqualTo(i);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return "" +  Value;
        }
    }
}
