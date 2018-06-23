using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions.Functions
{
    public class BinomCoef : Expression
    {
        private Expression n;

        private Expression r;

        public override double Priority
        {
            get
            {
                return 4;
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public BinomCoef(Expression n, Expression r)
        {
            this.n = n;
            this.r = r;
        }

        public override double Evaluate(Dictionary<string, double> values)
        {
            double nValue = n.Evaluate((values));
            double rValue = r.Evaluate((values));
            if (nValue % 1 != 0)
                throw new Exception("binomCoef only takes integer arguments. n = " + nValue);
            if (rValue % 1 != 0)
                throw new Exception("binomCoef only takes integer arguments. r = " + rValue);
            return Basic.BinomialCoefficient((long)nValue, (long)rValue);
        }

        public override Expression Differentiate(string variable)
        {
            throw new Exception("Cannot differentiate binomCoef");
        }

        public override Expression Reduce(Dictionary<string, double> values = null)
        {
            Expression nReduced = n.Reduce(values);
            Expression rReduced = r.Reduce(values);
            if (nReduced is ValueExpression)
            {
                if (rReduced is ValueExpression)
                    return new ValueExpression(Evaluate(values));
                else
                {
                    var nValue = nReduced.Evaluate();
                    if (nValue % 1 != 0)
                        throw new Exception("binomCoef only takes integer arguments. n = " + nValue);
                    return Basic.Factorial((long)nValue) / (new Factorial(rReduced) * new Factorial(nValue - rReduced));
                }
            }
            else if (rReduced is ValueExpression)
            {
                var rValue = rReduced.Evaluate();
                if (rValue % 1 != 0)
                    throw new Exception("binomCoef only takes integer arguments. r = " + rValue);
                return nReduced / (Basic.Factorial((long)rValue) * new Factorial(nReduced - rValue));
            }
            else
                return new BinomCoef(nReduced, rReduced);
        }

        public override bool IsEqual(Expression exp)
        {
            if (exp is BinomCoef)
            {
                var binomCoef = (BinomCoef)exp;
                return binomCoef.n.IsEqual(n) && binomCoef.r.IsEqual(r);
            }
            return false;
        }

        public override bool ContainsVariable(string variable)
        {
            return n.ContainsVariable(variable) || r.ContainsVariable(variable);
        }

        public override HashSet<string> ContainedVariables(HashSet<string> vars)
        {
            return n.ContainedVariables(r.ContainedVariables(vars));
        }

        public override string ToString()
        {
            return "binomCoef(" + n.ToString() + "," + r.ToString() + ")";
        }

        public override string ToTec()
        {
            throw new NotImplementedException();
        }
    }
}
