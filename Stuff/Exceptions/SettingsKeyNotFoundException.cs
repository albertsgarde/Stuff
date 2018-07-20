using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Exceptions
{
    [Serializable()]
    public class SettingsKeyNotFoundException : SettingsException
    {
        public SettingsKeyNotFoundException(string category, string key) : base(category, key) { }

        protected SettingsKeyNotFoundException(SerializationInfo si, StreamingContext sc) : base(si, sc) { }
    }
}
