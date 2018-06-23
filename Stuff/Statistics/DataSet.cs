using Stuff.Statistics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff
{
    public class DataSet
    {
        private static readonly char[] SEPERATION_CHARS = { ' ', ',', '	' };

        private readonly List<Dictionary<string, Value>> data;

        private readonly string[] dataNames;

        public DataSet(string fileName)
        {
            data = new List<Dictionary<string, Value>>();

            StreamReader file = new StreamReader(fileName);
            string line = file.ReadLine();
            string[] datums = line.Split(SEPERATION_CHARS);

            dataNames = datums;

            data = ReadData(file, dataNames, ReadUncertainties(file, dataNames));

            file.Close();
        }

        public DataSet(StreamReader file)
        {
            string line = file.ReadLine();
            string[] datums = line.Split(SEPERATION_CHARS);
            
            dataNames = datums;
        }

        private static Dictionary<string, double> ReadUncertainties(StreamReader file, string[] dataNames)
        {
            var uncertainties = new Dictionary<string, double>();
            if ((char)file.Peek() == 'u')
            {
                string line = file.ReadLine().Substring(1);
                string[] datums = line.Split(SEPERATION_CHARS);
                if (datums.Length != dataNames.Length)
                    throw new Exception("Not enough uncertainties.");
                for (int i = 0; i < dataNames.Length; i++)
                    uncertainties[dataNames[i]] = double.Parse(datums[i]);
            }
            else
            {
                for (int i = 0; i < dataNames.Length; i++)
                    uncertainties[dataNames[i]] = 0;
            }
            return uncertainties;
        }

        private static List<Dictionary<string, Value>> ReadData(StreamReader file, string[] dataNames, Dictionary<string, double> uncertainties)
        {
            var data = new List<Dictionary<string, Value>>();
            while (!file.EndOfStream)
            {
                var line = file.ReadLine();
                if (line == "")
                    return data;
                data.Add(StringToDataPoint(line, dataNames, uncertainties));
            }
            return data;
        }

        private static Dictionary<string, Value> StringToDataPoint(string s, string[] dataNames, Dictionary<string, double> uncertainties)
        {
            var result = new Dictionary<string, Value>();
            var datums = s.Split(SEPERATION_CHARS);
            for (int i = 0; i < datums.Length; i++)
                result[dataNames[i]] = new Value(double.Parse(datums[i]), uncertainties[dataNames[i]]);
            return result;
        }

        public List<KeyValuePair<double, double>> ToRegressionable(Func<Dictionary<string, Value>, double> x, Func<Dictionary<string, Value>, double> y)
        {
            var result = new List<KeyValuePair<double, double>>();
            foreach(var dataPoint in data)
                result.Add(new KeyValuePair<double, double>(x.Invoke(dataPoint), y.Invoke(dataPoint)));
            return result;
        }
    }
}
