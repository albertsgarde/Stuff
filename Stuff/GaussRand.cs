using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff
{
    public class ZigguratGaussRand
    {
        private const int SEGMENTS = 128;

        /// <summary>
        /// x0
        /// </summary>
        private const double R = 3.442619855899;

        /// <summary>
        /// The area of each Segment.
        /// </summary>
        private const double A = 9.91256303526217e-3;

        private const double UINT_TO_DOUBLE = 1.0 / uint.MaxValue;


        private Random rnd;

        private readonly double[] x;
        private readonly double[] y;

        /// <summary>
        /// How much of each segment is within the distribution.
        /// </summary>
        private readonly uint[] xComp;


        private readonly double A_DIV_Y0;

        public ZigguratGaussRand(Random rnd)
        {
            this.rnd = rnd;

            x = new double[SEGMENTS + 1];
            y = new double[SEGMENTS];

            x[0] = R;
            y[0] = GaussianPDFDenorm(R);
            
            x[1] = R;
            y[1] = y[0] + (A / x[1]);

            for (int i = 2; i < SEGMENTS; i++)
            {
                x[i] = GaussiandPDFDenormInv(y[i - 1]);
                y[i] = y[i - 1] + (A / x[i]);
            }

            x[SEGMENTS] = 0.0;

            // Precompute values.

            A_DIV_Y0 = A / y[0];

            xComp = new uint[SEGMENTS];

            xComp[0] = (uint)(((R * y[0]) / A) * uint.MaxValue);

            for (int i = 1; i < SEGMENTS - 1; i++)
            {
                xComp[i] = (uint)((x[i + 1] / x[i]) * uint.MaxValue);
            }
            xComp[SEGMENTS - 1] = 0;
            
            Debug.Assert(Math.Abs(1.0 - y[SEGMENTS - 1]) < 1e-10);
        }

        public double NextGaussian()
        {
            while(true)
            {
                int rn = rnd.Next();
                int i = rn & 0x7F;
                double sign = ((rn & 0x80) == 0) ? 1 : -1;

                //uint 
            }
        }

        public double GaussianPDFDenorm(double x)
        {
            return Math.Exp(-(x * x / 2.0));
        }

        public double GaussiandPDFDenormInv(double y)
        {
            return Math.Sqrt(-2.0 * Math.Log(y));
        }
    }
}
