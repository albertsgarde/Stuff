using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff
{
    public class CrossReferencedDictionary<V1, V2> : IEnumerable<KeyValuePair<V1, V2>>
    {
        private Dictionary<V1, V2> mapper1;

        private Dictionary<V2, V1> mapper2;

        public CrossReferencedDictionary()
        {
            mapper1 = new Dictionary<V1, V2>();
            mapper2 = new Dictionary<V2, V1>();
        }

        public void Clear()
        {
            mapper1.Clear();
            mapper2.Clear();
        }

        public int Count()
        {
            return mapper1.Count();
        }

        public bool Contains(V1 v1)
        {
            return mapper1.ContainsKey(v1);
        }

        public bool Contains(V2 v2)
        {
            return mapper2.ContainsKey(v2);
        }

        public V2 this[V1 key]
        {
            get
            {
                return mapper1[key];
            }
            set
            {
                Add(key, value);
            }
        }

        public V1 this[V2 key]
        {
            get
            {
                return mapper2[key];
            }
            set
            {
                Add(value, key);
            }
        }

        private void Add(V1 v1, V2 v2)
        {
            mapper2.Remove(mapper1[v1]);
            mapper1.Remove(mapper2[v2]);
            mapper1[v1] = v2;
            mapper2[v2] = v1;
        }

        public void Remove(V1 key)
        {
            mapper2.Remove(mapper1[key]);
            mapper1.Remove(key);
        }

        public void Remove(V2 key)
        {
            mapper1.Remove(mapper2[key]);
            mapper2.Remove(key);
        }

        public IEnumerator<KeyValuePair<V1, V2>> GetEnumerator()
        {
            return mapper1.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
