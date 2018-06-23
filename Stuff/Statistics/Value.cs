using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.Statistics
{
    public class Value
    {
        public double Mean { get; private set; }
        
        public double Uncertainty { get; private set; }

        public Value(double value, double uncertainty)
        {
            Mean = value;
            Uncertainty = uncertainty;
            if (Uncertainty < 0)
                throw new Exception("Uncertainty must be positive or zero.");
        }

        public double MinValue
        {
            get
            {
                return Mean - Uncertainty;
            }
        }

        public double MaxValue
        {
            get
            {
                return Mean + Uncertainty;
            }
        }

        public static Value operator +(Value v1, Value v2)
        {
            return new Value(v1.Mean + v2.Mean, v1.Uncertainty + v2.Uncertainty);
        }

        public static Value operator -(Value v1, Value v2)
        {
            return new Value(v1.Mean - v2.Mean, v1.Uncertainty + v2.Uncertainty);
        }

        public static Value operator *(Value v1, Value v2)
        {
            if (v1.MinValue < 0 || v2.MinValue < 0)
                throw new Exception("In order to multiply values, boths minimum values must be positive");
            double minValue = v1.MinValue * v2.MinValue;
            double maxValue = v1.MaxValue * v2.MaxValue;
            return new Value((minValue + maxValue) / 2, (maxValue - minValue) / 2);
        }

        public static Value operator /(Value v1, Value v2)
        {
            if (v1.MinValue < 0 || v2.MinValue < 0)
                throw new Exception("In order to multiply values, boths minimum values must be positive");
            double minValue = v1.MinValue / v2.MaxValue;
            double maxValue = v1.MaxValue / v2.MinValue;
            return new Value((minValue + maxValue) / 2, (maxValue - minValue) / 2);
        }

        public static Value operator +(Value v, double d)
        {
            return new Value(v.Mean + d, v.Uncertainty);
        }

        public static Value operator -(Value v, double d)
        {
            return new Value(v.Mean - d, v.Uncertainty);
        }

        public static Value operator *(Value v, double d)
        {
            return new Value(v.Mean * d, v.Uncertainty * d);
        }

        public static Value operator /(Value v, double d)
        {
            return new Value(v.Mean / d, v.Uncertainty / d);
        }

        public static implicit operator double(Value v)
        {
            return v.Mean;
        }
    }
}
