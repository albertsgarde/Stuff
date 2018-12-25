using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Expressions
{
    [Serializable()]
    public class NotDifferentiableException : Exception
    {
        public NotDifferentiableException() : base()
        {
        }

        public NotDifferentiableException(string message) : base(message)
        {
        }

        protected NotDifferentiableException(SerializationInfo si, StreamingContext sc) : base(si, sc) { }
    }
}
