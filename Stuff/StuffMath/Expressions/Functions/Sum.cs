using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Functions
{
    public class Sum : Expression
    {
        private readonly Expression body;

        private readonly Expression min;

        private readonly Expression max;

        private readonly string iterator;

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

        /// <param name="body">The expression to sum.</param>
        /// <param name="min">The start value. May not contain the iterator.</param>
        /// <param name="max">The max value. May not contain the iterator.</param>
        /// <param name="iterator">The name of the variable to iterate.</param>
        public Sum(Expression body, Expression min, Expression max, string iterator)
        {
            this.body = body;
            this.min = min;
            this.max = max;
            this.iterator = iterator;
        }

        /// <param name="body">The expression to sum.</param>
        /// <param name="min">The start value. May not contain the iterator.</param>
        /// <param name="max">The max value. May not contain the iterator.</param>
        /// <param name="iterator">The name of the variable to iterate. Must be a variable expression.</param>
        public Sum(Expression body, Expression min, Expression max, Expression iterator)
        {
            this.body = body;
            this.min = min;
            this.max = max;
            if (!(iterator is Variable))
                throw new ArgumentException("The iterator argument must be of type Variable.");
            this.iterator = ((Variable)iterator).ToString(); // Is very naughty.
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            if (values != null && values.ContainsKey(iterator))
                throw new Exception("A sum cannot be evaluated if its iterator variable has been assigned.");
            double result = 0;
            var newValues = values == null ? new Dictionary<string, double>() : values.Copy();
            var minVal = min.Evaluate(values);
            var maxVal = max.Evaluate(values);
            for (double d = minVal; d <= maxVal; d++)
            {
                newValues[iterator] = d;
                Console.WriteLine(result += body.Evaluate(newValues));
            }
            return result;
        }

        public override Expression Differentiate(string variable)
        {
            if (min.ContainsVariable(variable))
                throw new Exception("Cannot differentiate a sum function in a variable, if the sum functions minimum value's expression contains that variable.");
            else if (max.ContainsVariable(variable))
                throw new Exception("Cannot differentiate a sum function in a variable, if the sum functions maximum value's expression contains that variable.");
            return new Sum(body.Differentiate(variable), min, max, iterator);
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            return new Sum(body.Reduce(values), min.Reduce(values), max.Reduce(values), iterator);
        }

        public override bool IsEqual(Expression exp)
        {
            if (exp is Sum)
            {
                var sum = (Sum)exp;
                return body.IsEqual(sum.body) && min.IsEqual(sum.min) && max.IsEqual(sum.max) && iterator == sum.iterator;
            }
            return false;
        }

        public override bool ContainsVariable(string variable)
        {
            return body.ContainsVariable(variable) || min.ContainsVariable(variable) || max.ContainsVariable(variable) || iterator == variable;
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            vars.Add(iterator);
            return body.ContainedVariables(min.ContainedVariables(max.ContainedVariables(vars)));
        }

        public override string ToString()
        {
            return "sum(" + body + ", " + min + ", " + max + ", " + iterator + ")";
        }

        public override string ToLatex()
        {
            return "\\displaystyle\\sum_{" + iterator + " = " + min.ToLatex() + "}^{" + max.ToLatex() + "}" + body.ToLatex();
        }
    }
}
