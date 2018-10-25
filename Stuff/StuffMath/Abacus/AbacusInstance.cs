using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Abacus
{
    public class AbacusInstance
    {
        private readonly AbacusMachine am;

        private readonly AbacusMachineState ams;

        public int CurNode { get; private set; }

        public bool Halted { get; private set; }

        public AbacusInstance(AbacusMachine am)
        {
            this.am = am;
            ams = new AbacusMachineState();
            CurNode = 1;
            Halted = false;
        }

        public AbacusInstance(AbacusMachine am, AbacusMachineState ams)
        {
            this.am = am;
            this.ams = ams.Copy();
            CurNode = 1;
            Halted = false;
        }

        public bool NextStep()
        {
            if (!Halted)
            {
                var inst = am[CurNode];
                CurNode = ams.Apply(inst);
                if (CurNode == 0)
                {
                    Halted = true;
                    return false;
                }
                else if (CurNode < 0)
                {
                    throw new Exception("Error code: " + CurNode);
                }
                else
                    return true;
            }
            return false;
        }

        public int Run()
        {
            int steps = 0;
            while (!Halted)
            {
                NextStep();
                ++steps;
            }
            return steps;
        }

        public string AsString()
        {
            return $"{CurNode}{(Halted ? "HALTED!!!" : "")}: {ams.AsString()}";
        }
    }
}
