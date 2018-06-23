using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Statistics
{
    public static class StatsUtils
    {
        public static double Mean(this IEnumerable<double> list)
        {
            double result = 0;
            foreach (var d in list)
                result += d;
            return result / list.Count();
        }

        public static double Median(this IEnumerable<double> list)
        {
            int i = 0;
            int iMax = (list.Count()+1) / 2;
            bool even = list.Count() % 2 == 0;
            double save = 0;
            foreach (var d in list.OrderBy(d => d))
            {
                if (++i >= iMax)
                {
                    if (!even)
                        return d;
                    else if (i == iMax)
                        save = d;
                    else
                        return (save + d) / 2;
                }
            }
            throw new Exception("List is empty!");
        }
    }
}
