using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;

namespace Stuff.StuffMath
{
    public class LinearEquationSystem
    {
        public IReadOnlyList<LinearEquation> Equations { get; }
        
        public LinearEquationSystem(IEnumerable<LinearEquation> equations)
        {
            Equations = equations.Copy();
            var length = equations.First().Coefficients.Count;
            foreach (var le in Equations.Skip(1))
            {
                if (le.Coefficients.Count != length)
                    throw new ArgumentException("All equations must have the same length.");
            }
        }

        public LEMatrix CoefficientMatrix()
        {
            return new LEMatrix(Equations.Select(le => le.CoefficientMatrixRow()));
        }

        public LEMatrix BMatrix()
        {
            return new LEMatrix(Equations.Select(le => le.BMatrixRow()));
        }

        public LEMatrix ToMatrix()
        {
            return new LEMatrix(Equations.Select(le => le.ToMatrixRow()));
        }
    }
}
