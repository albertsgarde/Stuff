using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Stuff.StuffMath.Expressions
{
    public class FunctionManager
    {
        private List<Function> functions;

        private List<Operator> operators;

        public FunctionManager(params string[] functionDirs)
        {
            functions = new List<Function>();
            operators = new List<Operator>();
            foreach (string directory in functionDirs)
            {
                foreach (string functionFile in (from filePath in Directory.EnumerateFiles(directory, "*.xml", SearchOption.AllDirectories)
                                                    where !filePath.Substring(filePath.LastIndexOf('/')).StartsWith("!")
                                                    select filePath))
                {
                    LoadFile(functionFile);
                }
            }
            LoadFile("Assets/ExpressionFunctions/BasicMath.xml");
            LoadFile("Assets/ExpressionOperators/BasicMath.xml");
        }

        private void LoadFile(string file)
        {
            foreach (XElement element in XElement.Load(file).Elements("function"))
            {
                functions.Add(Expressions.Function.Load(element));
            }
            foreach (XElement element in XElement.Load(file).Elements("operator"))
            {
                operators.Add(Expressions.Operator.Load(element));
            }
        }

        public IEnumerable<Function> Functions
        {
            get
            {
                return functions;
            }
        }

        public IEnumerable<Operator> Operators
        {
            get
            {
                return operators;
            }
        }

        public Function Function(string alias)
        {
            return functions.First(f => f.Aliases.Contains(alias));
        }

        public Operator Operator(string symbol)
        {
            return operators.First(o => o.Symbol == symbol);
        }
    }
}
