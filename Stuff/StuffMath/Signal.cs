using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Signal : IEnumerable<float>
    {
        private readonly IReadOnlyList<float> samples;

        public Signal(IEnumerable<float> samples)
        {
            this.samples = samples.ToList();
        }

        public float this[int index] => samples[index];

        public int Length => samples.Count;

        public static Signal operator *(Signal signal1, Signal signal2)
        {
            if (signal2.Length != signal2.Length)
                throw new ArgumentException($"In order to multiply, the signals must have the same length. First signal length: {signal1.Length} Second signal length: {signal2.Length}");
            return new Signal(signal1.Zip(signal2, (s1, s2) => (s1, s2)).Select(next => next.s1 * next.s2));
        }

        public float Correlate(Signal signal)
        {
            if (signal.Length != Length)
                throw new ArgumentException($"In order to correlate, the signals must have the same length. First signal length: {Length} Second signal length: {signal.Length}");
            return samples.Zip(signal, (s1, s2) => (s1, s2)).Sum(next => next.s1 * next.s2);
        }

        public Signal Convolve(Signal signal)
        {
            var result = new List<float>(Length + signal.Length - 1);
            for (int i = 0; i < Length; ++i)
            {
                for (int j = 0; j < signal.Length; ++j)
                    result[i + j] = this[i] * signal[j];
            }
            return new Signal(result);
        }

        public IEnumerator<float> GetEnumerator()
        {
            return samples.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
