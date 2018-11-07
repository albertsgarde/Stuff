using Stuff.StuffMath.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class LinearFirstOrderDiffEq
    {
        public Expression P { get; }

        public Expression Q { get; }

        public LinearFirstOrderDiffEq(Expression p, Expression q)
        {
            P = p;
            Q = q;
        }


    }
}
