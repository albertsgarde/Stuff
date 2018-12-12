using Stuff.StuffMath.Expressions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions
{
    public abstract class Expression
    {
        /// <summary>
        /// The priority of evaluation when the math is written out.
        /// Plus would have lower priority than multiply would have lower priority than power.
        /// Lower number is higher priority.
        /// </summary>
        public abstract double Priority { get; protected set; } 

        public static Expression operator+(Expression left, Expression right)
        {
            return new Add(left, right);
        }

        public static Expression operator -(Expression left, Expression right)
        {
            return new Subtract(left, right);
        }

        public static Expression operator *(Expression left, Expression right)
        {
            return new Multiply(left, right);
        }

        public static Expression operator /(Expression left, Expression right)
        {
            return new Divide(left, right);
        }
        public static Expression operator +(Expression left, double right)
        {
            return new Add(left, new ValueExpression(right));
        }

        public static Expression operator -(Expression left, double right)
        {
            return new Subtract(left, new ValueExpression(right));
        }

        public static Expression operator *(Expression left, double right)
        {
            return new Multiply(left, new ValueExpression(right));
        }

        public static Expression operator /(Expression left, double right)
        {
            return new Divide(left, new ValueExpression(right));
        }

        public static Expression operator +(double left, Expression right)
        {
            return new Add(new ValueExpression(left), right);
        }

        public static Expression operator -(double left, Expression right)
        {
            return new Subtract(new ValueExpression(left), right);
        }

        public static Expression operator *(double left, Expression right)
        {
            return new Multiply(new ValueExpression(left), right);
        }

        public static Expression operator /(double left, Expression right)
        {
            return new Divide(new ValueExpression(left), right);
        }

        public static implicit operator Expression(double d)
        {
            return new ValueExpression(d);
        }

        public abstract double Evaluate(Dictionary<string, double> values = null);

        public double Evaluate(double x)
        {
            return Evaluate(new Dictionary<string, double>() { { "x", x } });
        }

        public abstract Expression Differentiate(string variable);

        public Expression Differentiate(string variable, int degree)
        {
            if (degree <= 0)
                throw new ArgumentException("Degree must be at least 1.");
            if (degree == 1)
                return Differentiate(variable);
            else
                return Differentiate(variable, --degree).Differentiate(variable);
        }

        /// <summary>
        /// Reduces the expression as much as possible, checking for various special cases. 
        /// Does not guarantee that the expression is in its most reduced form.
        /// </summary>
        /// <returns></returns>
        public abstract Expression Reduce(Dictionary<string, double> values = null);

        /// <summary>
        /// Checks whether this expression is mathematically equal to the specified expression. 
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

        public abstract override string ToString();

        public abstract string ToTec();
    }
}
