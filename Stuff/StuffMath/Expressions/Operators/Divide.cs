using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Operators
{
    public class Divide : Expression
    {
        private Expression left;

        private Expression right;

        public override double Priority
        {
            get
            {
                return 2;
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public Divide(Expression leftHand, Expression rightHand)
        {
            this.left = leftHand;
            this.right = rightHand;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            return left.Evaluate(values) / right.Evaluate(values);
        }

        public override Expression Differentiate(string variable)
        {
            return (left.Differentiate(variable) * right - left * right.Differentiate(variable)) / (right * right);
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            Expression leftReduced = left.Reduce(values);
            Expression rightReduced = right.Reduce(values);
            if (leftReduced is ValueExpression && rightReduced is ValueExpression)
                return new ValueExpression(leftReduced.Evaluate() / rightReduced.Evaluate());
            else if (leftReduced is ValueExpression && leftReduced.Evaluate() == 1)
                return rightReduced;
            else if (rightReduced is ValueExpression && rightReduced.Evaluate() == 1)
                return leftReduced;
            else
                return leftReduced / rightReduced;
        }

        public override bool IsEqual(Expression exp)
        {
            if (exp is Divide)
            {
                var divide = (Divide)exp;
                return divide.left.IsEqual(left) && divide.right.IsEqual(right);
            }
            return false;
        }

        public override bool ContainsVariable(string variable)
        {
            return left.ContainsVariable(variable) || right.ContainsVariable(variable);
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            return left.ContainedVariables(right.ContainedVariables(vars));
        }

        public override string ToString()
        {
            return "(" + left.ToString() + " / " + right.ToString() + ")";
        }

        public override string ToTec()
        {
            return "\\frac{" + left.ToTec() + "}{" + right.ToTec() + "}";
        }
    }
}
