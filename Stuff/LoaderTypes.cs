using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff
{
    public class LoaderTypes<T> : IEnumerable<XMLTypeLoader<T>>
    {
        private readonly Dictionary<string, XMLTypeLoader<T>> types;

        public LoaderTypes(PathList paths, string elementName, string assemblyName)
        {
            types = new Dictionary<string, XMLTypeLoader<T>>();
            foreach (var file in paths.Files())
            {
                foreach (var element in XMLUtil.FindElements(file, e => e.Name == elementName))
                {
                    var mt = new XMLTypeLoader<T>(element, assemblyName);
                    types[mt.Name] = mt;
                }
            }
        }

        public XMLTypeLoader<T> this[string typeName] => types[typeName];

        public IEnumerator<XMLTypeLoader<T>> GetEnumerator()
        {
            return types.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
