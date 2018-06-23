using Stuff.StuffMath.Expressions.Operators;
using System;
using System.Collections.Generic;

namespace Stuff.StuffMath.Expressions.Functions
{
    public class Tan : Expression
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

        public Tan(Expression arg)
        {
            this.arg = arg;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            return Math.Tan(arg.Evaluate(values));
        }

        public override Expression Differentiate(string variable)
        {
            return 1 / new Power(new Cosine(arg), 2) * arg.Differentiate(variable);
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
            return exp is Tan && ((Tan)exp).arg.IsEqual(arg);
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
            return "tan(" + arg.ToString() + ")";
        }

        public override string ToTec()
        {
            throw new NotImplementedException();
        }
    }
}
