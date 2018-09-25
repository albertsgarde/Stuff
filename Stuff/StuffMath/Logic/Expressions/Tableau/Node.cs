using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Logic.Expressions.Tableau
{
    public class Node
    {
        public IReadOnlyList<Expression> Expressions { get; }

        public IReadOnlyList<Node> Children { get; }

        private Node(IReadOnlyList<Expression> exps, IReadOnlyList<Node> children)
        {
            Expressions = exps;
            Children = children;
        }

        
    }
}
