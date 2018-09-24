using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic.Expressions.Operators
{
    public class XOr : Expression
    {
        public Expression Left { get; }

        public Expression Right { get; }

        public override double Priority => 5;

        public XOr(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public override bool Evaluate(Dictionary<string, bool> values = null)
        {
            return Left.Evaluate(values) != Right.Evaluate(values);
        }

        public override Expression Reduce(Dictionary<string, bool> values = null)
        {
            Expression LeftReduced = Left.Reduce(values);
            Expression RightReduced = Right.Reduce(values);
            if (LeftReduced.IsEqual(RightReduced))
                return false;
            else if (LeftReduced.IsNegation(RightReduced))
                return true;
            else if (LeftReduced is ValueExpression LeftValue)
            {
                if (LeftValue.Evaluate())
                    return !RightReduced;
                else
                    return RightReduced;
            }
            else if (RightReduced is ValueExpression RightValue)
            {
                if (RightValue.Evaluate())
                    return !LeftReduced;
                else
                    return LeftReduced;
            }
            else
                return new XOr(LeftReduced, RightReduced);
        }

        public override Expression ToNormalForm()
        {
            return new Or(new And(Left.Negate(), Right.ToNormalForm()), new And(Left.ToNormalForm(), Right.Negate()));
        }

        public override Expression Negate()
        {
            return new Iff(Left.ToNormalForm(), Right.ToNormalForm());
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
            return $"{(Left.Priority < Priority ? Left.ToString() : $"({Left.ToString()})")}!={(Right.Priority < Priority ? Right.ToString() : $"({Right.ToString()})")}";
        }

        public override string ToLatex()
        {
            return $"{(Left.Priority < Priority ? Left.ToLatex() : $"({Left.ToLatex()})")} \\otimes {(Right.Priority < Priority ? Right.ToLatex() : $"({Right.ToLatex()})")}";
        }
    }
}
