using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Functions
{
    public class Cosine : Expression
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

        public Cosine(Expression arg)
        {
            this.arg = arg;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            return Math.Cos(arg.Evaluate(values));
        }

        public override Expression Differentiate(string variable)
        {
            return 0 - new Sine(arg) * arg.Differentiate(variable);
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            Expression argReduced = arg.Reduce(values);
            if (argReduced is ValueExpression)
                return new ValueExpression(Math.Cos(argReduced.Evaluate()));
            else
                return new Cosine(argReduced);
        }

        public override bool IsEqual(Expression exp)
        {
            if (exp is Cosine)
            {
                var cosine = (Cosine)exp;
                return cosine.arg.IsEqual(arg);
            }
            return false;
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
            return "cos(" + arg.ToString() + ")";
        }

        public override string ToTec()
        {
            throw new NotImplementedException();
        }
    }
}
