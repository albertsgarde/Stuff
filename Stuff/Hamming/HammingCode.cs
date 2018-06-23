using System;
using Stuff.StuffMath;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Hamming
{
    public static class HammingCode
    {
        public static Bits WordToCode(Bits word, int n)
        {
            if (word.Length != Math.Pow(2, n) - 1 - n)
                throw new ArgumentException("The length of the word must be equal to 2^n - 1 - n.");
            var result = new bool[(int)Math.Pow(2, n) - 1];
            int nextParity = 0;
            int nextParityPower = 0;
            int[] parityTotals = new int[n];
            for (int i = 0; i < word.Length; i++)
            {
                if (i == nextParity)
                {
                    nextParity = nextParity * 2 + 1;
                    nextParityPower++;
                }
                else
                {
                    result[i] = word[i - nextParityPower];
                    for (int j = 0; j < nextParityPower; j++)
                    {
                        int power = (int)Math.Pow(2, j);
                        if (i % (power * 2) >= power)
                            parityTotals[j] += word[i - nextParityPower] ? 1 : 0;
                    }
                }
            }
            nextParityPower = 0;
            for (int i = 0; i < result.Length; i = i * 2 + 1)
            {
                result[i] = parityTotals[nextParityPower] % 2 == 1;
                nextParityPower++;
            }
            return new Bits(result);
        }

        public static Bits CodeToWord(Bits code, int n)
        {
            if (code.Length != Math.Pow(2, n) - 1)
                throw new ArgumentException("The length of the code must be equal to 2^n - 1.");
            var result = new bool[(int)Math.Pow(2, n) - n - 1];
            int nextParity = 0;
            int nextParityPower = 0;
            int[] parityTotals = new int[n];
            bool[] parities = new bool[n];
            for (int i = 0; i < code.Length; i++)
            {
                if (i == nextParity)
                {
                    nextParity = nextParity * 2 + 1;
                    parities[nextParityPower] = code[i];
                    nextParityPower++;
                }
                else
                {
                    result[i - nextParityPower] = code[i];
                    for (int j = 0; j < nextParityPower; j++)
                    {
                        int power = (int)Math.Pow(2, j);
                        if (i % (power * 2) >= power)
                            parityTotals[j] += code[i] ? 1 : 0;
                    }
                }
            }
            int errorBit = 0;
            for (int i = 0; i < n; i++)
            {
                if (parities[i] != (parityTotals[i] % 2 == 1))
                    errorBit += (int)Math.Pow(2, i);
            }
            result[errorBit - 1] = !result[errorBit - 1];
            return new Bits(result);
        }

        public static bool Xor(bool[] bools)
        {
            int i = 0;
            foreach (var b in bools)
                i += b ? 1 : 0;
            return i % 2 == 1;
        }

        public static Bits WordToCode74(Bits word)
        {
            if (word.Length != 4)
                throw new ArgumentException("word must be 4 bits long. Word: " + word);
            bool[] result = new bool[7];
            result[2] = word[0];
            result[4] = word[1];
            result[5] = word[2];
            result[6] = word[3];
            result[0] = Xor(new bool[] { result[2], result[4], result[6] });
            result[1] = Xor(new bool[] { result[2], result[5], result[6] });
            result[3] = Xor(new bool[] { result[4], result[5], result[6] });
            return new Bits(result);
        }

        public static Bits CodeToWord74(Bits code)
        {
            if (code.Length != 7)
                throw new ArgumentException("code must be 7 bits long. Code: " + code);
            bool[] result = new bool[4];
            bool p0Err = Xor(new bool[] { code[2], code[4], code[6] }) != code[0];
            bool p1Err = Xor(new bool[] { code[2], code[5], code[6] }) != code[1];
            bool p2Err = Xor(new bool[] { code[4], code[5], code[6] }) != code[3];
            //Console.WriteLine(new bool[] { p0Err, p1Err, p2Err }.AsString());
            result[0] = code[2] != (p0Err && p1Err && !p2Err);
            result[1] = code[4] != (p0Err && !p1Err && p2Err);
            result[2] = code[5] != (!p0Err && p1Err && p2Err);
            result[3] = code[6] != (p0Err && p1Err && p2Err);
            return new Bits(result);
        }

        public static double CorrectResultChance(double p)
        {
            return Math.Pow(1 - p, 7) + 7 * Math.Pow(1 - p, 6) * p;
        }
        
        /// <param name="word"></param>
        /// <param name="code"></param>
        /// <param name="p"></param>
        /// <returns>The chance of getting a specific code when a specific word is given and the probability of error per bit in the code is p.</returns>
        public static double ResultChance(Bits word, Bits code, params double[] p)
        {
            Bits correctCode = MatrixHammingCode.WordToCode(word);
            if (code.Length != correctCode.Length)
                throw new Exception("code must have the same length as word's code.");
            if (p.Length == 1)
                return Math.Pow(p[0], correctCode.Distance(code)) * Math.Pow(1 - p[0], code.Length - correctCode.Distance(code)); // k(n,r) is not a factor because only a specific selection works.
            else
            {
                if (p.Length != code.Length)
                    throw new Exception("If more than one probability is given, there must be one for each bit.");
                double totalProb = 1;
                for (int i = 0; i < p.Length; i++)
                    totalProb *= code[i] == correctCode[i] ? 1 - p[i] : p[i];
                return totalProb;
            }
        }

        public static double SpecificCodeProbability(Bits code, params double[] p)
        {
            double result = 0;
            for (int i = 0; i < 16; i++)
            {
                var bits = new Bits(i >= 8, i % 8 >= 4, i % 4 >= 2, i % 2 >= 1);
                result += ResultChance(bits, code, p);
            }
            return result / 16;
        }

        public static double WordWhenCode(Bits code, Bits word, params double[] p)
        {
            return ResultChance(word, code, p) * 1/16 / SpecificCodeProbability(code, p) ;
        }
    }

    public static class MatrixHammingCode
    {
        public static Matrix CodeGeneratorMatrix74
        {
            get
            {
                return new Matrix(new double[]
                    { 1, 1, 0, 1,
                      1, 0, 1, 1,
                      1, 0, 0, 0,
                      0, 1, 1, 1,
                      0, 1, 0, 0,
                      0, 0, 1, 0,
                      0, 0, 0, 1 }
                    , 4);
            }
        }

        public static Matrix ParityCheckMatrix74
        {
            get
            {
                return new Matrix(new double[]
                    { 1, 0, 1, 0, 1, 0, 1,
                      0, 1, 1, 0, 0, 1, 1,
                      0, 0, 0, 1, 1, 1, 1 }
                    , 7);
            }
        }

        public static Matrix DecoderMatrix74
        {
            get
            {
                return new Matrix(new double[]
                    { 0, 0, 1, 0, 0, 0, 0,
                      0, 0, 0, 0, 1, 0, 0,
                      0, 0, 0, 0, 0, 1, 0,
                      0, 0, 0, 0, 0, 0, 1 }
                    , 7);
            }
        }

        public static Bits WordToCode(Bits word)
        {
            return CodeGeneratorMatrix74 * word;
        }

        public static Bits CodeToErrors(Bits code)
        {
            return ParityCheckMatrix74 * code;
        }

        public static Bits CodeToWord(Bits code)
        {
            Bits errors = CodeToErrors(code);
            int errorLoc = 0;
            for (int i = 0; i < errors.Length; i++)
                errorLoc += errors[i] ? (int)Math.Pow(2, i) : 0;
            if (errorLoc == 0)
                return DecoderMatrix74 * code;
            else
                return DecoderMatrix74 * code.FlipBits(errorLoc - 1);
        }

        public static void Test(double p, int batchSize, int runs, bool timings = false)
        {
            Data data = new Data(p, batchSize);

            var time = DateTime.Now;

            List<Task> tasks = new List<Task>();

            Console.WriteLine("Expected: " + (1 - HammingCode.CorrectResultChance(p)));

            for (int i = 0; i < runs; i++)
                tasks.Add(Task.Run(() => new Simulation(data, Rand.Next()).ThreadRun()));

            foreach (var t in tasks)
                t.Wait();

            var totalTime = DateTime.Now - time;

            data.Print();
            
            Console.WriteLine("Expected: " + (1 - HammingCode.CorrectResultChance(p)));
            
            if (timings)
            {
                //Console.WriteLine("Times: ");
                //data.PrintTimes();
                Console.WriteLine("Total: " + totalTime);
            }

            Console.ReadLine();
        }
    }
}