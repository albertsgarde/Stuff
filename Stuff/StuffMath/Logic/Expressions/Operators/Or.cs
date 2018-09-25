using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic.Expressions.Operators
{
    public class Or : Expression
    {
        public override string Name => "Or";

        public Expression Left { get; }

        public Expression Right { get; }

        public override double Priority => 3;

        public Or(Expression left, Expression right)
        {
            Left = left;
            Right = right;
        }

        public override bool Evaluate(Dictionary<string, bool> values = null)
        {
            return Left.Evaluate(values) || Right.Evaluate(values);
        }

        public override Expression Reduce(Dictionary<string, bool> values = null)
        {
            Expression LeftReduced = Left.Reduce(values);
            Expression RightReduced = Right.Reduce(values);
            if (LeftReduced is ValueExpression LeftValue)
            {
                if (LeftValue.Evaluate())
                    return true;
                else
                    return RightReduced;
            }
            else if (RightReduced is ValueExpression RightValue)
            {
                if (RightValue.Evaluate())
                    return true;
                else
                    return LeftReduced;
            }
            else if (LeftReduced.IsEqual(RightReduced))
                return LeftReduced;
            else
                return new Or(LeftReduced, RightReduced);
        }

        public override Expression ToNormalForm()
        {
            return new Or(Left.ToNormalForm(), Right.ToNormalForm());
        }

        public override Expression Negate()
        {
            return new And(Left.Negate(), Right.Negate());
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            return Left.ContainedVariables(Right.ContainedVariables(vars));
        }

        public override bool ContainsVariable(string variable)
        {
            return Left.ContainsVariable(variable) || Right.ContainsVariable(variable);
        }

        protected override bool InternalTableau(IReadOnlyList<(Expression exp, bool value)> expressions, IReadOnlyDictionary<string, bool> values, bool value)
        {
            if (value)
            {
                if (!InternalTableauNextExp(expressions, values, (Left, true)))
                    return InternalTableauNextExp(expressions, values, (Right, true));
                else
                    return true;
            }
            else
            {
                return InternalTableauNextExp(expressions, values, (Left, false), (Right, false));
            }
        }

        public override string ToString()
        {
            return $"{(Left.Priority < Priority || Left is Or ? Left.ToString() : $"({Left.ToString()})")}+{(Right.Priority < Priority || Right is Or ? Right.ToString() : $"({Right.ToString()})")}";
        }

        public override string ToLatex()
        {
            return $"{(Left.Priority < Priority || Left is Or? Left.ToLatex() : $"({Left.ToLatex()})")} \\lor {(Right.Priority < Priority || Right is Or ? Right.ToLatex() : $"({Right.ToLatex()})")}";
        }
    }
}
