using System;
using System.Linq;
using System.Xml.Linq;

namespace Stuff.StuffMath.Expressions
{
    public class Operator
    {
        public Type Class { get; private set; }

        public string Symbol { get; private set; }

        public double Priority { get; private set; }

        public Operator(string className, string symbol, double priority)
        {
            Class = Type.GetType(className);

            Symbol = symbol;

            Priority = priority;

            var operators = ExpressionCompiler.Operators;
            if (!ExpressionCompiler.Operators.Contains(Symbol))
                throw new ArgumentException("An operator can only have a symbol contained in ExpressionCompiler's list of operators.");
        }

        public static Operator Load(XElement element)
        {
            string className = element.ElementValue("className");
            string symbol = element.ElementValue("symbol");
            double priority = double.Parse(element.ElementValue("priority"));
            return new Operator(className, symbol, priority);
        }

        public Expression Create(params Expression[] args)
        {
            return (Expression)Activator.CreateInstance(Class, args);
        }
    }
}
