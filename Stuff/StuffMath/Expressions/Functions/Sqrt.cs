using Stuff.StuffMath.Expressions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Functions
{
    public class Sqrt : Expression
    {
        private Expression arg;

        public override double Priority
        {
            get
            {
                return 1;
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public Sqrt(Expression arg)
        {
            this.arg = arg;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            return Math.Sqrt(arg.Evaluate(values));
        }

        public override Expression Differentiate(string variable)
        {
            return new Power(arg*0.5, -0.5) * arg.Differentiate(variable);
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            Expression argReduced = arg.Reduce(values);
            if (argReduced is ValueExpression)
                return new ValueExpression(Math.Sqrt(argReduced.Evaluate()));
            else if (argReduced is Power && ((Power)argReduced).Right is ValueExpression && ((Power)argReduced).Right.Evaluate() == 2)
                return ((Power)argReduced).Left;
            else
                return new Sqrt(argReduced);
        }

        public override bool IsEqual(Expression exp)
        {
            return (exp is Sqrt && ((Sqrt)exp).arg.IsEqual(arg)) || (exp is Power && ((Power)exp).Right is ValueExpression && ((Power)exp).Right.Evaluate() == 0.5);
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
            return "sqrt(" + arg.ToString() + ")";
        }

        public override string ToLatex()
        {
            return "\\sqrt{" + arg.ToLatex() + "}";
        }
    }
}
