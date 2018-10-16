using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.DSP
{
    public class RunningFrequencyAnalyzer
    {
        public int Size { get; }

        public IReadOnlyList<float> Frequencies { get; }

        private List<(float real, float imaginary)> amplitudes;

        private readonly CircularArray<List<(float real, float imaginary)>> data;

        int i = -1;

        public RunningFrequencyAnalyzer(int size, IEnumerable<float> frequencies)
        {
            Frequencies = new List<float>(frequencies);
            Size = size;


            amplitudes = new List<(float real, float imaginary)>(Frequencies.Count);
            for (int i = 0; i < Frequencies.Count; ++i)
                amplitudes.Add((0, 0));

            data = new CircularArray<List<(float real, float imaginary)>>(Size);
            for (int i = 0; i < Size; ++i)
            {
                data.Add(new List<(float real, float imaginary)>(Frequencies.Count));
                for (int k = 0; k < Frequencies.Count; ++k)
                    data.First().Add((0, 0));
            }
        }

        public void NewSample(float sample)
        {
            var r = ++i * Math.PI * 2;
            var list = data.Advance(); // The datapoint that will be changed.
            for (int k = 0; k < Frequencies.Count; ++k) //Runs through all frequencies. f = samplerate/(2*DFTSize)*k
            {
                amplitudes[k] = (amplitudes[k].real - list[k].real, amplitudes[k].imaginary - list[k].imaginary); // Subtracts the now disappearing data.
                list[k] = ((float)(sample * Math.Cos(r*Frequencies[k])), (float)(sample * Math.Sin(r*Frequencies[k]))); // Replaces the disappearing data with data from the new sample.
                amplitudes[k] = (amplitudes[k].real + list[k].real, amplitudes[k].imaginary + list[k].imaginary); // Adds the data from the new sample.
            }
        }

        private float Amplitude((float real, float imaginary) z)
        {
            return (float)Math.Sqrt(z.real * z.real + z.imaginary * z.imaginary);
        }

        public void NewSamples(float[] samples)
        {
            foreach (var s in samples)
                NewSample(s);
        }

        public float Fundamental()
        {
            var maxValue = amplitudes.Max(a => Amplitude(a));
            for (int k = 0; k < Frequencies.Count; ++k)
            {
                if (Amplitude(amplitudes[k]) == maxValue)
                    return Frequencies[k];
            }
            throw new Exception("Not possible");
        }

        public static IEnumerable<float> LogFreqs(double root, double start, int from, int to)
        {
            var result = new List<float>(to - from);
            for (int i = from; i < to; ++i)
                result.Add((float)(start*Math.Pow(root, i)));
            return result;
        }
    }
}
