using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic
{
    public class ValueTable
    {
        private readonly string[] variables;

        private readonly bool[][] values;

        public ValueTable(string[] variables, bool[][] values)
        {
            this.variables = new string[variables.Length];
            variables.CopyTo(this.variables, 0);

            this.values = new bool[values.Length][];
            for (int i = 0; i < values.Length; ++i)
            {
                Debug.Assert(values[i].Length == variables.Length);
                this.values[i] = new bool[values[i].Length];
                values[i].CopyTo(this.values[i], 0);
            }
        }

        public override string ToString()
        {
            var result = "|";
            for (int i = 0; i < variables.Length; ++i)
                result += variables[i] + "|";
            result += Environment.NewLine;
            for (int i = 0; i < values.Length; ++i)
            {
                Debug.Assert(values[i].Length == variables.Length);
                result += "|";
                for (int j = 0; j < variables.Length; ++j)
                {
                    result += values[i][j] ? "1" : "0";
                    result += Misc.EmptyString(variables[j].Length - 1);
                    result += "|";
                }
                result += Environment.NewLine;
            }
            return result;
        }
    }
}
