using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff
{
    public static class Parse
    {
        public static int ParseInt(string s, string errorMessage)
        {
            if (!int.TryParse(s, out int result))
                throw new InvalidCastException(errorMessage);
            else return result;
        }

        public static int ParseInt(string s) => ParseInt(s, s + " is not an int");

        public static float ParseFloat(string s, string errorMessage)
        {
            if (!float.TryParse(s, out float result))
                throw new InvalidCastException(errorMessage);
            else return result;
        }

        public static float ParseFloat(string s) => ParseFloat(s, s + " is not an float");

        public static double ParseDouble(string s, string errorMessage)
        {
            if (!double.TryParse(s, out double result))
                throw new InvalidCastException(errorMessage);
            else return result;
        }

        public static double ParseDouble(string s) => ParseDouble(s, s + " is not an double");
    }
}
