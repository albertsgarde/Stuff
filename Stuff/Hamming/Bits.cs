using System;
using Stuff.StuffMath;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Stuff.Hamming
{
    public class Bits : IEnumerable<bool>
    {
        private readonly bool[] bits;

        public Bits(int size)
        {
            bits = new bool[size];
            for (int i = 0; i < size; i++)
                bits[i] = Rand.NextBool();
        }

        public Bits(params bool[] bits)
        {
            this.bits = new bool[bits.Length];
            for (int i = 0; i < bits.Length; i++)
                this.bits[i] = bits[i];
        }

        public Bits(Bits bits)
        {
            this.bits = new bool[bits.Length];
            for (int i = 0; i < bits.Length; i++)
                this.bits[i] = bits[i];
        }

        public Bits(string word)
        {
            bits = new bool[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == '1')
                    bits[i] = true;
                else if (word[i] == '0')
                    bits[i] = false;
                else
                    throw new ArgumentException("word may only contain 0's and 1's.");
            }
        }

        public Bits(byte b)
        {
            bits = new bool[8];
            for (int i = 0; i < 8; --i)
                bits[i] = (b %= (byte)Math.Pow(2, 8 - i)) > Math.Pow(2, 8 - i - 1); 
        }

        public Bits(Vector vec)
        {
            if (vec.Count(x => x != 0 && x != 1) != 0)
                throw new ArgumentException("Vector may only contain 0's and 1's.");
            bits = vec.Select(x => x == 1).ToArray();
        }

        public bool this[int index]
        {
            get
            {
                return bits[index];
            }
        }

        public int Length
        {
            get
            {
                return bits.Length;
            }
        }

        public Bits FlipBits(params int[] indexes)
        {
            Bits result = new Bits(this);
            foreach (int i in indexes)
            {
                result.bits[i] = !result.bits[i];
            }
            return result;
        }

        public static implicit operator bool[] (Bits bits)
        {
            bool[] result = new bool[bits.Length];
            for (int i = 0; i < bits.Length; i++)
                result[i] = bits[i];
            return result;
        }

        public bool EqualTo(Bits bits)
        {
            if (Length != bits.Length)
                return false;
            for (int i = 0; i < Length; i++)
            {
                if (bits[i] != this.bits[i])
                    return false;
            }
            return true;
        }
        
        public int ToInt()
        {
            int result = 0;
            for (int i = 0; i < bits.Length; i++)
                result += bits[i] ? (int)Math.Pow(2, i) : 0;
            return result;
        }

        public int Distance(Bits bits)
        {
            if (bits.Length != Length)
                throw new ArgumentException("Bits must have the same length.");
            int result = 0;
            for (int i = 0; i < Length; i++)
                result += this.bits[i] == bits[i] ? 0 : 1;
            return result;
        }

        public Vector ToVector()
        {
            return new Vector(bits.Select(x => x ? 1d : 0d).ToArray());
        }

        public Bits XOR(Bits b)
        {
            var bits = new bool[Length];
            for (int i = 0; i < Length; ++i)
                bits[i] = this[i] != b[i];
            return new Bits(bits);
        }

        public Bits Append(Bits b)
        {
            var bits = new bool[Length + b.Length];
            for (int i = 0; i < Length; ++i)
                bits[i] = this[i];
            for (int i = Length; i < bits.Length; ++i)
                bits[i] = b[i];
            return new Bits(bits);
        }

        public override string ToString()
        {
            string result = "";
            foreach (bool bit in bits)
                result += bit ? '1' : '0';
            return result;
        }

        public IEnumerator<bool> GetEnumerator()
        {
            return ((IEnumerable<bool>)bits).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<bool>)bits).GetEnumerator();
        }
    }
}
