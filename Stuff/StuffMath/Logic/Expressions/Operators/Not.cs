using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic.Expressions.Operators
{
    public class Not : Expression
    {
        public Expression Arg { get; }

        public override double Priority => 1;

        public Not(Expression arg)
        {
            Arg = arg;
        }

        public override bool Evaluate(Dictionary<string, bool> values = null)
        {
            return !Arg.Evaluate(values);
        }

        public override Expression Reduce(Dictionary<string, bool> values = null)
        {
            Expression argReduced = Arg.Reduce(values);
            if (argReduced is ValueExpression argValue)
                return !argReduced.Evaluate();
            else if (argReduced is NAnd nand)
                return new And(nand.Left, nand.Right);
            else if (argReduced is NOr nor)
                return new Or(nor.Left, nor.Right);
            else if (argReduced is XOr xor)
                return new Iff(xor.Left, xor.Right);
            else if (argReduced is And and)
                return new NAnd(and.Left, and.Right);
            else if (argReduced is Or or)
                return new NOr(or.Left, or.Right);
            else if (argReduced is Iff iff)
                return new XOr(iff.Left, iff.Right);
            else
                return !argReduced;
        }

        public override Expression ToNormalForm()
        {
            return Arg.Negate();
        }

        public override Expression Negate()
        {
            return Arg.ToNormalForm();
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            return Arg.ContainedVariables(vars);
        }

        public override bool ContainsVariable(string variable)
        {
            return Arg.ContainsVariable(variable);
        }

        public override string ToString()
        {
            return $"!{(Arg.Priority < Priority || Arg is Not ? Arg.ToString() : $"({Arg.ToString()})")}";
        }

        public override string ToLatex()
        {
            return $"\\neg {(Arg.Priority < Priority || Arg is Not ? Arg.ToLatex() : $"({Arg.ToLatex()})")}";
        }
    }
}
