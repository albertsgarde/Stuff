using System;
using Stuff.Hamming;
using Stuff;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Hamming
{
    public class Simulation
    {
        private Data data;

        private Random rand;

        public Simulation(Data data, int seed)
        {
            this.data = data;
            rand = new Random(seed);
        }

        /*public void ThreadRun()
        {
            var time = DateTime.Now;
            double p = data.P;
            int repeats = data.BatchSize;
            int failures = 0;

            for (int i = 0; i < repeats; i++)
            {
                Bits bits = new Bits(4);
                Bits code = MatrixHammingCode.WordToCode(bits);
                Bits eCode = new Bits(code);
                for (int bit = 0; bit < eCode.Length; bit++)
                {
                    if (Rand.Next() < p)
                        eCode = eCode.FlipBits(bit);
                }
                Bits errorCode = MatrixHammingCode.CodeToErrors(eCode);
                Bits result = MatrixHammingCode.CodeToWord(eCode);
                if (!result.EqualTo(bits))
                    failures++;
            }
            //Console.WriteLine(new Data.DataBatch(repeats, failures));
            data.AddBatch(new Data.DataBatch(repeats, failures));
            data.AddTime(DateTime.Now - time);
        }*/
    }
}