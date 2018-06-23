using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff
{
    public class Lottery
    {
        public static ulong chanceToWin(ulong winners, ulong total)
        {
            ulong result = 1;
            ulong totalWinners = 1;
            while (winners > 0)
            {
                result *= total;
                totalWinners *= winners;
                total--;
                winners--;
            }
            return result/totalWinners;
        }
    }
}
