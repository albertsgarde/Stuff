using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class BasePhi4
    {
        /// <summary>
        /// An array of the numbers digits. Starts just left of the decimal. Which way it moves is irrelevant as all Phi^4 numbers are symmetrical.
        /// </summary>
        private int[] digits;

        private bool negative;

        public BasePhi4(int number)
        {
            negative = number < 0;
            if (number != 0)
                digits = new int[(int)Math.Ceiling(Math.Log(number) / Math.Log(Math.Pow(Basic.Phi, 4)))];
            else
                digits = new int[1];
            digits[0] = number;
            CorrectDigits();
        }

        private static BasePhi4 BasePhi4IntWithLength(int length)
        {
            BasePhi4 result = new BasePhi4(0);
            result.digits = new int[length];
            return result;
        }

        public int Length
        {
            get
            {
                return digits.Length;
            }
        }

        public static BasePhi4 operator +(BasePhi4 numA, BasePhi4 numB)
        {
            var result = BasePhi4IntWithLength(Math.Max(numA.Length, numB.Length) + 1);
            for (int loop = 0; loop < result.Length; loop++)
            {
                int numADigit = numA.Length > loop ? numA.digits[loop] : 0;
                int numBDigit = numB.Length > loop ? numB.digits[loop] : 0;
                result.AddDigit(loop, numADigit + numBDigit);
            }
            return result.CorrectDigits();
        }

        private BasePhi4 CorrectDigits()
        {
            int lastCorrected = 0;
            int i = 0;
            do
            {
                while (digits[i] >= 7) // 6 < Phi^4 < 7
                {
                    digits[i] -= 7;
                    if (i > 1)
                    {
                        digits[i + 1]++;
                        digits[i - 1]++;
                    }
                    else if (i == 1)
                    {
                        digits[i - 1] += 2; // The middle digit gets an extra to simulate the digits on the other side of the decimal.
                        digits[i + 1]++;
                    }
                    else if (i == 0)
                    {
                        digits[i + 1]++;
                    }
                    else
                        throw new Exception("Digit index should never be negative.");
                    lastCorrected = i;
                }
                if (++i == digits.Length)
                    i = 0;

            }
            while (lastCorrected != i);
            return this;
        }

        private BasePhi4 AddDigit(int digit, int value)
        {
            digits[digit] += value;
            while (digits[digit] >= 7) // 6 < Phi^4 < 7
            {
                digits[digit] -= 7;
                if (digit > 1)
                {
                    digits[digit + 1]++;
                    digits[digit - 1]++;
                }
                else if (digit == 1)
                {
                    digits[digit - 1] += 2; // The middle digit gets an extra to simulate the digits on the other side of the decimal.
                    digits[digit + 1]++;
                }
                else if (digit == 0)
                {
                    digits[digit + 1]++;
                }
                else
                    throw new Exception("Digit index should never be negative.");
            }
            return this;
        }

        public double Calculate()
        {
            double result = digits[0];
            for (int i = 1; i < digits.Length; i++)
                result += digits[i] * (Math.Pow(Basic.Phi, i * 4) + Math.Pow(Basic.Phi, i * -4));
            return result;
        }

        public override string ToString()
        {
            string result = "";
            for (int loop = digits.Length - 1; loop >= 0; loop--)
                result += BaseConverter.decimals[digits[loop]];
            result += ".";
            for (int loop = 1; loop < digits.Length; loop++)
                result += BaseConverter.decimals[digits[loop]];
            return result;
        }
    }
}
