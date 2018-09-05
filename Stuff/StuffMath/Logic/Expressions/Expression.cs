using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.StuffMath.Logic.Expressions.Operators;

namespace Stuff.StuffMath.Logic.Expressions
{
    public abstract class Expression
    {
        public static implicit operator Expression(bool b)
        {
            return new ValueExpression(b);
        }

        public static Expression operator!(Expression exp)
        {
            return new Not(exp);
        }

        public abstract bool Evaluate(Dictionary<string, bool> values = null);
        
        public bool Evaluate(bool x)
        {
            return Evaluate(new Dictionary<string, bool>() { { "x", x } });
        }

        public bool Evaluate(params (string name, bool value)[] values)
        {
            return Evaluate(values.ToDictionary(v => v.name, v => v.value));
        }

        /// <summary>
        /// Reduces the expression as much as possible, checking for various special cases. 
        /// Does not guarantee that the expression is in its most reduced form.
        /// </summary>
        public abstract Expression Reduce(Dictionary<string, bool> values = null);
        
        /// <summary>
        /// Checks whether this expression is logically equal to the specified expression. 
        /// Will not always return true if they are, but will never return true if they aren't.
        /// Will mostly return far better results if both expressions are reduced first.
        /// </summary>
        /// <param name="exp">The expression to compare this expression to.</param>
        /// <returns>Returns false if the expressions are unequal. Might return true if they are equal.</returns>
        public abstract bool IsEqual(Expression exp);

        public abstract bool ContainsVariable(string variable);

        public HashSet<string> ContainedVariables()
        {
            var vars = new HashSet<string>();
            return ContainedVariables(vars);
        }

        public abstract HashSet<string> ContainedVariables(HashSet<string> vars);

        public bool IsTautology()
        {
            var vars = ContainedVariables().ToArray();
            var values = new bool[vars.Length];
            for (int i = 0; i < values.Length; i++)
                values[i] = true;

            int attempts = (int)Math.Pow(2, values.Length);
            for (int i = 0; i < attempts; ++i)
            {
                IncrementBoolValues(values);
                var attemptVars = new Dictionary<string, bool>();
                for (int j = 0; j < vars.Length; ++j)
                    attemptVars.Add(vars[j], values[j]);
                if (!Evaluate(attemptVars))
                    return false;
            }
            return true;
        }

        public bool IsContradiction()
        {
            var vars = ContainedVariables().ToArray();
            var values = new bool[vars.Length];
            for (int i = 0; i < values.Length; i++)
                values[i] = true;

            int attempts = (int)Math.Pow(2, values.Length);
            for (int i = 0; i < attempts; ++i)
            {
                IncrementBoolValues(values);
                var attemptVars = new Dictionary<string, bool>();
                for (int j = 0; j < vars.Length; ++j)
                    attemptVars.Add(vars[j], values[j]);
                if (Evaluate(attemptVars))
                    return false;
            }
            return true;
        }

        public ValueTable ValidInputs()
        {
            var vars = ContainedVariables().ToArray();
            var values = new List<bool[]>();
            var curValues = new bool[vars.Length];
            for (int i = 0; i < curValues.Length; i++)
                curValues[i] = true;

            int attempts = (int)Math.Pow(2, curValues.Length);
            for (int i = 0; i < attempts; ++i)
            {
                IncrementBoolValues(curValues);
                var attemptVars = new Dictionary<string, bool>();
                for (int j = 0; j < vars.Length; ++j)
                    attemptVars.Add(vars[j], curValues[j]);
                if (Evaluate(attemptVars))
                    values.Add(curValues.Copy());
            }
            return new ValueTable(vars, values.ToArray());
        }

        private void IncrementBoolValues(bool[] values)
        {
            var input = values.Copy();
            int i = values.Length;
            while (values[--i])
            {
                if (i == 0)
                {
                    i = values.Length;
                    for (int j = 0; j < values.Length; ++j)
                        values[j] = false;
                    return;
                }
            }
            values[i] = true;
            while (++i < values.Length)
                values[i] = false;
        }

        public abstract override string ToString();
    }
}
