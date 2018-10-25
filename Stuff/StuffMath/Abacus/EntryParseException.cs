using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Abacus
{
    public class EntryParseException : Exception
    {
        public string FunctionName { get; }

        public int InstructionNumber { get; }

        public EntryParseException(string functionName, int instructionNumber, Exception cause) : base($"{functionName} {instructionNumber}: {cause.Message}", cause)
        {
            FunctionName = functionName;
            InstructionNumber = instructionNumber;
        }
    }
}
