using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff.StuffMath
{
    public class Regression
    {
        /// <summary>
        /// Calculates the r^2-value of some data with a specified fit.
        /// </summary>
        /// <param name="data">The data to fit the function to.</param>
        /// <param name="function">The function to fit to the data.</param>
        /// <returns>The r^2-value of the function on the data.</returns>
        public static double RSquared(IEnumerable<KeyValuePair<double, double>> data, Func<double, double> function)
        {
            double dataAverage = data.Sum(dataPoint => dataPoint.Value)/data.Count();
            double ssTot = data.Sum(dataPoint => Math.Pow(dataPoint.Value - dataAverage, 2));
            double ssRes = data.Sum(dataPoint => Math.Pow(dataPoint.Value - function.Invoke(dataPoint.Key), 2));
            return 1 - (ssRes / ssTot);
        }

        /// <summary>
        /// Calculates a linear regression for the data with a specific y-axis intersection.
        /// </summary>
        /// <param name="data">The data for which a linear regression will be calculated.</param>
        /// <param name="b">The point on the y-axis to cross. Deafaults to 0.</param>
        /// <returns>The slope of the linear regression.</returns>
        public static double LinearRegressionSlope(IEnumerable<KeyValuePair<double, double>> data, double b = 0)
        {
            double x = data.Sum(dataPoint => dataPoint.Key);
            double xx = data.Sum(dataPoint => dataPoint.Key * dataPoint.Key);
            double xy = data.Sum(dataPoint => dataPoint.Key * dataPoint.Value);
            return (xy-b*x)/xx;
        }

        /// <summary>
        /// Calculates an inverse proportionality regression for the data.
        /// </summary>
        /// <param name="data">The data for which a regression will be calculated.</param>
        /// <returns>The slope of the inverse proportionality regression.</returns>
        public static double InverseProportionality(IEnumerable<KeyValuePair<double, double>> data)
        {
            double ynx = data.Sum(dataPoint => dataPoint.Value / dataPoint.Key);
            double nxx = data.Sum(dataPoint => Math.Pow(dataPoint.Key, -2));
            return ynx / nxx;
        }

        /// <summary>
        /// Calculates any power proportionality regression for the data.
        /// A regression to the function y=bx^a where the a is given.
        /// </summary>
        /// <param name="data">The data for which a regression will be calculated.</param>
        /// <returns>The slope of the proportionality regression.</returns>
        public static double PowerProportionality(IEnumerable<KeyValuePair<double, double>> data, double a = 1)
        {
            double xay = data.Sum(dataPoint => Math.Pow(dataPoint.Key, a) * dataPoint.Value);
            double x2a = data.Sum(dataPoint => Math.Pow(dataPoint.Key, 2 * a));
            return xay / x2a;
        }

        public static LineEquation LinearRegression(IEnumerable<KeyValuePair<double, double>> data)
        {
            double xSum = data.Sum(dataPoint => dataPoint.Key);
            double ySum = data.Sum(dataPoint => dataPoint.Value);
            double xxSum = data.Sum(dataPoint => dataPoint.Key * dataPoint.Key);
            double xySum = data.Sum(dataPoint => dataPoint.Key * dataPoint.Value);
            double b = (-(xySum * xSum - xxSum * ySum)) / (xSum * xSum + xxSum);
            double a = (xySum - b * xSum)/xxSum;
            return new LineEquation(a, b); // y=ax+b
        }

        public static QuadraticFunction QuadraticRegression(IEnumerable<KeyValuePair<double, double>> data)
        {
            double xSum = data.Sum(dataPoint => dataPoint.Key);
            double xxSum = data.Sum(dataPoint => dataPoint.Key * dataPoint.Key);
            double xxxSum = data.Sum(dataPoint => dataPoint.Key * dataPoint.Key * dataPoint.Key);
            double xxxxSum = data.Sum(dataPoint => dataPoint.Key * dataPoint.Key * dataPoint.Key * dataPoint.Key);
            double ySum = data.Sum(dataPoint => dataPoint.Value);
            double xySum = data.Sum(dataPoint => dataPoint.Key * dataPoint.Value);
            double xxySum = data.Sum(dataPoint => dataPoint.Key * dataPoint.Key * dataPoint.Value);

            double a = (xxySum * xSum * xSum - xSum * (xxxSum * ySum + xxSum * xySum) + xxSum * xxSum * ySum - xxSum * xxySum + xxxSum * xySum) / 
                (xxxxSum * xSum * xSum - 2 * xSum * xxSum * xxxSum + xxSum * xxSum * xxSum - xxSum * xxxxSum + xxxSum * xxxSum);

            double b = (xSum * (xxxxSum * ySum - xxSum * xxySum) - xxSum * xxxSum * ySum + xxSum * xxSum * xySum + xxxSum * xxySum - xxxxSum * xySum) /
                (xxxxSum * xSum * xSum - 2 * xSum * xxSum * xxxSum+xxSum*xxSum*xxSum-xxSum*xxxxSum+xxxSum*xxxSum);

            double c = (-((xxxSum*xxySum-xxxxSum*xySum)*xSum+(xxSum*xxxxSum-xxxSum*xxxSum)*ySum-xxSum*(xxSum*xxySum-xxxSum*xySum)))/
                (xxxxSum*xSum*xSum)-2*xSum*xxSum*xxxSum+xxSum*xxSum*xxSum-xxSum*xxxxSum+xxxSum*xxxSum;

            return new QuadraticFunction(a, b, c);
        }

        public static QuadraticFunction QuadraticRegressionLockedC(IEnumerable<KeyValuePair<double, double>> data)
        {
            double xx = data.Sum(dataPoint => dataPoint.Key * dataPoint.Key);
            double xxx = data.Sum(dataPoint => dataPoint.Key * dataPoint.Key * dataPoint.Key);
            double xxxx = data.Sum(dataPoint => dataPoint.Key * dataPoint.Key * dataPoint.Key * dataPoint.Key);
            double xy = data.Sum(dataPoint => dataPoint.Key * dataPoint.Value);
            double xxy = data.Sum(dataPoint => dataPoint.Key * dataPoint.Key * dataPoint.Value);

            double a = 2 * (2 * xx * xxy - xxx * xy) / (4 * xx * xxxx - xxx * xxx);
            double b = -2 * (xxx * xxy - 2 * xxxx * xy) / (4 * xx * xxxx - xxx * xxx);
            return new QuadraticFunction(a, b, 0);
        }

        public static double PowerWithCoefficient(IEnumerable<KeyValuePair<double, double>> data, double power)
        {
            double xpky = data.Sum(dataPoint => Math.Pow(dataPoint.Key, power) * dataPoint.Value);
            double xp2k = data.Sum(dataPoint => Math.Pow(dataPoint.Key, power * 2));
            return xpky / xp2k;
        }
    }
}