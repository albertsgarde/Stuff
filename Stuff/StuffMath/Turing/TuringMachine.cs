using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Turing
{
    public class TuringMachine
    {
        private readonly State[] states;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="states">Element 0 in the given list, is state 1. Element n is state n+1.</param>
        public TuringMachine(IReadOnlyList<State> states)
        {
            this.states = states.ToArray();
        }

        public State this[int i]
        {
            get
            {
                if (i <= 0)
                    throw new ArgumentException("State numbers start from 1");
                return states[i - 1];
            }
        }

        public string AsString()
        {
            string result = "";
            for (int i = 1; i < states.Length + 1; ++i)
            {
                if (this[i][false].Action != TMAction.Halt)
                    result += $"q{i}S0{this[i][false].AsString()},";
                if (this[i][true].Action != TMAction.Halt)
                    result += $"q{i}S1{this[i][true].AsString()},";
                result += Environment.NewLine;
            }
            return result;
        }
    }

    public enum TMAction
    {
        Left, Right, Print, Erase, Halt
    }

    public struct Instruction
    {
        public TMAction Action { get; }

        public int NextState { get; }

        public Instruction(TMAction action, int nextState)
        {
            Action = action;
            NextState = nextState;
        }

        public static implicit operator Instruction((TMAction action, int nextState) instruction)
        {
            return new Instruction(instruction.action, instruction.nextState);
        }

        public string AsString()
        {
            string result = "";
            switch (Action)
            {
                case TMAction.Erase:
                    result += "S0";
                    break;
                case TMAction.Print:
                    result += "S1";
                    break;
                case TMAction.Left:
                    result += "L";
                    break;
                case TMAction.Right:
                    result += "R";
                    break;
                case TMAction.Halt:
                    result += "HALT";
                    break;
            }
            result += "q" + NextState;
            return result;
        }
    }

    public class State
    {
        private readonly Instruction zero;

        private readonly Instruction one;

        public State(Instruction zero, Instruction one)
        {
            this.zero = zero;
            this.one = one;
        }

        public State(int index)
        {
            zero = (TMAction.Halt, index);
            one = (TMAction.Halt, index);
        }

        public Instruction this[bool symbol] => symbol ? one : zero;

        public Instruction Instruction(bool symbol) => this[symbol];
    }
}
