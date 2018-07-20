using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Exceptions
{
    [Serializable()]
    public class SettingsException : Exception
    {
        public string Category { get; }

        public string Key { get; }

        public int Index { get; }

        protected SettingsException(string category, string key, int index = 0) : base()
        {
            Category = category;
            Key = key;
            Index = index;
        }

        public SettingsException(string category, string key, int index, string message) : base(message)
        {
            Category = category;
            Key = key;
            Index = index;
        }

        public SettingsException(string category, string key, string message) : this(category, key, 0, message) { }

        protected SettingsException(SerializationInfo si, StreamingContext sc) : base(si, sc) { }
    }
}
