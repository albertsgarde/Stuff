﻿using System;
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
        public LEVector Constant { get; }

        public IReadOnlyList<LEVector> Coefficients { get; }
        
        public ParametricEquation(LEVector constant, IReadOnlyList<LEVector> coefs)
        {
            Constant = constant;
            Coefficients = coefs.Copy();
            foreach (var coef in Coefficients)
            {
                if (coef.Size != Constant.Size)
                    throw new ArgumentException($"All coeffiecient vectors must have the same size (dimensionality) as the constant vector. The vector {coef} has the size {coef.Size}, while the constant vector has size {Constant.Size}");
            }
        }

        public LEVector Value(IReadOnlyList<double> values)
        {
            if (values.Count != Coefficients.Count)
                throw new ArgumentException("Values must be provided for all variables.");
            var total = Constant;
            for (int i = 0; i < values.Count; ++i)
                total += values[i] * Coefficients[i];
            return total;
        }
    }
}
