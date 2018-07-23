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
        private readonly Dictionary<string, XMLTypeLoader<T>> moduleTypes;

        public LoaderTypes(PathList paths, string elementName)
        {
            foreach (var file in paths)
            {
                foreach (var element in XMLUtil.FindElements(file, e => e.Name == elementName))
                {
                    var mt = new XMLTypeLoader<T>(element);
                    moduleTypes[mt.Name] = mt;
                }
            }
        }

        public XMLTypeLoader<T> this[string typeName] => moduleTypes[typeName];

        public IEnumerator<XMLTypeLoader<T>> GetEnumerator()
        {
            return moduleTypes.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
