using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Abacus
{
    public class AbacusMachine
    {
        public int Registers { get; }

        public int Nodes => nodes.Count;

        private readonly IReadOnlyList<Node> nodes;

        public AbacusMachine(params Node[] nodes) : this(nodes.AsEnumerable())
        {

        }

        public AbacusMachine(params IEnumerable<Node>[] nodes)
        {
            var result = new List<Node>();
            foreach (var nodeList in nodes)
                result.AddRange(new List<Node>(nodeList));
            this.nodes = result;
            Registers = this.nodes.Max(n => n.Register);
        }

        public Node this[int i]
        {
            get
            {
                if (i == 0)
                    throw new ArgumentException("Nodes start from 1");
                else
                    return nodes[i - 1];
            }
        }

        public List<Node> Translate(int nodeNumStart, int endNode, params int[] registers)
        {
            return Translate(nodeNumStart, endNode, registers.ToList());
        }

        public List<Node> Translate(int nodeNumStart, int endNode, List<int> registers)
        {
            if (registers.Count < Registers)
                throw new ArgumentException("Not enough registers.");
            var result = new List<Node>();
            foreach (var node in nodes)
            {
                if (node.Type == Node.NodeType.Add)
                    result.Add(new Node(registers[node.Register - 1], NodeNum(node.Dest, nodeNumStart, endNode)));
                else if (node.Type == Node.NodeType.Subtract)
                    result.Add(new Node(registers[node.Register - 1], NodeNum(node.Dest, nodeNumStart, endNode), NodeNum(node.FailDest, nodeNumStart, endNode)));
                else
                    throw new Exception($"Node type {node.Type} did not exist at the time of this methods creation");
            }
            return result;
        }

        private int NodeNum(int node, int nodeNumStart, int endNode)
        {
            if (node == 0)
                return endNode;
            else
                return node + nodeNumStart - 1;
        }

        public string AsString()
        {
            string result = "";
            for (int i = 1; i <= Nodes; ++i)
                result += $"{i}: {this[i].AsString()}{Environment.NewLine}";
            return result;
        }
    }
}
