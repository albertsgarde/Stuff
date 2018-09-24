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
        public IReadOnlyList<string> Variables { get; }

        public IReadOnlyList<IReadOnlyList<bool>> Values { get; }

        private readonly IReadOnlyList<TruthAssignment> rows;

        public ValueTable(IReadOnlyList<string> variables, IReadOnlyList<IReadOnlyList<bool>> values)
        {
            Variables = new List<string>(variables);
            var tempValues = new List<List<bool>>();

            var rows = new List<TruthAssignment>();
            
            for (int i = 0; i < values.Count; ++i)
            {
                Debug.Assert(values[i].Count == variables.Count);
                tempValues.Add(new List<bool>(values[i]));
                rows.Add(new TruthAssignment(variables, values[i]));
            }
            Values = tempValues;
            this.rows = rows;
        }

        private class TruthAssignment
        {
            private Dictionary<string, bool> values;

            public TruthAssignment(IReadOnlyList<string> variables, IReadOnlyList<bool> values)
            {
                Debug.Assert(variables.Count == values.Count);
                this.values = new Dictionary<string, bool>();
                for (int i = 0; i < variables.Count; ++i)
                    this.values[variables[i]] = values[i];
            }

            public bool this[string variable] => values[variable];

            public bool Agrees(TruthAssignment ta)
            {
                foreach(var key in values.Keys)
                {
                    if (ta.ContainsVar(key) && ta[key] != values[key])
                        return false;
                }
                return true;
            }

            public bool ContainsVar(string var)
            {
                return values.ContainsKey(var);
            }
        }

        private bool Valid(TruthAssignment ta)
        {
            foreach (var thisTa in rows)
            {
                if (thisTa.Agrees(ta))
                    return true;
            }
            return false;
        }

        public string ToLatex()
        {
            return ToLatex(Variables.ToDictionary(v => v, v => v));
        }

        public string ToLatex(Dictionary<string, string> latexVarNames)
        {
            foreach(var name in Variables)
            {
                if (!latexVarNames.ContainsKey(name))
                    latexVarNames[name] = name;
            }

            var result = "\\begin{tabular}{|";
            for (int i = 0; i < Variables.Count; ++i)
                result += "c|";
            result += "}\n";
            result += "\\hline\n";
            foreach (var name in Variables)
                result += "$" + latexVarNames[name] + "$ & ";
            result = result.Substring(0, result.Length - 3) + "\\\\\n";
            result += "\\hline\n";
            foreach (var values in Values)
            {
                var line = "";
                foreach (var value in values)
                    line += (value ? "\\top" : "\\bot") + "&";
                result += line.Substring(0, line.Length - 1) + "\\\\\n";
                result += "\\hline\n";
            }
            result += "\\end{tabular}\\\\";
            return result;
        }

        public override string ToString()
        {
            var result = "|";
            for (int i = 0; i < Variables.Count; ++i)
                result += Variables[i] + "|";
            result += Environment.NewLine;
            for (int i = 0; i < Values.Count; ++i)
            {
                Debug.Assert(Values[i].Count == Variables.Count);
                result += "|";
                for (int j = 0; j < Variables.Count; ++j)
                {
                    result += Values[i][j] ? "1" : "0";
                    result += Misc.EmptyString(Variables[j].Length - 1);
                    result += "|";
                }
                result += Environment.NewLine;
            }
            return result;
        }
    }
}
