using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stuff
{
    public class Rand
    {
        [ThreadStatic]
        private static Random local;

        public static Random ThisThreadsRandom
        {
            get { return local ?? (local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }

        /// <returns>A randomly generated integer.</returns>
        public static int Next()
        {
            return ThisThreadsRandom.Next();
        }

        /// <param name="exclusiveMaxValue">Maximum value of the randomly generated result.</param>
        /// <returns>A randomly generated integer from 0 inclusively to exclusiveMaxValue exclusively</returns>
        public static int Next(int exclusiveMaxValue)
        {
            return ThisThreadsRandom.Next(0, exclusiveMaxValue);
        }

        /// <param name="inclusiveMinValue">Minimum value of the randomly generated result.</param>
        /// <param name="exclusiveMaxValue">Maximum value of the randomly generated result.</param>
        /// <returns>A randomly generated integer from inclusiveMinValue inclusively to exclusiveMaxValue exclusively</returns>
        public static int Next(int inclusiveMinValue, int exclusiveMaxValue)
        {
            return ThisThreadsRandom.Next(inclusiveMinValue, exclusiveMaxValue);
        }

        /// <returns>A random double between 0.0 and 1.0.</returns>
        public static double NextDouble()
        {
            return ThisThreadsRandom.NextDouble();
        }
        
        /// <returns>A randomly generated boolean.</returns>
        public static bool NextBool()
        {
            return ThisThreadsRandom.Next(0, 2) == 1 ? true : false;
        }

        public static Random Random
        {
            get
            {
                return ThisThreadsRandom;
            }
        }

        public static int Roll(string roll)
        {
            string[] commands = roll.Split('d');
            int maxRoll = int.Parse(commands[1]);
            LinkedList<int> rolls = new LinkedList<int>();
            int numRolls = int.Parse(commands[0]);
            for (int i = 0; i < numRolls; i++)
                rolls.AddLast(Next(1, maxRoll + 1));
            if (commands.Length == 3)
            {
                int drops = int.Parse(commands[2]);
                for (int i = 0; i < drops; i++)
                    rolls.Remove(rolls.Min());
            }
            int result = 0;
            foreach (int i in rolls)
                result += i;
            return result;
        }
    }
}
