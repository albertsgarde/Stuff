using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Abacus
{
    public class AbacusMachineState
    {
        private readonly List<int> registers;

        public AbacusMachineState() : this(new List<int>())
        {

        }

        public AbacusMachineState(IReadOnlyList<int> registers)
        {
            this.registers = new List<int>(registers);
        }

        public int this[int register]
        {
            get
            {
                if (register > registers.Count)
                    return 0;
                else
                    return registers[register - 1];
            }
        }

        public void Add(int register)
        {
            while (registers.Count < register)
                registers.Add(0);
            registers[register - 1]++;
        }

        public bool Subtract(int register)
        {
            if (registers.Count < register || registers[register - 1] == 0)
                return false;
            --registers[register - 1];
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns>The next node. If this is zero, the machine halts. Negative values are error codes.</returns>
        public int Apply(Node node)
        {
            if (node.Type == Node.NodeType.Add)
            {
                Add(node.Register);
                return node.Dest;
            }
            else if (node.Type == Node.NodeType.Subtract)
            {
                if (Subtract(node.Register))
                    return node.Dest;
                else
                    return node.FailDest;
            }
            else
                throw new Exception($"Node type {node.Type} did not exist at the time of this methods creation");
        }

        public AbacusMachineState Copy()
        {
            return new AbacusMachineState(registers);
        }

        public string AsString()
        {
            var result = "";
            for (int i = 0; i < registers.Count; ++i)
                result += registers[i] + " ";
            return result;
        }
    }
}
