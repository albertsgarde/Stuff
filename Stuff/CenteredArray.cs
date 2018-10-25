using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff
{
    public class CenteredArray<T> : IEnumerable<T>
    {
        /// <summary>
        /// All the negative indices.
        /// </summary>
        private T[] left;

        /// <summary>
        /// All the positive indices and 0.
        /// </summary>
        private T[] right;

        public int Length { get; }

        public int Start => -left.Length;

        public int End => right.Length;

        public CenteredArray(int length)
        {
            Length = length;

            left = new T[Length == 0 ? 0 : Length - 1];
            right = new T[Length];
        }

        public CenteredArray(int leftLength, int rightLength)
        {
            left = new T[leftLength];
            right = new T[rightLength];

            Length = left.Length + right.Length;
        }

        public T this[int index]
        {
            get
            {
                if (index >= 0)
                    return right[index];
                else
                    return left[index * -1 - 1];
            }
            set
            {
                if (index >= 0)
                    right[index] = value;
                else
                    left[index * -1 - 1] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int loop = Start; loop < End; loop++)
                yield return this[loop];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string AsString()
        {
            string result = "{";
            foreach (T value in this)
            {
                result += value + ",";
            }
            return result.Substring(0, result.Length - 1) + "}";
        }
    }
}
