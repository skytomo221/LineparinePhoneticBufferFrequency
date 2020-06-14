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
        static char FirstLetter(string word) => word.Replace("-", string.Empty).Replace("'", string.Empty)[0];
        static char LastLetter(string word) => word.Replace("-", string.Empty).Replace("'", string.Empty).Reverse().First();
        static int CountString(string text, string word) => (text.Length - text.Replace(word, string.Empty).Length) / word.Length;

        class BufferData
        {
            public int All { get; set; }
            public int Buffer { get; set; }
            public List<Tuple<string, List<string>>> Example { get; set; }
        }

        static void Main(string[] args)
        {
            var dictionary =
                from word in OneToManyJsonSerializer.Deserialize(File.ReadAllText(@"dictionary.json")).Words
                select word.Entry.Form;
            var table = new Dictionary<Tuple<char, char>, BufferData>();
            using (StreamReader sr = new StreamReader("input.txt"))
            using (StreamWriter sw = new StreamWriter("output.tsv"))
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
                var decomposer = new LineparineDecomposer.LineparineDecomposer();
                foreach (var word in words)
                {
                    var subwords = decomposer.Decompose(word).FirstOrDefault() ?? new List<string>();
                    if (word == string.Join(string.Empty, subwords).Replace("-", string.Empty))
                    {
                        var count = CountString(text, word);
                        if (subwords.Count == 2)
                        {
                            var tuple = new Tuple<char, char>(LastLetter(subwords[0]), FirstLetter(subwords[1]));
                            if (table.ContainsKey(tuple))
                            {
                                table[tuple].All += count;
                                table[tuple].Example.Add(new Tuple<string, List<string>>(word, subwords));
                            }
                            else
                            {
                                table.Add(tuple, new BufferData
                                {
                                    All = count,
                                    Buffer = 0,
                                    Example = new List<Tuple<string, List<string>>>
                                    {
                                        new Tuple<string, List<string>> (word, subwords),
                                    }
                                });
                            }
                        }
                        else
                        {
                            for (int i = 1; i < subwords.Count - 1; i++)
                            {
                                var tuple = new Tuple<char, char>(LastLetter(subwords[i - 1]), FirstLetter(subwords[i + 1]));
                                var buffer = new List<string> { "-a-", "-e-", "-i-", "-l-", "-m-", "-rg-", "-u-", "-v-", "eu-", "-eu", };
                                if (buffer.Contains(subwords[i]))
                                {
                                    if (table.ContainsKey(tuple))
                                    {
                                        table[tuple].All += count;
                                        table[tuple].Buffer += count;
                                        table[tuple].Example.Add(new Tuple<string, List<string>>(word, subwords));
                                    }
                                    else
                                    {
                                        table.Add(tuple, new BufferData
                                        {
                                            All = count,
                                            Buffer = count,
                                            Example = new List<Tuple<string, List<string>>>
                                            {
                                                new Tuple<string, List<string>> (word, subwords),
                                            }
                                        });
                                    }
                                }
                                else
                                {
                                    if (table.ContainsKey(tuple))
                                    {
                                        table[tuple].All += count;
                                        table[tuple].Example.Add(new Tuple<string, List<string>>(word, subwords));
                                    }
                                    else
                                    {
                                        table.Add(tuple, new BufferData
                                        {
                                            All = count,
                                            Buffer = 0,
                                            Example = new List<Tuple<string, List<string>>>
                                            {
                                                new Tuple<string, List<string>> (word, subwords),
                                            }
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        subwords.Add("(ERROR)");
                    }
                    Console.WriteLine(word + " => " + string.Join(" ", subwords));
                    sw.WriteLine(word + "\t" + string.Join(" ", subwords));
                }
                var alphabet = new List<char> { 'i', 'y', 'u', 'o', 'e', 'a', 'p', 'f', 't', 'c', 'x', 'k', 'q', 'h', 'r', 'z', 'm', 'n', 'r', 'l', 'j', 'w', 'b', 'v', 'd', 's', 'g', };
                foreach (var first in alphabet)
                {
                    foreach (var last in alphabet)
                    {
                        var tuple = new Tuple<char, char>(first, last);
                        if (table.ContainsKey(tuple))
                        {
                            var example = table[tuple].Example
                                .Select(w => $"{w.Item1}\t{string.Join(" ", w.Item2)}")
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
