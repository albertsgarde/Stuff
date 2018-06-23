using System;
using System.Xml.Linq;
using System.Linq;

namespace Stuff.StuffMath.Expressions
{
    public class Function
    {
        public int Args { get; private set; }

        public Type Class { get; private set; }

        public string[] Aliases { get; private set; }

        public Function(int args, string className, params string[] aliases)
        {
            Args = args;

            Class = Type.GetType(className);

            Aliases = aliases;
        }

        public static Function Load(XElement element)
        {
            int args = int.Parse(element.ElementValue("args"));
            string className = element.ElementValue("className");
            string[] aliases = element.Element("aliases").ElementValues("alias").ToArray();
            return new Function(args, className, aliases);
        }

        public Expression Create(params Expression[] args)
        {
            return (Expression)Activator.CreateInstance(Class, args);
        }
    }
}
