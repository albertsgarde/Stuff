using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Stuff
{
    public class Logger
    {
        private readonly string logDirPath;

        private readonly List<string> logs;

        public Logger(string logDirPath)
        {
            this.logDirPath = logDirPath;
            Directory.CreateDirectory(logDirPath);
            logs = new List<string>();
        }

        private StreamWriter GetLog(string name)
        {
            if (logs.Contains(name))
                return new StreamWriter(File.Open(logDirPath + "/" + name + ".txt", FileMode.Append));
            else
            {
                logs.Add(name);
                return new StreamWriter(File.Open(logDirPath + "/" + name + ".txt", FileMode.Create));
            }
        }

        public void Log(string log, string message, bool timestamp = true, bool newline = true)
        {
            using (var writer = GetLog(log))
                writer.Write((timestamp ? TimeStamp() : "") + message + (newline ? Environment.NewLine : ""));
        }

        public void Log(string log, Exception e, bool timestamp = true)
        {
            string message = (timestamp ? TimeStamp() : "") + e.ToString().Indent("  ") + Environment.NewLine + e.StackTrace.Indent("  ");
            Log(log, message, timestamp, true);
        }

        private string TimeStamp()
        {
            return "[" + DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") + "]";
        }
    }
}
