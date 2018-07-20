using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Exceptions
{
    public class InvalidSettingFormat : SettingsException
    {
        public InvalidSettingFormat(string category, string key, int index) : base(category, key, index) { }
    }
}
