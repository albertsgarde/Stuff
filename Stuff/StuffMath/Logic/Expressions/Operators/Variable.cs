using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;

namespace Stuff.StuffMath.Logic.Expressions.Operators
{
    public class Variable : Expression
    {
        public override string Name => "Variable";

        private readonly string name;

        public override double Priority => 0;

        public Variable(string name)
        {
            this.name = name;
        }

        public override bool Evaluate(Dictionary<string, bool> values)
        {
            if (values == null || !values.ContainsKey(name))
                throw new Exception("Cannot evaluate an expression containing a variable without assigning the variable a value.");
            else
                return values[name];
        }

        public override Expression Reduce(Dictionary<string, bool> values = null)
        {
            if (values == null || !values.ContainsKey(name))
                return this;
            else
                return values[name];
        }

        public override Expression ToNormalForm()
        {
            return this;
        }

        public override Expression Negate()
        {
            return new Not(this);
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            vars.Add(name);
            return vars;
        }

        public override bool ContainsVariable(string variable)
        {
            return variable == name;
        }

        protected override bool InternalTableau(IReadOnlyList<(Expression exp, bool value)> expressions, IReadOnlyDictionary<string, bool> values, bool value)
        {
            var newValues = values.ToDictionary();
            if (values.ContainsKey(name))
            {
                if (values[name] != value)
                    return false;
            }
            else
                newValues[name] = value;

            if (expressions.Count == 0)
                return true;
            else
            {
                return InternalTableauNextExp(expressions, newValues);
            }
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
