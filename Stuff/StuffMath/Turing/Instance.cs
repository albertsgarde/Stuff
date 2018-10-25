using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Turing
{
    public class Instance
    {
        private readonly Tape tape;

        private readonly TuringMachine tm;

        public int Position { get; private set; }

        public int State { get; private set; }

        public bool Halted { get; private set; }

        public Instance(TuringMachine tm, IReadOnlyList<bool> arguments) : this(tm, new Tape(arguments))
        {
        }

        public Instance(TuringMachine tm, Tape tape)
        {
            this.tape = tape.Copy();
            this.tm = tm;
            Position = 0;
            State = 1;
            Halted = false;
        }

        public class Tape
        {
            /// <summary>
            /// All positive indices of the tape.
            /// </summary>
            private readonly List<bool> right;

            /// <summary>
            /// All negative indices of the tape.
            /// </summary>
            private readonly List<bool> left;

            public int Start => -left.Count;

            public int End => right.Count - 1;

            public Tape(IReadOnlyList<bool> arguments)
            {
                right = new List<bool>(arguments);
                left = new List<bool>();
            }

            public Tape(params int[] arguments)
            {
                right = new List<bool>();
                left = new List<bool>();
                for (int i = 0; i < arguments.Length; ++i)
                {
                    for (int j = 0; j < arguments[i]; ++j)
                        right.Add(true);
                    right.Add(false);
                }
            }

            private Tape(Tape tape)
            {
                right = new List<bool>(tape.right);
                left = new List<bool>(tape.left);
            }

            public void Print(int pos)
            {
                this[pos] = true;
            }

            public void Erase(int pos)
            {
                this[pos] = false;
            }

            public bool this[int pos]
            {
                get
                {
                    if (pos >= 0)
                    {
                        if (pos >= End)
                            return false;
                        else
                            return right[pos];
                    }
                    else
                    {
                        if (pos < Start)
                            return false;
                        else
                            return left[-pos - 1];
                    }
                }
                set
                {
                    if (pos >= 0)
                    {
                        while (pos >= End)
                            right.Add(false);
                        right[pos] = value;
                    }
                    else
                    {
                        while (pos < Start)
                            left.Add(false);
                        left[-pos - 1] = value;
                    }
                }
            }

            public Tape Copy()
            {
                return new Tape(this);
            }

            public string AsString()
            {
                string result = "";
                for (int i = Start; i < End; ++i)
                    result += this[i] ? "1" : "0";
                return result;
            }
        }

        public bool NextStep()
        {
            if (!Halted)
            {
                var inst = tm[State][tape[Position]];
                State = inst.NextState;
                switch (inst.Action)
                {
                    case TMAction.Halt:
                        Halted = true;
                        return false;
                    case TMAction.Left:
                        --Position;
                        break;
                    case TMAction.Right:
                        ++Position;
                        break;
                    case TMAction.Print:
                        tape.Print(Position);
                        break;
                    case TMAction.Erase:
                        tape.Erase(Position);
                        break;
                }
                if (Position >= tape.End)
                    tape[Position] = false;
                else if (Position < tape.Start)
                    tape[Position] = false;
            }
            return false;
        }

        public void Run()
        {
            while (!Halted)
                NextStep();
        }

        public int Value()
        {
            int i = Position;
            while (tape[i])
                i++;
            return i - Position;
        }

        public int[] Values()
        {
            List<int> result = new List<int>();
            var curNum = 0;
            for (int i = tape.Start; i < tape.End; ++i)
            {
                if (tape[i])
                    curNum++;
                else if (curNum > 0)
                {
                    result.Add(curNum);
                    curNum = 0;
                }
            }
            if (curNum > 0)
                result.Add(curNum);
            return result.ToArray();
        }

        public string TapeAsString()
        {
            return tape.AsString();
        }

        public string AsString()
        {
            string result = "";
            for (int i = tape.Start; i < tape.End; ++i)
            {
                result += tape[i] ? "1" : "0";
                if (Position == i)
                    result += "[" + State + "]";
            }
            if (Halted)
                result += "HALT!";
            return result;
        }
    }
}
