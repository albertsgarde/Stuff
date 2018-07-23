using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stuff
{
    public class PathList : IEnumerable<string>
    {
        public string Root { get; }

        private List<string> paths;

        /// <summary>
        /// The First path is taken as the root.
        /// </summary>
        /// <param name="paths"></param>
        public PathList(IEnumerable<string> paths, string superRoot = "")
        {
            Root = Path.IsPathRooted(paths.First()) ? paths.First() : Path.Combine(superRoot, paths.First());
            if (!File.GetAttributes(Root).HasFlag(FileAttributes.Directory))
                throw new DirectoryNotFoundException();
            this.paths = new List<string>();
            this.paths.Add(Root);
            foreach (var path in (from p in paths where p != paths.First() select FilePath(p)))
            {
                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory) && !path.EndsWith("/"))
                    Add(path + "/");
                else
                    Add(path);
            }
        }

        public PathList(params string[] paths) : this(paths.AsEnumerable()) { }

        public void Add(string path)
        {
            if (Path.IsPathRooted(path))
                paths.Add(path);
            else
                paths.Add(Path.Combine(Root, path));
        }

        public void Remove(string path)
        {
            paths.Remove(path);
        }

        public string FirstFile()
        {
            return Files().First();
        }

        public string FilePath(string path)
        {
            return Path.IsPathRooted(path) ? path : Path.Combine(Root, path);
        }

        public int DirCount()
        {
            return paths.Count(path => File.GetAttributes(path).HasFlag(FileAttributes.Directory));
        }

        public IEnumerable<string> Files()
        {
            foreach (var path in paths)
            {
                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                {
                    foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                        yield return file;
                }
                else
                    yield return path;
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return paths.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
