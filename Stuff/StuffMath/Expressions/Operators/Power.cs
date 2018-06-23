using Stuff.StuffMath.Expressions.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Operators
{
    public class Power : Expression
    {
        private readonly Expression left;

        private readonly Expression right;

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

        public Power(Expression leftHand, Expression rightHand)
        {
            left = leftHand;
            right = rightHand;
        }

        public Expression Left
        {
            get
            {
                return left;
            }
        }

        public Expression Right
        {
            get
            {
                return right;
            }
        }

        public override double Evaluate(Dictionary<string, double> values = null)
        {
            return Math.Pow(left.Evaluate(values), right.Evaluate(values));
        }

        public override Expression Differentiate(string variable)
        {
            if (right.Evaluate() != double.NaN)
                return new Multiply(new Multiply (new Power(left, right.Evaluate() - 1), new ValueExpression(right.Evaluate())), left.Differentiate(variable));
            else
                return (left.Differentiate(variable)*right/left + new Ln(left)*right.Differentiate(variable))*this;
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            Expression leftReduced = left.Reduce(values);
            Expression rightReduced = right.Reduce(values);
            if (leftReduced is ValueExpression && rightReduced is ValueExpression)
                return new ValueExpression(Math.Pow(leftReduced.Evaluate(), rightReduced.Evaluate()));
            else if (leftReduced is Power)
                return new Power(((Power)leftReduced).Left, (rightReduced * ((Power)leftReduced).Right).Reduce());
            else if (rightReduced is ValueExpression && ((ValueExpression)rightReduced).Evaluate(values) == 1)
                 return leftReduced;
            else
                return new Power(leftReduced, rightReduced);
        }

        public override bool IsEqual(Expression exp)
        {
            if (exp is Power)
            {
                var power = (Power)exp;
                return power.left.IsEqual(left) && power.right.IsEqual(right);
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
            return "(" + left.ToString() + " ^ " + right.ToString() + ")";
        }

        public override string ToTec()
        {
            if (left.Priority > Priority)
                return "(" + left.ToTec() + ")" + "^{" + right.ToTec() + "}";
            else
                return left.ToTec() + "^{" + right.ToTec() + "}";
        }
    }
}
