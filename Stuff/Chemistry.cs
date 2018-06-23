using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Stuff
{
    public static class Chemistry
    {
        private static readonly LinkedList<Element> elements = new LinkedList<Element>();

        public const double MOL = 602214129000000000000000d;

        public const double GRAMS_PER_UNIT = 0.00000000000000000000000166;

        /// <summary>
        /// L*bar/mol*K
        /// V*p/n*T
        /// </summary>
        public const double GAS_CONSTANT = 0.0831;

        /// <summary>
        /// K
        /// </summary>
        public const double STANDARD_TEMPERATURE = 293.15;

        /// <summary>
        /// bar
        /// </summary>
        public const double STANDARD_PRESSURE = 1;

        public const double CELCIUS0 = 273.15;

        /// <summary>
        /// L/mol
        /// </summary>
        public const double STANDARD_VOLUME_PER_MOL = 24.4;

        public static LinkedList<Element> Elements
        {
            get
            {
                if (elements.Count == 0)
                {
                    elements.AddLast(new Element("Hydrogen", "H", 1, 1.00794));
                    elements.AddLast(new Element("Helium", "He", 2, 4.0026));

                    elements.AddLast(new Element("Lithium", "Li", 3, 6.941));
                    elements.AddLast(new Element("Beryllium", "Be", 4, 9.01218));
                    elements.AddLast(new Element("Bohr", "B", 5, 10.811));
                    elements.AddLast(new Element("Carbon", "C", 6, 12.011));
                    elements.AddLast(new Element("Nitrogen", "N", 7, 14.00674));
                    elements.AddLast(new Element("Oxygen", "O", 8, 15.9994));
                    elements.AddLast(new Element("Flourine", "F", 9, 18.9984));
                    elements.AddLast(new Element("Neon", "Ne", 10, 20.1797));

                    elements.AddLast(new Element("Natrium", "Na", 11, 22.98977));
                    elements.AddLast(new Element("Magnesium", "Mg", 12, 24.3050));
                    elements.AddLast(new Element("Aluminium", "Al", 13, 26.98154));
                    elements.AddLast(new Element("Silicon", "Si", 14, 28.0855));
                    elements.AddLast(new Element("Phosphor", "P", 15, 30.97376));
                    elements.AddLast(new Element("Sulphur", "S", 16, 32.066));
                    elements.AddLast(new Element("Chlorine", "Cl", 17, 35.4527));
                    elements.AddLast(new Element("Argon", "Ar", 18, 39.948));

                    elements.AddLast(new Element("Potassium", "K", 19, 39.093));
                    elements.AddLast(new Element("Calcium", "Ca", 20, 40.078));
                    elements.AddLast(new Element("Scandium", "Sc", 21, 44.95591));
                    elements.AddLast(new Element("Titanium", "Ti", 22, 47.867));
                    elements.AddLast(new Element("Vanadium", "V", 23, 50.9415));
                    elements.AddLast(new Element("Chromium", "Cr", 24, 51.9961));
                    elements.AddLast(new Element("Manganese", "Mn", 25, 54.93805));
                    elements.AddLast(new Element("Iron", "Fe", 26, 55.845));
                    elements.AddLast(new Element("Cobalt", "Co", 27, 58.93320));
                    elements.AddLast(new Element("Nickel", "Ni", 28, 58.6934));
                    elements.AddLast(new Element("Copper", "Cu", 29, 63.546));
                    elements.AddLast(new Element("Zinc", "Zn", 30, 65.39));
                    elements.AddLast(new Element("Gallium", "Ga", 31, 69.723));
                    elements.AddLast(new Element("Germanium", "Ge", 32, 72.61));
                    elements.AddLast(new Element("Arsenic", "As", 33, 74.92159));
                    elements.AddLast(new Element("Selenium", "Se", 34, 78.96));
                    elements.AddLast(new Element("Bromine", "Br", 35, 79.904));
                    elements.AddLast(new Element("Krypton", "Kr", 36, 83.80));

                    elements.AddLast(new Element("Rubidium", "Rb", 37, 85.4678));
                    elements.AddLast(new Element("Strontium", "Sr", 38, 87.62));
                    elements.AddLast(new Element("Yttrium", "Y", 39, 88.90585));
                    elements.AddLast(new Element("Zirconium", "Zr", 40, 91.224));
                    elements.AddLast(new Element("Niobium", "Nb", 41, 92.9064));
                    elements.AddLast(new Element("Molybdenium", "Mo", 42, 95.94));
                    elements.AddLast(new Element("Technetium", "Tc", 43, 98));
                    elements.AddLast(new Element("Ruthenium", "Ru", 44, 101.07));
                    elements.AddLast(new Element("Rhodium", "Rh", 45, 102.9055));
                    elements.AddLast(new Element("Palladium", "Pd", 46, 106.42));
                    elements.AddLast(new Element("Silver", "Ag", 47, 107.8682));
                    elements.AddLast(new Element("Cadmium", "Cd", 48, 112.411));
                    elements.AddLast(new Element("Indium", "In", 49, 114.818));
                    elements.AddLast(new Element("Tin", "Sn", 50, 118.710));
                    elements.AddLast(new Element("Antinomy", "Sb", 51, 121.760));
                    elements.AddLast(new Element("Tellurium", "Te", 52, 127.60));
                    elements.AddLast(new Element("Iodine", "I", 53, 126.90447));
                    elements.AddLast(new Element("Xenon", "Xe", 54, 118.710));

                    elements.AddLast(new Element("Caesium", "Cs", 55, 132.90543));
                    elements.AddLast(new Element("Barium", "Ba", 56, 137.327));
                    elements.AddLast(new Element("Lanthanum", "La", 57, 138.9055));
                    elements.AddLast(new Element("Cerium", "Ce", 58, 140.115));
                    elements.AddLast(new Element("Praseodymium", "Pr", 59, 140.90765));
                    elements.AddLast(new Element("Neodymium", "Nd", 60, 144.24));
                    elements.AddLast(new Element("Promethium", "Pm", 61, 145));
                    elements.AddLast(new Element("Samarium", "Sm", 62, 150.36));
                    elements.AddLast(new Element("Europium", "Eu", 63, 151.965));
                    elements.AddLast(new Element("Gadolinium", "Gd", 64, 157.25));
                    elements.AddLast(new Element("Terbium", "Tb", 65, 158.92534));
                    elements.AddLast(new Element("Dysprosium", "Dy", 66, 162.50));
                    elements.AddLast(new Element("Holmium", "Nd", 67, 164.93032));
                    elements.AddLast(new Element("Erbium", "Er", 68, 167.26));
                    elements.AddLast(new Element("Thulium", "Tm", 69, 168.93421));
                    elements.AddLast(new Element("Ytterbium", "Yb", 70, 173.04));
                    elements.AddLast(new Element("Lutetium", "Lu", 71, 174.967));
                    elements.AddLast(new Element("Hafnium", "Hf", 72, 178.49));
                    elements.AddLast(new Element("Tantalum", "Ta", 73, 180.9479));
                    elements.AddLast(new Element("Wolfram", "W", 74, 183.84));
                    elements.AddLast(new Element("Rhenium", "Re", 75, 186.207));
                    elements.AddLast(new Element("Osmium", "Os", 76, 190.23));
                    elements.AddLast(new Element("Iridium", "Ir", 77, 192.217));
                    elements.AddLast(new Element("Platinum", "Pt", 78, 195.08));
                    elements.AddLast(new Element("Gold", "Au", 79, 196.96654));
                    elements.AddLast(new Element("Mercury", "Hg", 80, 200.59));
                    elements.AddLast(new Element("Thallium", "Ti", 81, 204.3833));
                    elements.AddLast(new Element("Lead", "Pb", 82, 207.2));
                    elements.AddLast(new Element("Bismuth", "Bi", 83, 208.98037));
                    elements.AddLast(new Element("Polonium", "Po", 84, 209));
                    elements.AddLast(new Element("Astatine", "At", 85, 210));
                    elements.AddLast(new Element("Radon", "Rn", 86, 222));

                    elements.AddLast(new Element("Francium", "Fr", 87, 223));
                    elements.AddLast(new Element("Radium", "Ra", 88, 226.0254));
                    elements.AddLast(new Element("Actinium", "Ac", 89, 227));
                    elements.AddLast(new Element("Thorium", "Th", 90, 232.0381));
                    elements.AddLast(new Element("Protactium", "Pa", 91, 231));
                    elements.AddLast(new Element("Uranium", "U", 92, 238.0289));
                   /*elements.AddLast(new Element("Neptunium", "Np", 93, 237));
                    elements.AddLast(new Element("Plutonium", "Pu", 94, 244));
                    elements.AddLast(new Element("Americium", "Am", 95, 243));
                    elements.AddLast(new Element("Curium", "Cm", 96, 247));
                    elements.AddLast(new Element("Berkelium", "Bk", 97, 247));
                    elements.AddLast(new Element("Californium", "Cf", 98, 251));
                    elements.AddLast(new Element("Einsteinium", "Es", 99, 254));
                    elements.AddLast(new Element("Fermium", "Fm", 100, ));
                    elements.AddLast(new Element("Mendelevium", "Md", 101, ));
                    elements.AddLast(new Element("Nobelium", "No", 102, ));
                    elements.AddLast(new Element("Lawrencium", "Lr", 103, ));
                    elements.AddLast(new Element("Rutherfordium", "Rf", 104, ));
                    elements.AddLast(new Element("Dubnium", "Db", 105, ));
                    elements.AddLast(new Element("Seaborgium", "Sg", 106, ));
                    elements.AddLast(new Element("Bohrium", "Bh", 107, ));
                    elements.AddLast(new Element("Hassium", "Hs", 108, ));
                    elements.AddLast(new Element("Meitnerium", "Mt", 109, ));*/
                }
                return elements;
            }
        }

        public struct Element
        {
            public string Name { get; private set; }

            public string Symbol { get; private set; }

            public int AtomicNumber { get; private set; }

            public double AtomicMass { get; private set; }

            public Element(string name, string symbol, int atomicNumber, double atomicMass) : this()
            {
                Name = name;
                Symbol = symbol;
                AtomicNumber = atomicNumber;
                AtomicMass = atomicMass;

            }
        }

        public struct Molecule
        {
            public Dictionary<Element, int> Elements { get; private set; }

            public int Charge { get; private set; }

            public Molecule(Dictionary<Element, int> elements, int charge) : this()
            {
                Elements = elements;
                Charge = charge;
            }

            public Molecule(string formula) : this()
            {
                if (formula.Contains('+'))
                {
                    string[] parts = formula.Split('+');
                    Charge = parts[1] == "" ? 1 : int.Parse(parts[1]);
                    formula = parts[0];
                }
                if (formula.Contains('-'))
                {
                    string[] parts = formula.Split('-');
                    Charge = parts[1] == "" ? 1 : 0 - int.Parse(parts[1]);
                    formula = parts[0];
                }
                Elements = ElementsFromString(formula);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns>The total oxidation state of all non-oxygen and non-hydrogen elements.</returns>
            public int OxidationState()
            {
                int oxidation = Charge;
                oxidation -= Elements[Chemistry.Elements.Single(e => e.AtomicNumber == 1)];
                oxidation += Elements[Chemistry.Elements.Single(e => e.AtomicNumber == 8)] * 2;
                return oxidation;
            }
        }

       /* public struct Reaction
        {
            public Dictionary<Molecule, int> reactants { get; private set; }

            public Dictionary<Molecule, int> products { get; private set; }

            public Reaction(string reaction)
            {
                string[] parts = reaction.Split('=');

                string[] reactantFormulas = parts[0].Split(new string[] { " + " }, StringSplitOptions.None);
                string[] productFormulas = parts[1].Split(new string[] { " + " }, StringSplitOptions.None);

                reactants = new Dictionary<Molecule, int>();
                foreach (string reactant in reactantFormulas)
                {
                    string multiplier = "";
                    int index = 0;
                    for (; Regex.Matches("" + reactant[index], "[A-Z]").Count == 1; index++)
                        multiplier += reactant[index];
                    reactants[new Molecule(reactant.Substring(index))] = int.Parse(multiplier);
                }

                products = new Dictionary<Molecule, int>();
                foreach (string product in productFormulas)
                {
                    string multiplier = "";
                    int index = 0;
                    for (; Regex.Matches("" + product[index], "[A-Z]").Count == 1; index++)
                        multiplier += product[index];
                    products[new Molecule(product.Substring(index))] = int.Parse(multiplier);
                }
            }*/

            /*public string Redox(Acidity acidity)
            {
                int h = 0;
                int oh = 0;

                foreach(KeyValuePair<Molecule, int> reactant in reactants)
                {
                    if (reactant.Key.Elements[Elements.Single(e => e.AtomicNumber == 1)] == 1 && reactant.Key.Elements.Count() == 1 && reactant.Key.Charge == 1)
                        h += reactant.Value;
                    else if (reactant.Key.Elements[Elements.Single(e => e.AtomicNumber == 1)] == 1 && reactant.Key.Elements[Elements.Single(e => e.AtomicNumber == 8)] == 1 && reactant.Key.Elements.Count() == 2 && reactant.Key.Charge == -1)
                        oh += reactant.Value;
                }

                foreach (KeyValuePair<Molecule, int> product in products)
                {
                    if (product.Key.Elements[Elements.Single(e => e.AtomicNumber == 1)] == 1 && product.Key.Elements.Count() == 1 && product.Key.Charge == 1)
                        h -= product.Value;
                    else if (product.Key.Elements[Elements.Single(e => e.AtomicNumber == 1)] == 1 && product.Key.Elements[Elements.Single(e => e.AtomicNumber == 8)] == 1 && product.Key.Elements.Count() == 2 && product.Key.Charge == -1)
                        oh -= product.Value;
                }


            }*/
        //}

        public enum Acidity
        {
            Acidic, Basic, Neutral
        }

        public static double MolarMass(string s)
        {
            if (s.StartsWith("M(") && s.EndsWith(")"))
                s = s.Substring(2, s.Length - 3);

            Dictionary<Element, int> elements = ElementsFromString(s);

            double result = 0;
            foreach (KeyValuePair<Element, int> element in elements)
            {
                result += element.Key.AtomicMass * element.Value;
            }
            return result;
        }

        public static double MolarMass(Dictionary<Element, int> elements)
        {
            double result = 0;
            foreach (KeyValuePair<Element, int> element in elements)
            {
                result += element.Key.AtomicMass * element.Value;
            }
            return result;
        }

        public static double Mole(string formular, double grams)
        {
            return grams / MolarMass(formular);
        }

        public static double Mole(Dictionary<Element, int> elements, double grams)
        {
            return grams / MolarMass(elements);
        }

        public static Dictionary<Element, int> ElementsFromString(string s)
        {
            return ElementsFromString(s, new Dictionary<Element, int>(), 1);
        }

        /// <summary>
        /// Nested [] brackets do not work.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="elements"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        public static Dictionary<Element, int> ElementsFromString(string s, Dictionary<Element, int> elements, int multiplier = 1)
        {
            Dictionary<Element, int> result = new Dictionary<Element, int>();

            int startBracket = -1;
            int braces = 0;
            for (int index = 0; index < s.Length; index++ )
            {
                if (s[index] == '[')
                {
                    if (startBracket == -1)
                        startBracket = index;
                    braces++;
                }
                if (s[index] == ']')
                {
                    braces--;
                    if (braces < 0)
                        throw new Exception("Bracket at " + index + " missing start bracket.");
                    else if (braces == 0)
                    {
                        int bracketEnd = index;
                        string subMultiplier = "";
                        index++;
                        for (; index != s.Length && Regex.Matches("" + s[index], "[0-9]").Count == 1; index++ )
                            subMultiplier += s[index];
                        result = ElementsFromString(s.Substring(startBracket + 1, bracketEnd - (startBracket + 1)), result, int.Parse(subMultiplier));
                        index -= index - startBracket;
                        s = s.Substring(0, startBracket) + s.Substring(bracketEnd + subMultiplier.Length + 1, s.Length - (bracketEnd + subMultiplier.Length + 1));
                    }
                }
            }

            for (int index = 0; index < s.Length; )
            {
                string part = "" + s[index++];

                while (index < s.Length && Regex.Matches("" + s[index], "[A-Z]").Count < 1)
                {
                    part += s[index];
                    ++index;
                }

                string elementSymbol = "";
                int partIndex = 0;
                for (; partIndex != part.Length && Regex.Matches("" + part[partIndex], "[0-9]").Count < 1; partIndex++)
                    elementSymbol += part[partIndex];
                string amount = part.Substring(partIndex);

                Element element;
                try
                {
                    element = Elements.Single((Element e) => { return e.Symbol == elementSymbol; });
                }
                catch (Exception e)
                {
                    throw new Exception("Element " + elementSymbol + " does not exist.");
                }
                if (result.ContainsKey(element))
                    result[element] += amount == "" ? 1 : int.Parse(amount);
                else
                    result[element] = amount == "" ? 1 : int.Parse(amount);
            }
            foreach (KeyValuePair<Element, int> element in result)
            {
                if (elements.ContainsKey(element.Key))
                    elements[element.Key] += element.Value * multiplier;
                else
                    elements[element.Key] = element.Value * multiplier;
            }
            return elements;
        }

        
    }
}
