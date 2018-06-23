using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Stuff;

namespace Stuff
{
    public class SettingsLoader
    {
        public static SettingsManager LoadSettings(string path = "Assets/Settings", bool recursive = true)
        {
            SettingsManager result = new SettingsManager();
            try
            {
                foreach (string f in (from filePath in Directory.EnumerateFiles(path, "*.txt", SearchOption.AllDirectories) where 
                                          !filePath.Substring(filePath.LastIndexOf('/')).StartsWith("!") select filePath))
                {
                    StreamReader file = new StreamReader(f);
                    string category = f.Substring(f.LastIndexOf("\\") + 1, f.Length - 5 - f.LastIndexOf("\\"));
                    result.AddCategory(category);
                    while (!file.EndOfStream)
                    {
                        string line = file.ReadLine();
                        if (line != "")
                        {
                            if (line.Contains("//"))
                               line = line.Substring(0, line.IndexOf("//") + 1);
                            string[] args = line.Split(' ');
                            string key = args[0];
                            string[] values = args[1].Split(',');
                            result.AddSetting(category, key, values);
                        }
                    }
                    file.Close();
                }
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Console.WriteLine(PathEx.Message);
            }
            catch (DirectoryNotFoundException dnfe)
            {
                Console.WriteLine(dnfe.Message);
            }
            return result;
        }
   
        public static void SaveSettings(SettingsManager settings, string path = "Assets/Settings")
        {
            LinkedList<string> files = new LinkedList<string>();
            if (Directory.Exists(path))
            {
                foreach (string f in (from filePath in Directory.EnumerateFiles(path, "*.txt", SearchOption.AllDirectories)
                                      where
                                          !filePath.Substring(filePath.LastIndexOf('/')).Contains('!')
                                      select filePath))
                    files.AddLast(f);
            }
            foreach (string category in settings.Categories())
            {
                string file; // Find the file for the category if it exists, or else create a new one.
                if (files.Count((string s) => {return s.EndsWith("/" + category + ".txt");}) != 0)
                    file = files.Single((string s) => { return s.EndsWith("/" + category + ".txt"); });
                else
                {
                    using (File.Create(path + "/" + category + ".txt")){}
                    file = path + "/" + category + ".txt";
                }
                //Create an IEnumerable with the lines.
                LinkedList<string> lines = new LinkedList<string>();
                foreach (KeyValuePair<string, string[]> setting in settings.SettingsInCategory(category))
                {
                    string line = setting.Key + " ";
                    foreach (string value in setting.Value)
                        line += value + " ";
                    lines.AddLast(line.Trim());
                }
                //Actually save the category.
                File.WriteAllLines(file, lines);
            }
        }
    }
}
