using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.StuffMath;

namespace Stuff
{
    public class MassPoint
    {
        public double Mass { get; }

        public Location2D Loc { get; }

        public MassPoint(double mass, Location2D loc)
        {
            Mass = mass;
            Loc = loc;
        }

        public MassPoint(double mass, double x, double y)
        {
            Mass = mass;
            Loc = new Location2D(x, y);
        }

        public static MassPoint CentreOfMass(params MassPoint[] masses)
        {
            var resultMass = 0d;
            var resultLoc = new Vector2D();
            foreach (var mass in masses)
            {
                resultMass += mass.Mass;
                resultLoc += mass.Loc.LocationVector() * mass.Mass;
            }
            return new MassPoint(resultMass, resultLoc / resultMass);
        }

        public static MassPoint operator+(MassPoint mp1, MassPoint mp2)
        {
            return new MassPoint(mp1.Mass + mp2.Mass, (mp1.Loc.LocationVector() * mp1.Mass + mp2.Loc.LocationVector() * mp2.Mass) / (mp1.Mass + mp2.Mass));
        }

        public static MassPoint operator-(MassPoint mp1, MassPoint mp2)
        {
            return new MassPoint(mp1.Mass - mp2.Mass, (mp1.Loc.LocationVector() * mp1.Mass - mp2.Loc.LocationVector() * mp2.Mass) / (mp1.Mass - mp2.Mass));
        }

        public static MassPoint operator-(MassPoint mp)
        {
            return new MassPoint(-mp.Mass, mp.Loc);
        }

        public override string ToString()
        {
            return $"{{{Mass}, {Loc}}}";
        }
    }
}
