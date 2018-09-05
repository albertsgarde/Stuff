using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic.Expressions.Operators
{
    public class NOr : Expression
    {
        public Expression Left { get; }

        public Expression Right { get; }

        public NOr(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public override bool Evaluate(Dictionary<string, bool> values = null)
        {
            return !(Left.Evaluate(values) || Right.Evaluate(values));
        }

        public override bool IsEqual(Expression exp)
        {
            if (exp is NOr nor)
                return nor.Left.IsEqual(Left) && nor.Right.IsEqual(Right) || nor.Left.IsEqual(Right) && nor.Right.IsEqual(Left);
            if (exp is Not not && not.Arg is Or or)
                return or.Left.IsEqual(Left) && or.Right.IsEqual(Right) || or.Left.IsEqual(Right) && or.Right.IsEqual(Left);
            return false;
        }

        public override Expression Reduce(Dictionary<string, bool> values = null)
        {
            Expression LeftReduced = Left.Reduce(values);
            Expression RightReduced = Right.Reduce(values);
            if (LeftReduced is ValueExpression LeftValue)
            {
                if (LeftValue.Evaluate())
                    return false;
                else
                    return !RightReduced;
            }
            else if (RightReduced is ValueExpression RightValue)
            {
                if (RightValue.Evaluate())
                    return false;
                else
                    return !LeftReduced;
            }
            else
                return new NOr(LeftReduced, RightReduced);
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            return Left.ContainedVariables(Right.ContainedVariables(vars));
        }

        public override bool ContainsVariable(string variable)
        {
            return Left.ContainsVariable(variable) || Right.ContainsVariable(variable);
        }

        public override string ToString()
        {
            return $"({Left.ToString()}!+{Right.ToString()})";
        }
    }
}
