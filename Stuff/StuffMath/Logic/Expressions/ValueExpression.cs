using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic.Expressions
{
    public class ValueExpression : Expression
    {
        private readonly bool value;

        public override double Priority => 0;

        public ValueExpression(bool value)
        {
            this.value = value;
        }

        public override bool Evaluate(Dictionary<string, bool> values = null)
        {
            return value;
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            return vars;
        }

        public override bool ContainsVariable(string variable)
        {
            return false;
        }

        public override Expression Reduce(Dictionary<string, bool> values = null)
        {
            return this;
        }

        public override Expression ToNormalForm()
        {
            return this;
        }

        public override Expression Negate()
        {
            return !value;
        }

        public override string ToString()
        {
            return value ? "1" : "0";
        }

        public override string ToLatex()
        {
            return value ? "\\top" : "\\bot";
        }
    }
}
