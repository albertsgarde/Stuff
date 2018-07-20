using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Exceptions
{
    public class InvalidSettingFormat : SettingsException
    {
        public string Value { get; }

        public InvalidSettingFormat(string category, string key, int index, string value) : base(category, key, index)
        {
            Value = value;
        }
    }
}
