using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic.Expressions.Tableau
{
    public class Node
    {
        public IReadOnlyList<(Expression exp, bool value)> NodeExpressions { get; }

        public IReadOnlyList<(Expression exp, bool value)> Expressions { get; }

        public IReadOnlyList<Node> Children { get; }

        public Node(IReadOnlyList<(Expression exp, bool value)> exps, IReadOnlyList<Node> children)
        {
            NodeExpressions = exps;
            Children = children;
        }

        public Node(IReadOnlyList<(Expression exp, bool value)> nodeExps, IReadOnlyList<(Expression exp, bool value)> exps)
        {
            NodeExpressions = nodeExps;
        }

        public Node(Expression exp, bool value)
        {
            NodeExpressions = new List<(Expression exp, bool value)>()
            {
                (exp, value)
            };
        }
    }
}
