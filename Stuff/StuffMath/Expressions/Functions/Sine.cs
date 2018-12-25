using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Functions
{
    public class Sine : Expression
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

        public Sine(Expression arg)
        {
            this.arg = arg;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            return Math.Sin(arg.Evaluate(values));
        }

        public override Expression Differentiate(string variable)
        {
            return new Cosine(arg) * arg.Differentiate(variable);
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            Expression argReduced = arg.Reduce(values);
            if (argReduced is ValueExpression)
                return new ValueExpression(Math.Sin(argReduced.Evaluate()));
            else
                return new Sine(argReduced);
        }

        public override bool IsEqual(Expression exp)
        {
            return exp is Sine && ((Sine)exp).arg.IsEqual(arg);
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
            return "sin(" + arg.ToString() + ")";
        }

        public override string ToLatex()
        {
            return "sin(" + arg.ToLatex() + ")";
        }
    }
}
