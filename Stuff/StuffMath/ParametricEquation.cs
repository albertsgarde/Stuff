using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;
using Stuff.StuffMath.Structures;

namespace Stuff.StuffMath
{
    public class ParametricEquation<F> where F : IField<F>, new()
    {
        public Vector<F> Constant { get; }

        public IReadOnlyList<Vector<F>> Coefficients { get; }
        
        public ParametricEquation(Vector<F> constant, IReadOnlyList<Vector<F>> coefs)
        {
            Constant = constant;
            Coefficients = coefs.Copy();
            foreach (var coef in Coefficients)
            {
                if (coef.Size != Constant.Size)
                    throw new ArgumentException($"All coeffiecient vectors must have the same size (dimensionality) as the constant vector. The vector {coef} has the size {coef.Size}, while the constant vector has size {Constant.Size}");
            }
        }

        public Vector<F> Value(IReadOnlyList<F> values)
        {
            if (values.Count != Coefficients.Count)
                throw new ArgumentException("Values must be provided for all variables.");
            var total = Constant;
            for (int i = 0; i < values.Count; ++i)
                total += values[i] * Coefficients[i];
            return total;
        }

        public override string ToString()
        {
            var result = Constant.IsNull() ? "" : Constant.ToString();
            foreach (var coef in Coefficients)
                result += " + s" + coef;
            return result.TrimStart(' ', '+');
        }
    }
}
