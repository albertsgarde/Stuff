using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Exceptions
{
    [Serializable()]
    public class CategoryNotFoundException : SettingsException
    {
        public CategoryNotFoundException(string category, string key) : base(category, key) { }

        protected CategoryNotFoundException(SerializationInfo si, StreamingContext sc) : base(si, sc) { }
    }
}
