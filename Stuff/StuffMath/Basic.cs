using Stuff.StuffMath.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public static class Basic
    {
        public static double Phi
        {
            get
            {
                return (1 + Math.Sqrt(5)) / 2;
            }
            private set
            {
                Phi = value;
            }
        }

        public static double Norm(double d)
        {
            return d < 0 ? d * -1 : d;
        }

        public static float Norm(float f)
        {
            return f < 0 ? f * -1 : f;
        }

        public static double Round(double d, double rounder)
        {
            return d % rounder > rounder / 2 ? d - (d % rounder) + rounder : d - (d % rounder);
        }

        public static double Round(double d, int power)
        {
            return ((d / Math.Pow(10, power) % 1 < 0.5) ? Math.Floor(d / Math.Pow(10, power)) : Math.Ceiling(d / Math.Pow(10, power))) * Math.Pow(10, power);
        }

        public static string ToOrdinal(this long l)
        {
            if (l < 0)
                return "" + l;
            else
            {
                long rem = l % 100;
                if (rem >= 11 && rem <= 13)
                    return l + "th";
                else
                {
                    rem = l % 10;
                    if (rem == 1)
                        return l + "st";
                    else if (rem == 2)
                        return l + "nd";
                    else if (rem == 3)
                        return l + "rd";
                    else
                        return l + "th";
                }
            }
        }

        public static string ToOrdinal(this int i)
        {
            if (i < 0)
                return "" + i;
            else
            {
                long rem = i % 100;
                if (rem >= 11 && rem <= 13)
                    return i + "th";
                else
                {
                    rem = i % 10;
                    if (rem == 1)
                        return i + "st";
                    else if (rem == 2)
                        return i + "nd";
                    else if (rem == 3)
                        return i + "rd";
                    else
                        return i + "th";
                }
            }
        }

        public static long Factorial(long num)
        {
            if (num == 0)
                return 1;
            if (num < 0)
                throw new Exception("Cannot take the factorial of a negative number.");
            long result = num;
            for (long l = num - 1; l > 0; l--)
                result *= l;
            return result;
        }

        public static int Factorial(int num)
        {
            if (num == 0)
                return 1;
            if (num < 0)
                throw new Exception("Cannot take the factorial of a negative number.");
            int result = num;
            for (int i = num - 1; i > 0; i--)
                result *= i;
            return result;
        }

        public static double Heron(double a, double b, double c)
        {
            double s = (a + b + c) / 2;
            return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
        }

        public static long BinomialCoefficient(long n, long r)
        {
            return Factorial(n) / (Factorial(r) * (Factorial(n - r)));
        }

        public static bool IsPrime(int num)
        {
            if (num == 1)
                return false;
            else if (num == 2)
                return true;
            for (int i = 3; i < Math.Sqrt(num) + 1; i++)
            {
                if (num % i == 0)
                    return false;
            }
            return true;
        }

        public static int GCD(int a, int b)
        {
            if (b == 0)
                return a;
            else
                return GCD(b, Mod(a,b));
        }

        public static int Mod(int a, int b)
        {
            return a - b * (int)Math.Floor((double)a / (double)b);
        }
    }
}
