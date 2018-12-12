using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Operators
{
    public class Multiply : Expression
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

        public Multiply(Expression leftHand, Expression rightHand)
        {
            left = leftHand;
            right = rightHand;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            return left.Evaluate(values) * right.Evaluate(values);
        }

        public override Expression Differentiate(string variable)
        {
            return (left.Differentiate(variable) * right) + (left * right.Differentiate(variable));
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            Expression leftReduced = left.Reduce(values);
            Expression rightReduced = right.Reduce(values);
            if (leftReduced is ValueExpression && rightReduced is ValueExpression)
                return new ValueExpression(leftReduced.Evaluate() * rightReduced.Evaluate());
            else if (leftReduced is ValueExpression && leftReduced.Evaluate() == 1)
                return rightReduced;
            else if (rightReduced is ValueExpression && rightReduced.Evaluate() == 1)
                return leftReduced;
            else if (rightReduced is ValueExpression && rightReduced.Evaluate() == 0 || leftReduced is ValueExpression && leftReduced.Evaluate() == 0)
                return new ValueExpression(0);
            else if (leftReduced.IsEqual(rightReduced))
                return new Power(leftReduced, 2);
            else
                return leftReduced * rightReduced;
        }

        public override bool IsEqual(Expression exp)
        {
            if (exp is Multiply)
            {
                var multiply = (Multiply)exp;
                return multiply.left.IsEqual(left) && multiply.right.IsEqual(right) || multiply.left.IsEqual(right) && multiply.right.IsEqual(left);
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
            return "(" + left.ToString() + " * " + right.ToString() + ")";
        }

        public override string ToTec()
        {
            return (left.Priority > Priority ? "(" + left.ToTec() + ")" : left.ToTec()) + "\\cdot "  + (right.Priority > Priority ? "(" + right.ToTec() + ")" : right.ToTec());
        }
    }
}
