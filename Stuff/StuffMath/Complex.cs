using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    /// <summary>
    /// A complex number.
    /// </summary>
    public class Complex
    {
        public double Real { get; private set; }

        public double Imaginary { get; private set; }

        public static readonly Complex ONE = new Complex(1, 0);

        public static readonly Complex I = new Complex(0, 1);

        public static readonly Complex NULL = new Complex(0, 0);

        public Complex(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public static implicit operator Complex(double d)
        {
            return new Complex(d, 0);
        }

        public static Complex operator+(Complex c1, Complex c2)
        {
            return new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        }

        public static Complex operator +(Complex c, double d)
        {
            return new Complex(c.Real + d, c.Imaginary);
        }

        public static Complex operator +(double d, Complex c)
        {
            return new Complex(c.Real + d, c.Imaginary);
        }

        public static Complex operator +(Complex c)
        {
            return c;
        }

        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);
        }

        public static Complex operator -(Complex c, double d)
        {
            return new Complex(c.Real - d, c.Imaginary);
        }

        public static Complex operator -(double d, Complex c)
        {
            return new Complex(c.Real - d, c.Imaginary);
        }

        public static Complex operator -(Complex c)
        {
            return new Complex(-c.Real, -c.Imaginary);
        }

        public static Complex operator *(Complex c1, Complex c2)
        {
            return new Complex(c1.Real * c2.Real - c1.Imaginary * c2.Imaginary, c1.Real * c2.Imaginary + c2.Real * c1.Imaginary);
        }

        public static Complex operator *(Complex c, double d)
        {
            return new Complex(c.Real * d, c.Imaginary * d);
        }

        public static Complex operator *(double d, Complex c)
        {
            return new Complex(c.Real * d, c.Imaginary * d);
        }

        public static Complex operator /(Complex c1, Complex c2)
        {
            var a = c1.Real;
            var b = c1.Imaginary;
            var c = c2.Real;
            var d = c2.Imaginary;
            return new Complex(a*c/(c*c+d*d)+b*d/(c*c+d*d), b*c/(c*c+d*d) - a*d/(c*c+d*d));
        }

        public static Complex operator /(Complex c, double d)
        {
            return new Complex(c.Real / d, c.Imaginary / d);
        }

        public static Complex operator /(double d, Complex c)
        {
            var a = d;
            var r = c.Real;
            var i = c.Imaginary;
            return new Complex(a * r / (r * r + i * i), - a * d / (r * r + i * i));
        }

        /// <summary>
        /// Takes the square root of a double.
        /// </summary>
        public static Complex Sqrt(double d)
        {
            return d < 0 ? new Complex(0, Math.Sqrt(-d)) : new Complex(Math.Sqrt(d), 0);
        }

        public Complex Square()
        {
            return this * this;
        }
        
        /// <returns>The complex result of e^θi.</returns>
        public static Complex Exp(double θ)
        {
            return new Complex(Math.Cos(θ), Math.Sin(θ));
        }

        /// <returns>The complex result of e^z where z is this complex number.</returns>
        public Complex Exp()
        {
            return Math.Exp(Real) * Exp(Imaginary);
        }

        /// <returns>The complex result of e^z.</returns>
        public static Complex Exp(Complex z)
        {
            return Math.Exp(z.Real) * Exp(z.Imaginary);
        }

        /// <summary>
        /// Uses the radians between 0 and 2π.
        /// </summary>
        public Complex Ln()
        {
            return new Complex(Math.Log(Absolute), Radians);
        }

        /// <summary>
        /// Uses the radians between 0 and 2π.
        /// </summary>
        /// <returns>The complex result of z^x where z is this complex number.</returns>
        public Complex Power(double x)
        {
            return Exp(Real*Math.Log(x) + Imaginary*Math.Log(x)*I);
        }

        /// <summary>
        /// Uses the radians between 0 and 2π.
        /// </summary>
        /// <returns>x^z</returns>
        public static Complex Power(double x, Complex z)
        {
            return Exp(x*z.Ln());
        }

        /// <returns>z^x where both are complex and z is this complex number.</returns>
        public Complex Power(Complex x)
        {
            return (x * Math.Log(Real) + I * x * Radians).Exp();
        }

        public Complex Conjugate()
        {
            return new Complex(Real, -Imaginary);
        }

        public Complex InnerProduct(Complex c)
        {
            return this * c.Conjugate();
        }

        public double Absolute
        {
            get
            {
                return Math.Sqrt(AbsoluteSquared);
            }
        }

        public double AbsoluteSquared
        {
            get
            {
                return Real * Real + Imaginary * Imaginary;
            }
        }

        /// <summary>
        /// The radians between 0 and 2π.
        /// </summary>
        public double Radians
        {
            get
            {
                return Imaginary < 0 ? 2 * Math.PI - Math.Acos(Real / Absolute) : Math.Acos(Real / Absolute);
            }
        }

        public override string ToString()
        {
            return "(" + Real + " + " + Imaginary + "i)";
        }
    }
}
