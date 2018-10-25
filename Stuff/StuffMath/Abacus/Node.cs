using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Abacus
{
    public class Node
    {
        public enum NodeType
        {
            Add,
            Subtract
        }

        public int Register { get; }

        public NodeType Type { get; }

        public int Dest { get; }

        public int FailDest { get; }

        public Node(int register, int dest)
        {
            Register = register;
            Dest = dest;
            FailDest = -1;
            Type = NodeType.Add;
        }

        public Node(int register, int dest, int failDest)
        {
            Register = register;
            Dest = dest;
            FailDest = failDest;
            Type = NodeType.Subtract;
        }

        public string AsString()
        {
            switch (Type)
            {
                case NodeType.Add:
                    return $"{Register}+ {Dest}";
                case NodeType.Subtract:
                    return $"{Register}- {Dest} {FailDest}";
                default:
                    throw new Exception($"Node type {Type} did not exist at the time of this methods creation");
            }
        }
    }
}
