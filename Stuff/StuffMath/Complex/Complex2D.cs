using Stuff.StuffMath.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath.Complex
{
    /// <summary>
    /// A complex number.
    /// </summary>
    public struct Complex2D : IHilbertField<Complex2D>
    {
        public double Real { get; private set; }

        public double Imaginary { get; private set; }

        public static readonly Complex2D I = new Complex2D(0, 1);

        public static readonly Complex2D NULL = new Complex2D(0, 0);

        public Complex2D ONE => new Complex2D(1, 0);

        public Complex2D ZERO => new Complex2D();

        public Complex2D(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public Complex2D(Vector2D v)
        {
            Real = v.X;
            Imaginary = v.Y;
        }

        public Complex2D(Location2D loc)
        {
            Real = loc.X;
            Imaginary = loc.Y;
        }

        /// <summary>
        /// Creates a complex of length 1 and with the given length.
        /// </summary>
        public Complex2D(double argument)
        {
            Real = Math.Cos(argument);
            Imaginary = Math.Sin(argument);
        }

        public static Complex2D Angular(double magnitude, double argument)
        {
            return new Complex2D(Math.Cos(argument) * magnitude, Math.Sin(argument) * magnitude);
        }

        public static implicit operator Complex2D(double d)
        {
            return new Complex2D(d, 0);
        }

        public static implicit operator Complex2D((double, double) c)
        {
            return new Complex2D(c.Item1, c.Item2);
        }

        public static implicit operator Complex2D(int i)
        {
            return new Complex2D(i, 0);
        }

        public void Deconstruct(out double a, out double b)
        {
            a = Real;
            b = Imaginary;
        }

        public static Complex2D operator+(Complex2D c1, Complex2D c2)
        {
            return new Complex2D(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary);
        }

        public static Complex2D operator +(Complex2D c, double d)
        {
            return new Complex2D(c.Real + d, c.Imaginary);
        }

        public static Complex2D operator +(double d, Complex2D c)
        {
            return new Complex2D(c.Real + d, c.Imaginary);
        }

        public static Complex2D operator +(Complex2D c)
        {
            return c;
        }

        public static Complex2D operator -(Complex2D c1, Complex2D c2)
        {
            return new Complex2D(c1.Real - c2.Real, c1.Imaginary - c2.Imaginary);
        }

        public static Complex2D operator -(Complex2D c, double d)
        {
            return new Complex2D(c.Real - d, c.Imaginary);
        }

        public static Complex2D operator -(double d, Complex2D c)
        {
            return new Complex2D(d - c.Real, -c.Imaginary);
        }

        public static Complex2D operator -(Complex2D c)
        {
            return (-c.Real, -c.Imaginary);
        }

        public static Complex2D operator *(Complex2D c1, Complex2D c2)
        {
            return new Complex2D(c1.Real * c2.Real - c1.Imaginary * c2.Imaginary, c1.Real * c2.Imaginary + c2.Real * c1.Imaginary);
        }

        public static Complex2D operator *(Complex2D c, double d)
        {
            return new Complex2D(c.Real * d, c.Imaginary * d);
        }

        public static Complex2D operator *(double d, Complex2D c)
        {
            return new Complex2D(c.Real * d, c.Imaginary * d);
        }

        public static Complex2D operator /(Complex2D c1, Complex2D c2)
        {
            var a = c1.Real;
            var b = c1.Imaginary;
            var c = c2.Real;
            var d = c2.Imaginary;
            var recMagSquare = c2.AbsoluteSquared;
            return new Complex2D((a * c + b * d) * recMagSquare, (b * c - a * d) * recMagSquare);
        }

        public static Complex2D operator /(Complex2D c, double d)
        {
            return new Complex2D(c.Real / d, c.Imaginary / d);
        }

        public static Complex2D operator /(double d, Complex2D c)
        {
            var a = d;
            var r = c.Real;
            var i = c.Imaginary;
            var recMagSquare = c.AbsoluteSquared;
            return new Complex2D(a * r * recMagSquare, - a * d * recMagSquare);
        }

        public static bool operator ==(Complex2D c1, Complex2D c2)
        {
            return c1.Real == c2.Real && c1.Imaginary == c2.Imaginary;
        }

        public static bool operator !=(Complex2D c1, Complex2D c2)
        {
            return c1.Real != c2.Real || c1.Imaginary != c2.Imaginary;
        }

        public Complex2D Add(Complex2D c) => this + c;

        public Complex2D Multiply(Complex2D c) => this * c;

        public Complex2D Multiply(FDouble d) => this * d;

        public Complex2D AdditiveInverse() => -this;

        public Complex2D Reciprocal()
        {
            return Angular(1 / Absolute, -Argument);
        }

        public Complex2D MultiplicativeInverse() => Reciprocal();

        /// <summary>
        /// Takes the square root of a double.
        /// </summary>
        public static Complex2D Sqrt(double d)
        {
            return d < 0 ? new Complex2D(0, Math.Sqrt(-d)) : new Complex2D(Math.Sqrt(d), 0);
        }

        public Complex2D Square()
        {
            return this * this;
        }

        public (Complex2D, Complex2D) Sqrt()
        {
            var result1 = new Complex2D(Argument / 2) * Math.Sqrt(Absolute);
            var result2 = new Complex2D(Argument / 2 + Math.PI) * Math.Sqrt(Absolute);
            return (result1, result2);
        }

        public Complex2D AbsSqrt()
        {
            return Math.Sqrt(Absolute);
        }

        public double AbsoluteDistance(Complex2D c1, Complex2D c2)
        {
            return (c2 - c1).Absolute;
        }

        /// <summary>
        /// Returns only one of the possibly infinite results.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Complex2D Power(double d)
        {
            return Angular(Math.Pow(Absolute, d), Argument * d);
        }
        
        /// <returns>The complex result of e^θi.</returns>
        public static Complex2D Exp(double θ)
        {
            return new Complex2D(Math.Cos(θ), Math.Sin(θ));
        }

        /// <returns>The complex result of e^z where z is this complex number.</returns>
        public Complex2D Exp()
        {
            return Exp(Imaginary)*Math.Exp(Real);
        }
        
        /// <returns>The natural logarithm where -pi < b < pi </returns>
        public Complex2D Ln()
        {
            return new Complex2D(Math.Log(AbsoluteSquared)*(1/2), Argument);
        }

        public static Complex2D Power(double root, Complex2D exponent)
        {
            return (Math.Log(root) * exponent).Exp();
        }

        public Complex2D Root(double d)
        {
            return new Complex2D(Argument / d) * Math.Pow(AbsoluteSquared, 1/(2*d));
        }

        public Complex2D Conjugate()
        {
            return new Complex2D(Real, -Imaginary);
        }

        public Complex2D InnerProduct(Complex2D c)
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

        public double Distance(Complex2D c)
        {
            return (this - c).Absolute;
        }

        public Circle Circle(double radius)
        {
            return new Circle(Real, Imaginary, radius);
        }

        /// <summary>
        /// The radians between 0 and 2π.
        /// </summary>
        public double Argument
        {
            get
            {
                if (Real == 0 && Imaginary == 0)
                    return 0;
                else
                    return Imaginary < 0 ? 0 - Math.Acos(Real / Absolute) : Math.Acos(Real / Absolute);
            }
        }

        public override string ToString()
        {
            if (Imaginary != 0)
                return $"({Real} {(Imaginary >= 0 ? "+" : "-")} i{Math.Abs(Imaginary)})";
            else
                return "" + Real;
        }

        public string RadialToString()
        {
            return $"({Absolute},{Argument/Math.PI}π)";
        }

        public FDouble RealPart()
        {
            return Real;
        }

        public bool EqualTo(Complex2D c)
        {
            return Real == c.Real && Imaginary == c.Imaginary;
        }

        public override bool Equals(object obj)
        {
            if (obj is Complex2D c)
            {
                return EqualTo(c);
            }
            else if (obj is double d)
            {
                return Real == d && Imaginary == 0;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return Misc.HashCode(17, 23, Real, Imaginary);
        }
    }
}
