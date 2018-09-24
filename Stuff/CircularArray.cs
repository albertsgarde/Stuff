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

        /// <summary>
        /// Adds the specified element to the front of the array, thereby removing the last element.
        /// </summary>
        public T Add(T value)
        {
            if (--curPos == -1)
                curPos = Length - 1;
            values[curPos] = value;
            return value;
        }

        /// <summary>
        /// Moves the last element to the front.
        /// </summary>
        /// <returns>The new front element.</returns>
        public T Advance()
        {
            if (--curPos == -1)
                curPos = Length - 1;
            return values[curPos];
        }

        public T First()
        {
            return values[curPos];
        }

        public T Last()
        {
            if (curPos == 0)
                return values[Length - 1];
            else
                return values[curPos - 1];
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
