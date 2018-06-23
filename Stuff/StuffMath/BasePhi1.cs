using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;

namespace Stuff.StuffMath
{
    public class BasePhi1
    {
        private CenteredArray<int> digits;

        public int MaxPower { get; private set; }

        public int MinPower { get; private set; }

        public BasePhi1(int number)
        {
            var numDigits = (int)Math.Ceiling(Math.Log(number) / Math.Log(Basic.Phi))*2;
            digits = new CenteredArray<int>(numDigits + 1, numDigits + 1);
            digits[-1] = number;
            MaxPower = numDigits;
            MinPower = -numDigits - 1;
            Console.WriteLine(ToString());
            CorrectDigits();
        }

        private void CorrectDigits()
        {
            int lastCorrected = 0;
            int i = digits.Start;
            do
            {
                while (digits[i] >= 2) // 1 < Phi < 2
                {
                    digits[i] -= 2;
                    digits[i - 1]++;
                    digits[i + 2]++;
                    lastCorrected = i;
                }
                while (digits[i] == 1 && digits[i + 1] > 0)
                {
                    digits[i]--;
                    digits[i + 1]--;
                    digits[i - 1]++;
                    lastCorrected = i;
                }
                //Console.WriteLine(digits.AsString());
                if (++i == digits.End)
                    i = digits.Start;

            }
            while (lastCorrected != i);
        }

        public double Calculate()
        {
            double result = 0;
            for (int i = -MaxPower - 1; i < -MinPower; i++)
            {
                result += digits[i] * Math.Pow(Basic.Phi, -i - 1);
            }
            return result;
        }

        public override string ToString()
        {
            string result = "";
            for (int loop = digits.Start; loop < 0; loop++)
                result += digits[loop];
            result += ".";
            for (int loop = 0; loop < digits.End; loop++)
                result += digits[loop];
            return result;
        }
    }
}
