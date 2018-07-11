using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Stuff
{
    public static class XMLUtil
    {
        /// <param name="name">The name of the child.</param>
        /// <returns>Whether the this XElement contains a child of the specified name.</returns>
        public static bool Contains(this XElement element, XName name)
        {
            return element.Elements(name).Count(p => true) > 0;
        }

        /// <param name="predicate">The predicate to check against the child elements.</param>
        /// <returns>Whether this XElement contains a child matching the predicate.</returns>
        public static bool Contains(this XElement element, Func<XElement, bool> predicate)
        {
            foreach (XElement e in element.Elements())
            {
                if (predicate(e))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Finds all child elements matching a predicate.
        /// </summary>
        /// <param name="predicate">The predicate to check against the child elements.</param>
        /// <returns>An IEnumerable containing all matching child elements.</returns>
        public static IEnumerable<XElement> Where(this XElement element, Func<XElement, bool> predicate)
        {
            foreach (XElement e in element.Elements())
            {
                if (predicate(e))
                    yield return e;
            }
        }

        /// <param name="name">The name of the child to get the value of.</param>
        /// <returns>The value of a specified child.</returns>
        public static string ElementValue(this XElement element, XName name)
        {
            return element.Element(name).Value;
        }

        /// <param name="name">The name of the child to get the value of.</param>
        /// <returns>The value of a specified child.</returns>
        public static string ElementValueOrDefault(this XElement element, XName name, string defaultValue)
        {
            return element.Contains(name) ? element.Element(name).Value : defaultValue;
        }

        /// <returns>An IEnumerable containing the values of all elements contained in this element.</returns>
        public static IEnumerable<string> ElementValues(this XElement element)
        {
            foreach (XElement subElement in element.Elements())
                yield return subElement.Value;
        }

        /// <returns>An IEnumerable containing the values of all elements contained in this element with the specified name.</returns>
        public static IEnumerable<string> ElementValues(this XElement element, XName name)
        {
            foreach (XElement subElement in element.Elements(name))
                yield return subElement.Value;
        }

        public static void AddValue(this XElement element, XName name, object value)
        {
            element.Add(new XElement(name, value));
        }

        /// <summary>
        /// Adds an element with the given name and value if it doesn't exist, and replaces it if it does.
        /// </summary>
        /// <param name="name">The name of the element to add/replace.</param>
        /// <param name="value">The value of the element to add/replace.</param>
        public static void Put(this XElement element, XName name, object value)
        {
            if (element.Contains(name))
                element.Element(name).SetValue(value);
            else
                element.Add(new XElement(name, value));
        }

        /// <summary>
        /// Adds the given element if it doesn't exist, and replaces it if it does.
        /// </summary>
        /// <param name="input">The element to add/replace.</param>
        public static void Put(this XElement element, XElement input)
        {
            if (element.Contains(input.Name.LocalName))
                element.Element(input.Name.LocalName).Remove();
            element.Add(input);
        }

        /// <summary>
        /// Returns an element with the given name if such exists and creates a new, empty one if it doesn't.
        /// </summary>
        /// <param name="name">The name of the element to get.</param>
        public static XElement GetOrNew(this XElement element, XName name)
        {
            if (element.Contains(name))
                return element.Element(name);
            else
            {
                element.Add(new XElement(name));
                return element.Element(name);
            }
        }
    }
}
