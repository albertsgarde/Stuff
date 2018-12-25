using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.StuffMath.Expressions;
using Stuff.StuffMath.Expressions.Functions;
using Stuff.StuffMath.Expressions.Operators;

namespace Stuff.StuffMath.Structures
{
    /// <summary>
    /// A wrapper class that allows Expressions to be used as IHilbertFields.
    /// </summary>
    public class ExpressionField : IHilbertField<ExpressionField>
    {
        public Expression Exp { get; }

        public ExpressionField ONE => new ExpressionField(1);

        public ExpressionField ZERO => new ExpressionField(0);

        public ExpressionField() : this(0) { }

        public ExpressionField(Expression exp)
        {
            Exp = exp;
        }

        public static implicit operator ExpressionField(Expression exp) => new ExpressionField(exp);

        public static implicit operator Expression(ExpressionField expf) => expf.Exp;

        public ExpressionField AbsSqrt() => new Sqrt(Exp);

        public ExpressionField Add(ExpressionField t) => Exp + t;

        public ExpressionField AdditiveInverse() => -Exp;

        public ExpressionField Conjugate() => new Conjugate(Exp);

        public bool EqualTo(ExpressionField t) => Exp.IsEqual(t.Exp);

        public ExpressionField MultiplicativeInverse() => 1 / Exp;

        public ExpressionField Multiply(ExpressionField t) => Exp * t;

        public override string ToString() => Exp.ToString();

        public string ToLatex() => Exp.ToTec();
    }
}
