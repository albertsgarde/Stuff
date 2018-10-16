using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class LinearParameter2D
    {
        public Vector2D A { get; private set; }

        public Vector2D B { get; private set; }

        public LinearParameter2D(double xA, double xB, double yA, double yB)
        {
            A = new Vector2D(xA, yA);
            B = new Vector2D(xB, yB);
        }

        public LinearParameter2D(Vector2D b, Vector2D a)
        {
            A = a;
            B = b;
        }

        public LinearParameter2D(Location2D loc1, Location2D loc2)
        {
            B = new Vector2D(loc1);
            A = new Vector2D(loc1, loc2);
        }

        public bool CollidesWith(LinearParameter2D param)
        {
            double t = (param.A.X * (B.Y - param.B.Y) - param.A.Y * (B.Y - param.B.X)) / (A.X * param.A.Y - param.A.X * A.Y);
            double s = (A.X * (B.Y - param.B.Y) - A.Y * (B.X - param.B.X)) / (A.X * param.A.Y - param.A.X * A.Y);
            return s == t;
        }

        public double XAxisIntersection()
        {
            double t = -B.Y / A.Y;
            return X(t);

        }

        public double YAxisIntersection()
        {
            double t = -B.X / A.X;
            return Y(t);

        }

        public double X(double t)
        {
            return A.X * t + B.X;
        }

        public double Y(double t)
        {
            return A.Y * t + B.Y;
        }

        public Location2D Point(double t)
        {
            return new Location2D(B + t * A);
        }

        public Vector2D DirectionVector()
        {
            return A;
        }

        public Vector2D OrthogonalVector()
        {
            return A.OrthogonalVector();
        }

        public LinearFunction LinearFunction()
        {
            return new LinearFunction(B, A.OrthogonalVector());
        }

        public static implicit operator LinearFunction(LinearParameter2D lp)
        {
            return lp.LinearFunction();
        }

        public override string ToString()
        {
            return "{" + B + " + t" + A + "}";
        }
    }
}
