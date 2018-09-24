using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.DSP
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

        public static Signal operator +(Signal signal1, Signal signal2)
        {
            if (signal2.Length != signal2.Length)
                throw new ArgumentException($"In order to multiply, the signals must have the same length. First signal length: {signal1.Length} Second signal length: {signal2.Length}");
            return new Signal(signal1.Zip(signal2, (s1, s2) => (s1, s2)).Select(next => next.s1 + next.s2));
        }

        public static Signal operator -(Signal signal1, Signal signal2)
        {
            if (signal2.Length != signal2.Length)
                throw new ArgumentException($"In order to multiply, the signals must have the same length. First signal length: {signal1.Length} Second signal length: {signal2.Length}");
            return new Signal(signal1.Zip(signal2, (s1, s2) => (s1, s2)).Select(next => next.s1 - next.s2));
        }

        public Signal Scale(float gain)
        {
            return new Signal(this.Select(s => s * gain));
        }

        public float Correlate(Signal signal)
        {
            if (signal.Length != Length)
                throw new ArgumentException($"In order to correlate, the signals must have the same length. First signal length: {Length} Second signal length: {signal.Length}");
            return samples.Zip(signal, (s1, s2) => (s1, s2)).Sum(next => next.s1 * next.s2);
        }

        public float TotalDifference(Signal signal)
        {
            if (signal.Length != Length)
                throw new ArgumentException($"In order to correlate, the signals must have the same length. First signal length: {Length} Second signal length: {signal.Length}");
            var total = 0f;
            for (int i = 0; i < Length; ++i)
                total += Math.Abs(this[i] - signal[i]);
            return total;
        }

        public float AverageDifference(Signal signal)
        {
            return TotalDifference(signal) / Length;
        }

        public bool IsOrthogonal(Signal signal)
        {
            return Correlate(signal) == 0;
        }

        public bool IsEqual(Signal signal)
        {
            if (signal.Length != Length)
                return false;
            for (int i = 0; i < Length; ++i)
            {
                if (this[i] != signal[i])
                    return false;
            }
            return true;
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

        /// <summary>
        /// Normalizes the signal so the total value of all samples is the given totalValue.
        /// </summary>
        public Signal Normalize(float totalValue = 1)
        {
            var curTotal = samples.Sum();
            var factor = totalValue / curTotal;

            return new Signal(samples.Select(s => s * factor));
        }

        /// <summary>
        /// Resizes the Signal to the specified length. If the resulting signal is longer, it is filled out with 0's.
        /// </summary>
        public Signal Resize(int length)
        {
            if (length > Length)
            {
                var result = this.ToList();
                for (int i = Length; i < length; ++i)
                    result.Add(0);
                return new Signal(result);
            }
            else
            {
                var result = new List<float>(length);
                for (int i = 0; i < length; ++i)
                    result.Add(this[i]);
                return new Signal(result);
            }

        }

        public IEnumerator<float> GetEnumerator()
        {
            return samples.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Signal Null(int length)
        {
            var samples = new List<float>(length);
            for (int i = 0; i < length; ++i)
                samples.Add(0);
            return new Signal(samples);
        }

        public static Signal Impulse(int length, int impulsePos = 0)
        {
            var samples = new List<float>(length);
            for (int i = 0; i < impulsePos; ++i)
                samples.Add(0);
            samples.Add(1);
            for (int i = impulsePos + 1; i < length; ++i)
                samples.Add(0);
            return new Signal(samples);
        }

        public static Signal Step(int length, int stepPos = 0)
        {
            var samples = new List<float>(length);
            for (int i = 0; i < stepPos; ++i)
                samples.Add(0);
            for (int i = stepPos; i < length; ++i)
                samples.Add(1);
            return new Signal(samples);
        }

        public static Signal Noise(int length)
        {
            var samples = new List<float>(length);
            for (int i = 0; i < length; ++i)
                samples.Add((float)Rand.NextDouble() * 2 - 1);
            return new Signal(samples);
        }

        public static Signal SineWave(float frequency, float length, float amplitude = 1, float phase = 0, int samplerate = 44100)
        {
            var radialFreq = frequency * 2 * Math.PI;
            var samples = new List<float>((int)(length * samplerate));
            var delta = 1f / samplerate;

            var startT = 0 + phase / radialFreq;
            var endT = length + startT;

            var t = startT;
            while (t <= endT)
            {
                samples.Add(amplitude * (float)Math.Sin(radialFreq * t));
                t += delta;
            }

            return new Signal(samples);
        }
    }
}
