using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Functions
{
    public class Factorial : Expression
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

        public Factorial(Expression arg)
        {
            this.arg = arg;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            double argValue = arg.Evaluate((values));
            if (argValue % 1 != 0)
                throw new Exception("factorial only takes integer arguments. arg = " + argValue);
            return Basic.Factorial((long)argValue);
        }

        public override Expression Differentiate(string variable)
        {
            throw new Exception("Cannot differentiate factorial");
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            Expression argReduced = arg.Reduce(values);
            if (argReduced is ValueExpression)
                return new ValueExpression(Evaluate(values));
            else
                return new Factorial(argReduced);
        }

        public override bool IsEqual(Expression exp)
        {
            if (exp is Factorial)
            {
                var factorial = (Factorial)exp;
                return factorial.arg.IsEqual(arg);
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
            return "factorial(" + arg.ToString() + ")";
        }

        public override string ToLatex()
        {
            return arg.ToString() + "!";
        }
    }
}
