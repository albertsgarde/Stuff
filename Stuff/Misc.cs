using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Stuff
{
    public static class Misc
    {
        public static string EmptyString(int length = 0)
        {
            return UniformString(length, ' ');
        }
        public static string UniformString(int length = 0, char symbol = ' ')
        {
            string result = "";
            for (int i = 0; i < length; ++i)
                result += symbol;
            return result;
        }

        public static bool DoubleEquals(double value1, double value2, double tolerance = 0.00000001)
        {
            return value1 - value2 < tolerance && value2 - value1 < tolerance;
        }

        public static bool FloatEquals(float value1, float value2, float tolerance = 0.000001f)
        {
            return value1 - value2 < tolerance && value2 - value1 < tolerance;
        }

        public static string Flip(this string s)
        {
            string result = "";
            for (int loop = s.Length - 1; loop >= 0; loop--)
                result += s[loop];
            return result;
        }

        public static List<double>[] ReadData(string dataFile)
        {
            StreamReader file = new StreamReader(dataFile);
            string line = file.ReadLine();
            string[] datums = line.Split('	');
            var result = new List<double>[datums.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new List<double>();
                result[i].Add(double.Parse(datums[i]));
            }
            while (!file.EndOfStream)
            {
                line = file.ReadLine();
                datums = line.Split('	');
                for (int i = 0; i < result.Length; i++)
                    result[i].Add(double.Parse(datums[i]));
            }
            file.Close();
            return result;
        }

        public static IEnumerable<KeyValuePair<double, double>> ReadData(string dataFile, int xIndex, int yIndex)
        {
            var data = ReadData(dataFile);
            return data[xIndex].Zip(data[yIndex], (x, y) => new KeyValuePair<double, double>(x, y));
        }

        public static string Indent(this string s, string indent)
        {
            return indent + s.Replace(Environment.NewLine, Environment.NewLine + indent);
        }

        public static IList<T> TopologicalSort<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
        {
            var sorted = new List<T>();
            var visited = new Dictionary<T, bool>();

            foreach (var item in source)
            {
                Visit(item, getDependencies, sorted, visited);
            }

            return sorted;
        }

        private static void Visit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted, Dictionary<T, bool> visited)
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item, out inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    throw new ArgumentException("Cyclic dependency found.");
                }
            }
            else
            {
                visited[item] = true;

                var dependencies = getDependencies(item);
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)
                    {
                        Visit(dependency, getDependencies, sorted, visited);
                    }
                }

                visited[item] = false;
                sorted.Add(item);
            }
        }

        private const long DATE_TIME_TICKS_PER_MICRO = 10;

        public static long Microseconds(this DateTime dt)
        {
            return dt.Ticks / DATE_TIME_TICKS_PER_MICRO;
        }

        public static long Milliseconds(this DateTime dt)
        {
            return dt.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
