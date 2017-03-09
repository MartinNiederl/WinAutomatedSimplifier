using System.Collections.Generic;

namespace WindowsAutomatedSimplifier.DeCompress
{
    internal class ListManagement
    {
        private static readonly NamedList Zip = new NamedList("Zip", new List<string> { "None", "Deflate", "BZip2", "LZMA", "LZMA2", "PPMd" }),
            Tar = new NamedList("Tar", new List<string> { "None", "BZip", "GZip" }),
            GZip = new NamedList("GZip", new List<string> { "GZip" }),
            Rar = new NamedList("Rar", new List<string> { "Rar" }),
            SevenZip = new NamedList("SevenZip", new List<string> { "LZMA", "LZMA2", "BZip2", "PPMd", "BCJ", "BCJ" });

        public static List<NamedList> Compress { get; } = new List<NamedList> { Zip, Tar, GZip };

        public static List<NamedList> Decompress { get; } = new List<NamedList> { Zip, Tar, GZip, Rar, SevenZip };
    }

    public class NamedList
    {
        public string Name { get; set; }
        public List<string> List { get; set; }

        public NamedList(string name, List<string> list)
        {
            Name = name;
            List = list;
        }

        public override string ToString() => Name;
    }
}
