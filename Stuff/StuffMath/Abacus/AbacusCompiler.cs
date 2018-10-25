using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stuff;
using System.Xml.Linq;
using System.IO;

namespace Stuff.StuffMath.Abacus
{
    public class AbacusCompiler
    {
        private readonly Dictionary<string, AbacusMachine> functions;

        public IReadOnlyDictionary<string, AbacusMachine> Functions => functions;

        public AbacusCompiler()
        {
            functions = new Dictionary<string, AbacusMachine>();
        }

        private class Entry
        {
            public string Name { get; }

            public IReadOnlyList<int> Registers { get; }

            public string Dest { get; }

            public string FailDest { get; }

            public int Number { get; }

            public string Label { get; }

            public Entry(string code, int num)
            {
                var args = code.Split(':');
                Name = args[0];
                Registers = args[1].Split(',').Select(s => int.Parse(s)).ToList();
                var dests = args[2].Split(',');
                Dest = dests[0];
                FailDest = dests.Length > 1 ? dests[1] : "-1";
                if (args.Length > 3)
                    Label = args[3];
                Number = num;
            }
        }

        public AbacusMachine AddFunction(string name, AbacusMachine am)
        {
            functions.Add(name, am);
            return functions[name];
        }

        public AbacusMachine AddFunction(string name, string code)
        {
            functions.Add(name, Compile(code));
            return functions[name];
        }

        public void LoadFunctions(string path)
        {
            IEnumerable<string> files;
            if (path.EndsWith(".xml"))
                files = new List<string> { path };
            else if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                files = Directory.EnumerateFiles(path).Where(f => f.EndsWith(".xml"));
            else
                throw new ArgumentException("Path must either lead to an xml file or a directory.");
            foreach (var file in files)
            {
                var element = XDocument.Load(file).Element("functions");
                foreach (var f in element.Elements())
                    AddFunction(f.ElementValue("name"), Compile(f.ElementValue("code"), f.ElementValue("name")));
            }
        }

        private IEnumerable<Entry> ToEntries(string code, string functionName)
        {
            int i = 0;
            try
            {
                return code.Trim(';').Split(';').Select(c => new Entry(c, i++));
            }
            catch (Exception e)
            {
                throw new EntryParseException(functionName, i, e);
            }
        }

        private AbacusMachine CompileEntries(IEnumerable<Entry> entries)
        {
            var result = new List<List<Node>>();

            var startNodes = new List<int>();

            int curNode = 1;
            foreach (var e in entries)
            {
                startNodes.Add(curNode);
                curNode += AbacusLength(e.Name);
            }

            int curNum = 1;

            foreach (var e in entries)
            {
                if (e.Name == "+")
                {
                    result.Add(new List<Node> { new Node(e.Registers[0], EntryNumToNode(e.Dest, entries, startNodes)) });
                    ++curNum;
                }
                else if (e.Name == "-")
                {
                    result.Add(new List<Node> { new Node(e.Registers[0], EntryNumToNode(e.Dest, entries, startNodes), EntryNumToNode(e.FailDest, entries, startNodes)) });
                    ++curNum;
                }
                else
                {
                    if (functions.ContainsKey(e.Name))
                        result.Add(functions[e.Name].Translate(curNum, EntryNumToNode(e.Dest, entries, startNodes), e.Registers.ToArray()));
                    else
                        throw new ArgumentException("No function exists with name: " + e.Name);
                    curNum += functions[e.Name].Nodes;
                }
            }
            return new AbacusMachine(result.ToArray());
        }

        private int EntryNumToIndex(string dest, IEnumerable<Entry> entries)
        {
            if (dest == "")
                throw new ArgumentException("A entry's destination may not be empty");
            if (int.TryParse(dest, out int index))
            {
                if (index > entries.Count())
                    throw new ArgumentException("The destination index of an instruction must be less than or equal to the total number of entries.");
                return index;
            }
            else if (entries.Contains(e => e.Label == dest))
                return entries.FirstIndexOf(e => e.Label == dest);
            else
                throw new ArgumentException("No entry exists with label: " + dest);
        }

        private int EntryNumToNode(string dest, IEnumerable<Entry> entries, List<int> startNodes)
        {
            int index = EntryNumToIndex(dest, entries);
            return index > 0 ? startNodes[index-1] : index;
        }

        private int AbacusLength(string abacusName)
        {
            if (abacusName == "+" || abacusName == "-")
                return 1;
            else if (functions.ContainsKey(abacusName))
                return functions[abacusName].Nodes;
            else
                throw new ArgumentException("No function exists with name: " + abacusName);
        }

        public AbacusMachine Compile(string code, string functionName = "")
        {
            return CompileEntries(ToEntries(code.RemoveWhiteSpace(), functionName));
        }
    }
}
