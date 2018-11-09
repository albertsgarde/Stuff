using Stuff.StuffMath.Complex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;

namespace Stuff.StuffMath.Structures
{
    public class TrigSpace : IVectorSpace<TrigSpace, Complex2D>
    {
        public IReadOnlyDictionary<double, Complex2D> Waves { get; }

        public Complex2D ONE => new Complex2D().ONE;

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

        public Complex2D this[double d] => Waves[d];

        public Complex2D Value(double t)
        {
            var result = new Complex2D();
            foreach (var (f, pa) in Waves.Select(kvp => (kvp.Key, kvp.Value)))
                result += Complex2D.Exp(f * t + pa.Argument) * pa.Absolute;
            return result;
        }

        public TrigSpace Add(TrigSpace t)
        {
            var result = new Dictionary<double, Complex2D>();
            foreach (var (f, pa) in Waves.Select(kvp => (kvp.Key, kvp.Value)))
            {
                if (result.ContainsKey(f))
                    result[f] += pa;
                else
                    result.Add(f, pa);
            }
            foreach (var (f, pa) in t.Waves.Select(kvp => (kvp.Key, kvp.Value)))
            {
                if (result.ContainsKey(f))
                    result[f] += pa;
                else
                    result.Add(f, pa);
            }
            return new TrigSpace(result);
        }

        public TrigSpace AdditiveInverse()
        {
            var result = new Dictionary<double, Complex2D>();
            foreach (var (f, pa) in Waves.Select(kvp => (kvp.Key, kvp.Value)))
                result.Add(f, -pa);
            return new TrigSpace(result);
        }

        public TrigSpace Multiply(Complex2D s)
        {
            var result = new Dictionary<double, Complex2D>();
            foreach (var (f, pa) in Waves.Select(kvp => (kvp.Key, kvp.Value)))
                result.Add(f, pa * s);
            return new TrigSpace(result);
        }

        public Vector<Complex2D> ToVector()
        {
            return new Vector<Complex2D>(Waves.Values);
        }

        public bool EqualTo(TrigSpace t)
        {
            if (Waves.Keys.Count() != Waves.Keys.Count() || !Waves.Keys.ContainsAll(t.Waves.Keys))
                return false;
            foreach (var f in Waves.Keys)
            {
                if (t.Waves[f] != Waves[f])
                    return false;
            }
            return true;
        }
    }
}
