using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Hamming
{
    public class Data
    {
        public double P { get; private set; }

        public int BatchSize { get; private set; }

        private List<DataBatch> dataBatches;

        private List<TimeSpan> times;

        private object addLoc = new object();

        public Data(double p, int batchSize)
        {
            P = p;
            BatchSize = batchSize;
            dataBatches = new List<DataBatch>();
            times = new List<TimeSpan>();
        }

        public class DataBatch
        {
            public int Total { get; private set; }

            public int Failures { get; private set; }

            public DataBatch(int total, int failures)
            {
                Total = total;
                Failures = failures;
            }

            public double Ratio()
            {
                return (double)Failures / Total;
            }

            public override string ToString()
            {
                return "" + Failures + "/" + Total + " = " + Ratio();
            }
        }

        public IEnumerable<DataBatch> DataBatches()
        {
            return dataBatches;
        }

        public void AddBatch(DataBatch batch)
        {
            dataBatches.Add(batch);
        }

        public void AddTime(TimeSpan time)
        {
            times.Add(time);
        }

        public int Failures()
        {
            return dataBatches.Select(x => x.Failures).Sum();
        }

        public int Total()
        {
            return dataBatches.Select(x => x.Total).Sum();
        }

        public double TotalRatio()
        {
            return (double)Failures() / Total();
        }

        public void PrintTimes()
        {
            foreach (var time in times)
                Console.WriteLine(time);
        }

        public void Print()
        {
            foreach (var batch in dataBatches)
                Console.WriteLine(batch);
            Console.WriteLine("Total: " + Failures() + "/" + Total() + " = " + TotalRatio());
        }
    }
}
