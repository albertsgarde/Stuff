using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.StuffMath;

namespace Stuff
{
    public class ChargePoint
    {
        public const double COULOMB = 1 / (4 * Math.PI * 8.85 * 0.000000000001);

        public Location2D Location { get; private set; }

        public double Charge { get; private set; }

        public ChargePoint(Location2D loc, double charge)
        {
            Location = loc;
            Charge = charge;
        }

        public double ForceSize(ChargePoint cp)
        {
            return COULOMB * Charge * cp.Charge / Location.DistanceToSquared(cp.Location);
        }

        public Vector2D ForceAtPoint(Location2D loc, double charge)
        {
            return new Vector2D(Location, loc).Scale(COULOMB * Charge * charge / Location.DistanceToSquared(loc));
        }
    }
}
