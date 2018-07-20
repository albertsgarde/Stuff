using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Exceptions
{
    public class InvalidSettingFormatException : SettingsException
    {
        public string Value { get; }

        public InvalidSettingFormatException(string category, string key, int index, string value) : base(category, key, index)
        {
            Value = value;
        }
    }
}
