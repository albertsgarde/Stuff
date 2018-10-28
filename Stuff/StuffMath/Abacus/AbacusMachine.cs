using Stuff.StuffMath.Turing;
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

        private List<State> AddToTuring(int reg, int startNode)
        {
            var result = new List<State>();
            var node = startNode;

            for (int i = 0; i < reg - 1; ++i)
            {
                result.Add(new State((TMAction.Right, node + 1), (TMAction.Right, node)));
                node++;
            }

            for (int i = 0; i < Registers - reg; ++i)
            {
                result.Add(new State((TMAction.Print, node + 1), (TMAction.Right, node)));
                node++;
                result.Add(new State((TMAction.Halt, -1), (TMAction.Right, node + 1)));
                node++;
                result.Add(new State((TMAction.Right, node + 1), (TMAction.Erase, node)));
                node++;
            }

            result.Add(new State((TMAction.Print, node + 1), (TMAction.Right, node)));
            node++;

            for (int i = 0; i < Registers - 1; ++i)
            {
                result.Add(new State((TMAction.Left, node + 1), (TMAction.Left, node)));
                node++;
            }

            result.Add(new State((TMAction.Right, node + 1/* TODO: Replace with addNodes target node */), (TMAction.Left, node)));
            node++;
            return result;
        }

        private List<State> SubtractToTuring(int reg, int startNode)
        {
            var result = new List<State>();
            var node = startNode;

            for (int i = 0; i < reg - 1; ++i)
            {
                result.Add(new State((TMAction.Right, node + 1), (TMAction.Right, node)));
                node++;
            }

            result.Add(new State((TMAction.Halt, -1), (TMAction.Right, node + 1)));
            node++;
            var ifNodeNum = node;
            //If node
            node++;
            var zNodes = new List<State>();
            var zeroNodeNum = node;
            
            for (int i = 0; i < reg - 1; ++i)
            {
                zNodes.Add(new State((TMAction.Left, node + 1), (TMAction.Left, node)));
                node++;
            }

            zNodes.Add(new State((TMAction.Right, node + 1/* TODO: Replace with subtractNodes e-target node */), (TMAction.Left, node)));
            node++;

            var oNodes = new List<State>();
            var oneNodeNum = node;

            for (int i = 0; i < Registers - reg - 1; ++i)
            {
                oNodes.Add(new State((TMAction.Right, node + 1), (TMAction.Right, node)));
                node++;
            }

            oNodes.Add(new State((TMAction.Left, node + 1), (TMAction.Right, node)));
            node++;

            for (int i = 0; i < Registers - reg; ++i)
            {
                oNodes.Add(new State((TMAction.Left, node + 1), (TMAction.Erase, node)));
                node++;
                oNodes.Add(new State((TMAction.Print, node + 1), (TMAction.Left, node)));
                node++;
                oNodes.Add(new State((TMAction.Halt, -1), (TMAction.Left, node + 1)));
                node++;
            }

            oNodes.Add(new State((TMAction.Left, node + 1), (TMAction.Erase, node)));
            node++;

            for (int i = 0; i < reg - 1; ++i)
            {
                oNodes.Add(new State((TMAction.Left, node + 1), (TMAction.Left, node)));
                node++;
            }

            oNodes.Add(new State((TMAction.Right, node + 1/* TODO: Replace with subtractNodes target node */), (TMAction.Left, node)));

            result.Add(new State((TMAction.Left, zeroNodeNum), (TMAction.Right, oneNodeNum)));
            result.Concat(zNodes);
            result.Concat(oNodes);
            return result;
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
