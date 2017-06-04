using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// ReSharper disable InconsistentNaming
namespace WindowsAutomatedSimplifier.FileSystem
{
    internal class PatternEvaluator : IDisposable
    {
        public string Pattern { get; private set; }
        private FileInfo CurFile { get; set; }
        private int counter;
        private int numPadSize;
        private List<Func<string>> actions;

        public PatternEvaluator(string pattern)
        {
            Pattern = pattern;
            counter = 0;
            numPadSize = 0;
            actions = new List<Func<string>>();
            CreateActions(Pattern);
        }

        private void CreateActions(string pattern)
        {
            bool addCountAfter = pattern.IndexOf("{") < 0;
            var parts = new List<string>();

            string curr = "";
            foreach (char c in pattern)
            {
                switch (c)
                {
                    case '}':
                    case '>':
                        parts.Add(curr + c);
                        curr = "";
                        break;
                    case '{':
                    case '<':
                        if (curr.Length > 0) parts.Add(curr);
                        curr = c.ToString();
                        break;
                    default:
                        curr += c;
                        break;
                }
            }
            if (addCountAfter) parts.Add("{}");

            foreach (string part in parts)
            {
                if (part.StartsWith("<"))
                    actions.Add(EvaluateAngleBrackets(part));
                else if (part.StartsWith("{"))
                    actions.Add(EvaluateBraces(part));
                else actions.Add(() => part);
            }
        }

        private Func<string> EvaluateBraces(string tag)
        {
            tag = tag.Trim('{', '}', ' ');
            numPadSize = tag.Length;
            if (numPadSize == 0) tag = "0";

            if (!int.TryParse(tag, out counter)) return () => "";  //TODO return better exception value
            counter -= 1;
            return () => counter++.ToString().PadLeft(numPadSize, '0');
        }

        private Func<string> EvaluateAngleBrackets(string tag)
        {
            switch (tag.Trim('<', '>', ' ').ToLower())
            {
                case "name": return () => CurFile.Name;
                case "lcd":
                case "longcreationdate": return () => CurFile.CreationTime.ToLongDateString();
                case "scd":
                case "shortcreationdate": return () => CurFile.CreationTime.ToShortDateString();
                case "lct":
                case "longcreationtime": return () => CurFile.CreationTime.ToLongTimeString();
                case "sct":
                case "shortcreationtime": return () => CurFile.CreationTime.ToShortTimeString();
                case "led":
                case "longeditdate": return () => CurFile.LastWriteTime.ToLongDateString();
                case "sed":
                case "shorteditdate": return () => CurFile.LastWriteTime.ToShortDateString();
                case "let":
                case "longedittime": return () => CurFile.LastWriteTime.ToLongTimeString();
                case "set":
                case "shortedittime": return () => CurFile.LastWriteTime.ToShortTimeString();
                case "size": return () => CurFile.Length.ToString();
                default: return () => "";  //TODO return better exception value
            }
        }

        public string ForFile(string filename)
        {
            CurFile = new FileInfo(filename);
            return actions.Aggregate("", (current, func) => current + func.Invoke());
        }

        public void Dispose()
        {
            Pattern = null;
            CurFile = null;
            counter = 0;
            numPadSize = 0;
            actions = null;
        }
    }
}
