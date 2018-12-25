using Stuff.StuffMath.Expressions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Functions
{
    public class Ln : Expression
    {
        private Expression arg;

        public override double Priority
        {
            get
            {
                return 4;
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public Ln(Expression arg)
        {
            this.arg = arg;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            return Math.Log(arg.Evaluate(values));
        }

        public override Expression Differentiate(string variable)
        {
            return new Power(arg, new ValueExpression(-1)) * arg.Differentiate(variable);
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            Expression argReduced = arg.Reduce(values);
            if (argReduced is ValueExpression)
                return new ValueExpression(Math.Log(argReduced.Evaluate()));
            else
                return new Ln(argReduced);
        }

        public override bool IsEqual(Expression exp)
        {
            return exp is Ln && ((Ln)exp).arg.IsEqual(arg);
        }

        public override bool ContainsVariable(string variable)
        {
            return arg.ContainsVariable(variable);
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            return arg.ContainedVariables(vars);
        }

        public override string ToString()
        {
            return "ln(" + arg.ToString() + ")";
        }

        public override string ToLatex()
        {
            throw new NotImplementedException();
        }
    }
}
