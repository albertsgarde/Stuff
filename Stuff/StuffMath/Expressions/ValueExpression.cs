using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions
{
    public class ValueExpression : Expression
    {
        private readonly double value;

        public override double Priority
        {
            get
            {
                return 0;
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public ValueExpression(double value)
        {
            this.value = value;
        }

        public static ValueExpression operator+(ValueExpression v1, ValueExpression v2)
        {
            return new ValueExpression(v1.value + v2.value);
        }

        public static ValueExpression operator-(ValueExpression v1, ValueExpression v2)
        {
            return new ValueExpression(v1.value - v2.value);
        }

        public static ValueExpression operator*(ValueExpression v1, ValueExpression v2)
        {
            return new ValueExpression(v1.value * v2.value);
        }

        public static ValueExpression operator /(ValueExpression v1, ValueExpression v2)
        {
            return new ValueExpression(v1.value / v2.value);
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            return value;
        }

        public override Expression Differentiate(string variable)
        {
            return new ValueExpression(0);
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            return this;
        }

        public override bool IsEqual(Expression exp)
        {
            return (exp is ValueExpression && ((ValueExpression)exp).value == value);
        }

        public override bool ContainsVariable(string variable)
        {
            return false;
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            return vars;
        }

        public override string ToString()
        {
            return "" + value;
        }

        public override string ToTec()
        {
            return "" + value;
        }
    }
}
