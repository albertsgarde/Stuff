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

        public override double Priority => 4;

        public Implies(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public override bool Evaluate(Dictionary<string, bool> values = null)
        {
            return !Left.Evaluate(values) || Right.Evaluate(values);
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
            else if (LeftReduced.IsEqual(RightReduced))
                return true;
            else
                return new Implies(LeftReduced, RightReduced);
        }

        public override Expression ToNormalForm()
        {
            return new Implies(Left.ToNormalForm(), Right.ToNormalForm());
        }

        public override Expression Negate()
        {
            return new And(Left.ToNormalForm(), Right.Negate());
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
            return $"{(Left.Priority < Priority ? Left.ToString() : $"({Left.ToString()})")}>{(Right.Priority < Priority ? Right.ToString() : $"({Right.ToString()})")}";
        }

        public override string ToLatex()
        {
            return $"{(Left.Priority < Priority ? Left.ToLatex() : $"({Left.ToLatex()})")} \\implies {(Right.Priority < Priority ? Right.ToLatex() : $"({Right.ToLatex()})")}";
        }
    }
}
