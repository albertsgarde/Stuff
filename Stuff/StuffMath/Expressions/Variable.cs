using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions
{
    public class Variable : Expression
    {
        private readonly string name;

        public override double Priority
        {
            get
            {
                return 0;
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public Variable(string name)
        {
            this.name = name;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            if (values == null || !values.ContainsKey(name))
                throw new Exception("Cannot evaluate an expression containing a variable without asigning the variable a value.");
            else
                return values[name];
        }

        public override Expression Differentiate(string variable)
        {
            return variable == name ? (Expression)new ValueExpression(1) : this;
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            if (values != null && values.ContainsKey(name))
                return values[name];
            else
                return this;
        }

        public override bool IsEqual(Expression exp)
        {
            return exp is Variable && ((Variable)exp).name == name;
        }

        public override bool ContainsVariable(string variable)
        {
            return name == variable;
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            vars.Add(name);
            return vars;
        }

        public override string ToString()
        {
            return name;
        }

        public override string ToLatex()
        {
            return name;
        }
    }
}
