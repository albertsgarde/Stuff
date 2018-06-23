using Stuff.StuffMath.Expressions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Functions
{
    public class Exp : Expression
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

        public Exp(Expression arg)
        {
            this.arg = arg;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            return Math.Exp(arg.Evaluate(values));
        }

        public override Expression Differentiate(string variable)
        {
            return this * arg.Differentiate(variable);
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            Expression argReduced = arg.Reduce(values);
            if (argReduced is ValueExpression)
                return new ValueExpression(Math.Exp(argReduced.Evaluate()));
            else
                return new Exp(argReduced);
        }

        public override bool IsEqual(Expression exp)
        {
            return (exp is Exp && ((Exp)exp).arg.IsEqual(arg)) || (exp is Power && ((Power)exp).Left is ValueExpression && ((Power)exp).Left.Evaluate() == Math.E);
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
            return "e^" + arg.ToString();
        }

        public override string ToTec()
        {
            throw new NotImplementedException();
        }
    }
}
