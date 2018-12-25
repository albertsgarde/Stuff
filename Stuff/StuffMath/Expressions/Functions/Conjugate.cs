using Stuff.StuffMath.Expressions.Operators;
using Stuff.StuffMath.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Functions
{
    public class Conjugate : Expression
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

        public Conjugate(Expression arg)
        {
            this.arg = arg;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            return arg.Evaluate();
        }

        public override Expression Differentiate(string variable)
        {
            throw new NotDifferentiableException();
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            return arg;
        }

        public override bool IsEqual(Expression exp) => exp is Conjugate con ? arg.IsEqual(con.arg) : false;

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
            return "conjugate(" + arg.ToString() + ")";
        }

        public override string ToLatex()
        {
            return "\\overline{" + arg.ToLatex() + "}";
        }
    }
}
