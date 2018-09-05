using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic.Expressions.Operators
{
    public class And : Expression
    {
        public Expression Left { get; }

        public Expression Right { get; }

        public And(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public And(params Expression[] expressions)
        {
            Left = expressions[0];
            for (int i = 1; i < expressions.Length - 1; ++i)
                Left = new And(Left, expressions[i]);
            Right = expressions[expressions.Length - 1];
        }

        public And(IEnumerable<Expression> expressions)
        {
            var and = (And)expressions.Aggregate((a, b) => new And(a, b));
            Left = and.Left;
            Right = and.Right;
        }

        public override bool Evaluate(Dictionary<string, bool> values = null)
        {
            return Left.Evaluate(values) && Right.Evaluate(values);
        }

        public override bool IsEqual(Expression exp)
        {
            if (exp is And and)
                return and.Left.IsEqual(Left) && and.Right.IsEqual(Right) || and.Left.IsEqual(Right) && and.Right.IsEqual(Left);
            if (exp is Not not && not.Arg is NAnd nand)
                return nand.Left.IsEqual(Left) && nand.Right.IsEqual(Right) || nand.Left.IsEqual(Right) && nand.Right.IsEqual(Left);
            return false;
        }

        public override Expression Reduce(Dictionary<string, bool> values = null)
        {
            Expression LeftReduced = Left.Reduce(values);
            Expression RightReduced = Right.Reduce(values);
            if (LeftReduced is ValueExpression LeftValue)
            {
                if (LeftValue.Evaluate())
                    return RightReduced;
                else
                    return false;
            }
            else if (RightReduced is ValueExpression RightValue)
            {
                if (RightValue.Evaluate())
                    return LeftReduced;
                else
                    return false;
            }
            else
                return new And(LeftReduced, RightReduced);
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
            return $"({Left.ToString()}*{Right.ToString()})";
        }
    }
}
