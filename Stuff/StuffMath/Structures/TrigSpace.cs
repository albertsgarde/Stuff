using Stuff.StuffMath.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;

namespace Stuff.StuffMath.Structures
{
    public class TrigSpace : IVectorSpace<TrigSpace, FDouble>
    {
        public IReadOnlyDictionary<double, Complex2D> Waves { get; }

        public FDouble ONE => new FDouble().ONE;

        public TrigSpace ZERO => new TrigSpace();

        public TrigSpace()
        {
            Waves = new Dictionary<double, Complex2D>();
        }

        public TrigSpace(params (double frequency, Complex2D phaseAmplitude)[] waves)
        {
            var result = new Dictionary<double, Complex2D>();
            foreach(var (f, pa) in waves)
            {
                if (result.ContainsKey(f))
                    result[f] += pa;
                else
                    result.Add(f, pa);
            }
            Waves = result;
        }

        public TrigSpace(IReadOnlyDictionary<double, Complex2D> waves)
        {
            Waves = waves.Copy();
        }

        public TrigSpace Add(TrigSpace t)
        {
            throw new NotImplementedException();
        }

        public TrigSpace AdditiveInverse()
        {
            var result = new Dictionary<double, Complex2D>();
            foreach (var (f, pa) in Waves.Select(kvp => (kvp.Key, kvp.Value)))
                result.Add(f, -pa);
            return new TrigSpace(result);
        }

        public TrigSpace Multiply(FDouble s)
        {
            throw new NotImplementedException();
        }

        public Vector<FDouble> ToVector()
        {
            throw new NotImplementedException();
        }

        public bool EqualTo(TrigSpace t)
        {
            throw new NotImplementedException();
        }
    }
}
