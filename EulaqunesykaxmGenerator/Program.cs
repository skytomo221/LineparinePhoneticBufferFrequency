using Otamajakushi;
using System;
using System.IO;
using System.Linq;

namespace EulaqunesykaxmGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var dictionary =
                from word in OneToManyJsonSerializer.Deserialize(File.ReadAllText(@"dictionary.json")).Words
                select word.Entry.Form;
            using (StreamReader sr = new StreamReader("input.txt"))
            using (StreamWriter swBuffer = new StreamWriter("buffer.tsv"))
            using (StreamWriter swAll = new StreamWriter("all.tsv"))
            using (StreamWriter swExample = new StreamWriter("example.tsv"))
            {
            }
        }
    }
}
