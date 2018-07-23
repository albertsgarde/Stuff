using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Stuff
{
    /// <summary>
    /// Loads a class from an xmlfile that references the class and creates an instance.
    /// </summary>
    /// <remarks>The loaded types must have a default constructor</remarks>
    public class XMLTypeLoader<T>
    {
        public string ClassPath { get; }

        public string Name { get; }

        public T Instance { get; }

        /// <summary>
        /// Loads the type from an XElement.
        /// The element should have two children:
        ///     A "name" element, designating the internally used name of the type. Should be unique, but does not have to be the same as the class name.
        ///     A "classPath" element, designating the location where the class is found.
        /// </summary>
        /// <param name="element">An XElement that holds information about the location of the class to be loaded and what name to give the type.</param>
        public XMLTypeLoader(XElement element)
        {
            ClassPath = element.ElementValue("classPath");
            Name = element.ElementValue("name");
            Instance = (T)Activator.CreateInstance(Type.GetType(ClassPath));
        }
    }
}
