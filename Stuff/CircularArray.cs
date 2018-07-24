using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff
{
    public class CircularArray<T> : IEnumerable<T>
    {
        private readonly T[] values;

        private int curPos;

        public int Length => values.Length;

        public CircularArray(int size)
        {
            curPos = 0;
            values = new T[size];
        }

        public T this[int index] => values[(index + curPos) % Length];

        public T Add(T value)
        {
            if (--curPos == -1)
                curPos = Length - 1;
            values[curPos] = value;
            return value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = curPos; i < Length; ++i)
                yield return values[i];
            for (int i = 0; i < curPos; ++i)
                yield return values[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
