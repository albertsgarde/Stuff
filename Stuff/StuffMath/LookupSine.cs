using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class LookupCosine
    {
        public float[] Table { get; }

        public LookupCosine(int length)
        {
            Table = new float[length];
            var increment = Math.PI * 2 / length;
            for (int i = 0; i < length; ++i)
                Table[i] = (float)Math.Cos(increment * i);
        }
    }
}
