using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;

namespace Stuff.StuffMath
{
    static public class BaseConverter
    {
        public const string decimals = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string ToDecimal(string number, int numberBase)
        {
            if (numberBase <= 1 && numberBase > 36)
                throw new Exception("Invalid number base " + numberBase);
            string flippedNumber = number.Flip();
            int result = 0;
            for (int loop = 0; loop < flippedNumber.Length; loop++)
            {
                int curDecimal = decimals.IndexOf(flippedNumber[loop]);
                if (curDecimal >= numberBase)
                    throw new Exception("Invalid decimal for number base.");
                result += curDecimal * (int)Math.Pow(numberBase, loop);
            }
            return "" + result;
        }

        public static string ToPhi4(int number)
        {
            return new BasePhi4(number).ToString();
        }

        public static string ToPhi(int number)
        {
            return new BasePhi1(number).ToString();
        }
        /*
        public string FromDecimal(string number, int numberBase)
        {
            if (numberBase <= 1 && numberBase > 36)
                throw new Exception("Invalid number base " + numberBase);
            int result = 0;
            for (int loop = 0; loop < number.Length; loop++)
            {
                int curDecimal = decimals.IndexOf(number[loop]);
                if (curDecimal >= numberBase)
                    throw new Exception("Invalid decimal for number base.");
                result += curDecimal * (int)Math.Pow(numberBase, loop);
            }
            return "" + result;
        }

        public string FromDecimal(int number, int numberBase)
        {
            return FromDecimal("" + number, numberBase);
        }*/
    }
}
