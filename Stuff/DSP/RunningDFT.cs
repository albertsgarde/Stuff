using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.DSP
{
    public class RunningDFT
    {
        public int Size { get; }

        public int DFTSize { get; }

        private readonly List<float> real;

        private readonly CircularArray<List<float>> realData;

        private readonly List<float> imaginary;

        private readonly CircularArray<List<float>> imaginaryData;

        private int i;

        public RunningDFT(int size)
        {
            if (size % 2 != 0)
                throw new ArgumentException("Size of a RunningDFT must be even.");
            Size = size;
            DFTSize = Size / 2 + 1;

            real = new List<float>(DFTSize);
            for (int k = 0; k < DFTSize; ++k)
                real.Add(0);

            realData = new CircularArray<List<float>>(size);
            for (int i = 0; i < Size; ++i)
            {
                realData.Add(new List<float>(DFTSize));
                for (int k = 0; k < DFTSize; ++k)
                    realData.First().Add(0);
            }

            imaginary = new List<float>(DFTSize);
            for (int k = 0; k < DFTSize; ++k)
                imaginary.Add(0);

            imaginaryData = new CircularArray<List<float>>(size);
            for (int i = 0; i < Size; ++i)
            {
                imaginaryData.Add(new List<float>(DFTSize));
                for (int k = 0; k < DFTSize; ++k)
                    imaginaryData.First().Add(0);
            }
            i = -1;
        }

        public void NewSample(float sample)
        {
            var radFreq = ++i * Math.PI * 2 / Size;
            var realList = realData.Advance(); // The datapoint that will be changed.
            var imaginaryList = imaginaryData.Advance(); // By the end of this method, these lists will contain data for the newest sample.
            for (int k = 0; k < DFTSize; ++k)//Runs through all frequencies. f = samplerate/(2*DFTSize)*k
            {
                real[k] -= realList[k]; // Subtracts the now disappearing data.
                realList[k] = (float)(sample * Math.Cos(k * radFreq)); // Replaces the disappearing data with data from the new sample.
                real[k] += realList[k]; // Adds the data from the new sample.
                imaginary[k] -= imaginaryList[k];
                imaginaryList[k] = (float)(sample * -Math.Sin(k * radFreq));
                imaginary[k] += imaginaryList[k];
            }
        }
    }
}
