using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Functions
{
    public class Derivative : Expression
    {
        private readonly Expression exp;

        private readonly string var;

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

        /// <param name="exp">The expression to differentiate.</param>
        /// <param name="var">The variable name to differentiate for.</param>
        public Derivative(Expression exp, string var)
        {
            this.exp = exp;
            this.var = var;
        }

        /// <param name="exp">The expression to differentiate.</param>
        /// <param name="var">The variable name to differentiate for. Must be a variable expression.</param>
        public Derivative(Expression exp, Expression var)
        {
            this.exp = exp;
            if (!(var is Variable))
                throw new ArgumentException("The var argument must be of type Variable.");
            this.var = ((Variable)var).ToString(); // Is very naughty.
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            return exp.Differentiate(var).Evaluate(values);
        }

        public override Expression Differentiate(string variable)
        {
            return new Derivative(exp.Differentiate(var), var);
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            return exp.Differentiate(var).Reduce();
        }

        public override bool IsEqual(Expression exp)
        {
            if (exp is Derivative)
            {
                var derivative = (Derivative)exp;
                return this.exp.IsEqual(exp) && var == derivative.var;
            }
            else
                return this.exp.Differentiate(var).IsEqual(exp);
        }

        public override bool ContainsVariable(string variable)
        {
            return exp.ContainsVariable(variable) || var == variable;
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            vars.Add(var);
            return exp.ContainedVariables(vars);
        }

        public override string ToString()
        {
            return "derivative(" + exp + ", " + var + ")";
        }

        public override string ToTec()
        {
            return "\\frac{d}{d" + var + "}(" + exp.ToTec() + ")";
        }
    }
}
