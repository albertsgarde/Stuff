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

        public override double Priority => 2;

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

        public override bool Evaluate(Dictionary<string, bool> values = null)
        {
            return Left.Evaluate(values) && Right.Evaluate(values);
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
            else if (RightReduced.IsEqual(LeftReduced))
                return RightReduced;
            else
                return new And(LeftReduced, RightReduced);
        }

        public override Expression ToNormalForm()
        {
            return new And(Left.ToNormalForm(), Right.ToNormalForm());
        }

        public override Expression Negate()
        {
            return new Or(Left.Negate(), Right.Negate());
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
            return $"{(Left.Priority < Priority || Left is And ? Left.ToString() : $"({Left.ToString()})" )}*{(Right.Priority < Priority || Right is And ? Right.ToString() : $"({Right.ToString()})")}";
        }

        public override string ToLatex()
        {
            return $"{(Left.Priority < Priority || Left is And ? Left.ToLatex() : $"({Left.ToLatex()})")} \\land {(Right.Priority < Priority || Right is And ? Right.ToLatex() : $"({Right.ToLatex()})")}";
        }
    }
}
