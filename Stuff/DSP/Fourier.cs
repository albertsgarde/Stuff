using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.DSP
{
    public static class Fourier
    {
        /*public static (Signal real, Signal imaginary) DFT(Signal x)
        {
            var resultLength = x.Length / 2 + 1;

            var real = new List<float>(resultLength);
            for (int k = 0; k < resultLength; ++k)
            {
                var reK = 0f;
                var radFreq = k * Math.PI * 2/x.Length;
                for (int i = 0; i < x.Length; ++i)
                    reK += x[i] * (float)Math.Cos(i * radFreq);
                real.Add(reK);
            }

            var imaginary = new List<float>(resultLength);
            for (int k = 0; k < resultLength; ++k)
            {
                var imK = 0f;
                var radFreq = k * Math.PI * 2 / x.Length;
                for (int i = 0; i < x.Length; ++i)
                    imK += x[i] * (float)Math.Sin(i * radFreq);
                imaginary.Add(-imK);
            }

            return (new Signal(real), new Signal(imaginary));
        }*/

        public static (Signal real, Signal imaginary) DFT(Signal x)
        {
            var resultLength = x.Length / 2 + 1;
            var real = new List<double>(resultLength);
            var imaginary = new List<double>(resultLength);

            for (int k = 0; k < resultLength; ++k)
            {
                real.Add(0);
                imaginary.Add(0);
            }

            for (int i = 0; i < x.Length; ++i)
            {
                var radFreq = i * Math.PI * 2 / x.Length;
                var cos = 0d;
                var sin = 0d;
                for (int k = 0; k < resultLength; ++k)
                {
                    cos = Math.Cos(k * radFreq);
                    real[k] += x[i] * cos;
                    sin = Math.Sin(k * radFreq);
                    imaginary[k] -= x[i] * sin;
                }
            }

            return (new Signal(real.Select(d => (float)d)), new Signal(imaginary.Select(d => (float)d)));
        }

        public static Signal IDFT(Signal real, Signal imaginary)
        {
            var length = (real.Length - 1) * 2;
            var result = new List<double>(length);
            for (int i = 0; i < length; ++i)
            {
                var radFreq = i * Math.PI * 2 / length;

                var x = real[0] * Math.Cos(radFreq * 0);
                for (int k = 1; k < real.Length - 1; ++k)
                    x += 2 * real[k] * Math.Cos(radFreq * k);
                x += real[real.Length - 1] * Math.Cos(radFreq * real.Length - 1);
                
                x -= imaginary[0] * Math.Sin(radFreq * 0);
                for (int k = 1; k < real.Length - 1; ++k)
                    x -= 2 * imaginary[k] * Math.Sin(radFreq * k);
                x -= imaginary[real.Length - 1] * Math.Sin(radFreq * real.Length - 1);
                result.Add(x / length);
            }
            return new Signal(result.Select(d => (float)d));
        }
    }
}
