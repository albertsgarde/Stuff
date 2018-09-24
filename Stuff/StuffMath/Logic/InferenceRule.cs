using Stuff.StuffMath.Logic.Expressions;
using Stuff.StuffMath.Logic.Expressions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic
{
    public class InferenceRule
    {
        public IReadOnlyList<Expression> Premises { get; }
    
        public Expression Conclusion { get; }

        public InferenceRule(Expression conclusion, params Expression[] premises)
        {
            Conclusion = conclusion;
            Premises = premises.ToList();
        }

        public Expression PremiseExpression()
        {
            return Expression.And(Premises);
        }

        public Implies TotalExpression()
        {
            return new Implies(PremiseExpression(), Conclusion);
        }

        public bool IsValid()
        {
            return TotalExpression().IsTautology();
        }

        public string ToLatex()
        {
            return $"\\frac{{{Premises.Aggregate("", (a,b)=>$"{a},{b.ToLatex()}").Substring(1)}}}{{{Conclusion.ToLatex()}}}";
        }
    }
}
