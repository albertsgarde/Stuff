using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff.StuffMath.Expressions;
using Stuff.StuffMath.Structures;

namespace Stuff.StuffMath
{
    public class LineEquation : IPolynomial
    {
        /// <summary>
        /// ax+by+c=0
        /// </summary>
        public double A { get; private set; }

        /// <summary>
        /// ax+by+c=0
        /// </summary>
        public double B { get; private set; }

        /// <summary>
        /// ax+by+c=0
        /// </summary>
        public double C { get; private set; }

        public int Degree => 1;

        public IPolynomial ZERO => new LineEquation(0, 0);

        public Real ONE => 1;

        public double this[int exponent] => Coefficient(exponent);

        /// <summary>
        /// ax+by+c=0
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public LineEquation(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        /// <summary>
        /// y=ax+b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public LineEquation(double a, double b)
        {
            A = a;
            B = -1;
            C = b;
        }

        /// <summary>
        /// MAY NOT WORK
        /// </summary>
        /// <param name="loc1"></param>
        /// <param name="loc2"></param>
        public LineEquation(Location2D loc1, Location2D loc2)
        {
            Vector2D vector = new Vector2D(loc1, loc2);
            A = vector.Y;
            B = vector.X;
            C = -(A * loc1.X + B * loc1.Y);
        }

        public LineEquation(Location2D coordinate, Vector2D orthogonalVector)
        {
            A = -orthogonalVector.X / orthogonalVector.Y;
            B = -1;
            C = coordinate.Y - (coordinate.X * A);
            /*
            A = orthogonalVector.X;
            B = orthogonalVector.Y;
            C = coordinate.Y - coordinate.X*B/A;
            */
        }

        public LineEquation(Location2D coordinate, double slope)
        {
            double a = slope;
            double b = coordinate.Y - (coordinate.X * slope);
            A = a;
            B = -1;
            C = b;
        }

        public static LineEquation operator *(LineEquation l, double d)
        {
            return new LineEquation(l.A * d, l.B * d, l.C *d);
        }

        public static LineEquation operator /(LineEquation l, double d)
        {
            return new LineEquation(l.A / d, l.B / d, l.C / d);
        }

        public static LineEquation operator -(LineEquation l, LineEquation lf)
        {
            return new LineEquation(l.A - lf.A, l.B - lf.B, l.C - lf.C);
        }

        public static LineEquation operator +(LineEquation l, LineEquation lf)
        {
            return new LineEquation(l.A + lf.A, l.B + lf.B, l.C + lf.C);
        }

        public LineEquation Subtract(LineEquation lf)
        {
            return new LineEquation(A - lf.A, B - lf.B, C - lf.C);
        }

        public double Y(double x)
        {
            return (A*x+C)/B*-1;
        }

        public double X(double y)
        {
            return (B * y + C) / A * -1;
        }

        public Location2D PointY(double x)
        {
            return new Location2D(x, Y(x));
        }

        public Location2D PointX(double y)
        {
            return new Location2D(X(y), y);
        }

        public  IPolynomial Differentiate()
        {
            return new Polynomial((0, A / B * -1));
        }

        public  IPolynomial Integrate()
        {
            return new QuadraticFunction(A / B * -1 / 2, C / B * -1, double.NaN);
        }

        public  double Integrate(double a, double b)
        {
            return (A * a + C) / B * -1 * (b - a) + (A * (b-a) + C) / B * (b - a) * -0.5;
        }

        public double Coefficient(int exponent)
        {
            switch(exponent)
            {
                case 0:
                    return C / B * -1;
                case 1:
                    return A / B * -1;
                default:
                    return 0;
            }
        }

        public  Polynomial AsPolynomial()
        {
            return new Polynomial((1, A / B * -1), (0, C/B*-1));
        }

        public Vector ToVector()
        {
            return new Vector(C / B * -1, A / B * -1);
        }

        public  IPolynomial MoveVertical(double k)
        {
            return new LineEquation(A, B, C + k * -B);
        }

        public  IPolynomial MoveHoriz(double k)
        {
            return new LineEquation(A, A * k + B);
        }

        public  IPolynomial Transform(Vector2D v)
        {
            return MoveVertical(v.Y).MoveHoriz(v.X);
        }

        public double ZeroIntersection()
        {
            return Intersection(new LineEquation(0, 0)).X;
        }

        public Location2D Intersection(LineEquation lf)
        {
            if (this.IsSame(lf))
                throw new Exception("The two functions are the same");
            else if (this.IsParallel(lf))
                throw new Exception("The two functions are parallel");

            LineEquation tempThis = this * lf.A;
            LineEquation tempThat = lf * A;

            LineEquation subtraction = tempThis.Subtract(tempThat);

            double y = (-subtraction.C) / subtraction.B;
            double x = this.X(y);
            return new Location2D(x, y);
        }

        public bool IsSame(LineEquation lf)
        {
            return Misc.DoubleEquals(A / B, lf.A / lf.B) && Misc.DoubleEquals(A / C, lf.A / lf.B);
        }

        public bool IsParallel(LineEquation lf)
        {
            return Misc.DoubleEquals(A / B, lf.A / lf.B);
        }

        public bool IsOrthogonal(LineEquation lf)
        {
            return A / B == lf.A / lf.B;
        }

        public Vector2D DirectionVector()
        {
            return new Vector2D(1, A / -B);
        }

        public Vector2D OrthogonalVector()
        {
            return new Vector2D(A / B, 1);
        }

        /// <returns>A point on the line.</returns>
        public Location2D PointOnLine()
        {
            return B != 0 ? new Location2D(0, Y(0)) : new Location2D(X(0), 0);
        }

        public  Expression ToExpression(string variableName)
        {
            return A * new Variable(variableName) + B;
        }
        
        /// <param name="x">The x-coordinate of the LinearParameter's b vector.</param>
        /// <returns>A LinearParamater that is equivalent to this LinearFunction.</returns>
        public LinearParameter2D LinearParameter(double x = 0)
        {
            return new LinearParameter2D(DirectionVector(), new Vector2D(x, Y(x)));
        }

        public static implicit operator LinearParameter2D(LineEquation lf)
        {
            return lf.LinearParameter();
        }

        public override string ToString()
        {
            string text = "";
            if (A == 1)
                text += "x ";
            else if (A != 0)
                text += A + "x ";
            
            if (B < 0)
                text += "- " + -B + "y ";
            else if (B != 0)
            {
                if (text.Length != 0)
                    text += "+ ";
                if (B == 1)
                    text += "y ";
                else
                    text += B + "y ";
            }

            if (C < 0)
                text += "- " + -C + " ";
            else if (C != 0)
            {
                if (text.Length != 0)
                    text += "+ ";
                text += C + " ";
            }
            return text + "= 0";
        }

        public override bool Equals(object obj)
        {
            return obj is IPolynomial p && AsPolynomial() == p;
        }

        public override int GetHashCode()
        {
            return Misc.HashCode(17, 23, A, B, C);
        }

        public IEnumerator<double> GetEnumerator()
        {
            yield return C / B * -1;
            yield return A / B * -1;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IPolynomial Add(IPolynomial t)
        {
            return AsPolynomial() + t.AsPolynomial();
        }

        public IPolynomial AdditiveInverse()
        {
            return new LineEquation(-A, -B, -C);
        }

        public IPolynomial Multiply(Real s)
        {
            return new LineEquation(A * (double)s, B * (double)s, C * (double)s);
        }

        public bool EqualTo(IPolynomial t)
        {
            return Equals(t);
        }
    }
}
