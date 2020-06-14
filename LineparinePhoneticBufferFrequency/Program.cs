using Otamajakushi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LineparineDecomposer;

namespace LineparinePhoneticBufferFrequency
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
                var text = sr.ReadToEnd();
                var words =
                   from word in text.Split('\n')
                   .Distinct()
                   where !dictionary.Contains(word)
                   select word;
                var analyzer = new LineparinePhoneticBufferFrequencyAnalyzer
                {
                    Text = text,
                    Words = words,
                };
                var table = analyzer.Analyze();
                var alphabet = new List<string> {
                    "i", "y", "u", "o", "e", "a",
                    "p", "fh", "f", "t", "c", "x",
                    "k", "q", "h", "r", "z", "m",
                    "n", "r", "l", "j", "w", "b",
                    "vh", "v", "d", "s", "g", "dz",
                    "ph", "ts", "ch", "ng", "sh",
                    "th", "dh", "kh", "rkh", "rl",
                };
                foreach (var first in alphabet)
                {
                    foreach (var last in alphabet)
                    {
                        var tuple = new Tuple<string, string>(first, last);
                        if (table.ContainsKey(tuple))
                        {
                            var example = table[tuple].Examples
                                .Select(w => $"{w.Word}\t{string.Join(" ", w.Decomposition)}\t{w.FastLetterIndex}\t{w.LastLetterIndex}")
                                .Aggregate((now, next) => $"{now}\n{next}");
                            swAll.Write(table[tuple].All + "\t");
                            swBuffer.Write(table[tuple].Buffer + "\t");
                            swExample.WriteLine($"{first.ToString()} + {last.ToString()}\n{example}");
                            swExample.WriteLine();
                        }
                        else
                        {
                            swBuffer.Write("0\t");
                            swAll.Write("0\t");
                        }
                    }
                    swBuffer.Write("\n");
                    swAll.Write("\n");
                }
            }
        }
    }
}
