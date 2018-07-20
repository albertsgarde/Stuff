using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Exceptions
{
    [Serializable()]
    public class IndexNotFoundException : SettingsException
    {
        public IndexNotFoundException(string category, string key, int index) : base(category, key, index) { }

        protected IndexNotFoundException(SerializationInfo si, StreamingContext sc) : base(si, sc) { }
    }
}
