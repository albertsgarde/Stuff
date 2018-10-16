using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;

namespace Stuff.StuffMath
{
    public class ParametricEquation
    {
        public Vector Constant { get; }

        public IReadOnlyDictionary<string, Vector> Coefficients { get; }
        
        public ParametricEquation(Vector constant, IReadOnlyDictionary<string, Vector> coefs)
        {
            Constant = constant;
            Coefficients = coefs.Copy();
            foreach (var coef in Coefficients)
            {
                if (coef.Value.Size != Constant.Size)
                    throw new ArgumentException($"All coeffiecient vectors must have the same size (dimensionality) as the constant vector. The vector for {coef.Key} has the size {coef.Value.Size}, while the constant vector has size {Constant.Size}");
            }
        }

        public Vector Value(IReadOnlyDictionary<string, double> values)
        {
            if (!values.Keys.ContainsAll(Coefficients.Keys))
                throw new ArgumentException("Values must be provided for all variables.");
            var total = Constant;
            foreach (var coef in Coefficients)
                total += coef.Value * values[coef.Key];
            return total;
        }
    }
}
