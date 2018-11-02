using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Circle
    {
        public double A { get; private set; }

        public double B { get; private set; }

        public double R { get; private set; }

        public Circle(double a, double b, double r)
        {
            A = a;
            B = b;
            R = r;
        }

        /// <summary>
        /// Creates a circle function with loc as centre and lf as tangent.
        /// 
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="lf"></param>
        public Circle(Location2D loc, LineEquation lf)
        {
            A = loc.X;
            B = loc.Y;
            R = loc.DistanceTo(lf);
        }

        /// <summary>
        /// x^2 + ax + y^2 + by + c = 0
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns>The collapsed circle.</returns>
        public static Circle Collapse(double a, double b, double c)
        {
            return new Circle(-a / 2, -b / 2, Math.Sqrt(Math.Pow(-a / 2, 2) + Math.Pow(-b / 2, 2) - c));
        }

        public double[] Y(double x)
        {
            double[] result = { 0 - Math.Sqrt((R * R - x * x + 2 * A * x - A * A) - B), Math.Sqrt((R * R - x * x + 2 * A * x - A * A) - B) };
            return result;
        }

        public double[] X(double y)
        {
            double[] result = { 0 - Math.Sqrt((R * R - y * y + 2 * B * y - B * B) - A), Math.Sqrt((R * R - y * y + 2 * B * y - B * B) - A) };
            return result;
        }

        public Location2D Centre
        {
            get
            {
                return new Location2D(A, B);
            }
        }

        public static Circle operator *(Circle c, double d)
        {
            return new Circle(c.A, c.B, c.R * d);
        }

        public static Circle operator /(Circle c, double d)
        {
            return new Circle(c.A, c.B, c.R / d);
        }

        public bool PointIsPartOf(Location2D loc)
	    {
		    return Math.Pow(loc.X - A, 2) + Math.Pow(loc.Y - B, 2) == Math.Pow(R, 2);
        }

        public Location2D Point(double radians)
        {
            return Centre + new Vector2D(Math.Cos(A), Math.Sin(B));
        }

        public LineEquation Tangent(Location2D loc)
        {
            if (PointIsPartOf(loc))
            {
                Vector2D vecPC = new Vector2D(loc, new Location2D(A, B));
                return new LineEquation(loc, vecPC);
            }
            else
                throw new Exception("The point is not part of the circle");
        }

        public string ToLatex()
        {
           //( x + -a) ^{ 2} +(y + -b) ^{ 2}= r ^{ 2}
            return "(x " + (A < 0 ? "+ " + -A : "- " + A) + ")^{2} + (y " + (B < 0 ? "+ " + -B : "- " + B) + ")^{2} = " + R + "^{2}";
        }

        public override string ToString()
        {
            return "(x " + (A < 0 ? "+ " + -A : "- " + A) + ")^2 + (y " + (B < 0 ? "+ " + -B : "- " + B) + ")^2 = " + R + "^2";
        }
    }
}