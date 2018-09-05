using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic.Expressions.Operators
{
    public class Implies : Expression
    {
        public Expression Left { get; }

        public Expression Right { get; }

        public Implies(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public override bool Evaluate(Dictionary<string, bool> values = null)
        {
            return !Left.Evaluate(values) || Right.Evaluate(values);
        }

        public override bool IsEqual(Expression exp)
        {
            if (exp is Implies implies)
                return implies.Left.IsEqual(Left) && implies.Right.IsEqual(Right);
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
                    return true;
            }
            else if (RightReduced is ValueExpression RightValue)
            {
                if (RightValue.Evaluate())
                    return true;
                else
                    return !LeftReduced;
            }
            else
                return new Implies(LeftReduced, RightReduced);
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
            return $"({Left.ToString()}>{Right.ToString()})";
        }
    }
}
