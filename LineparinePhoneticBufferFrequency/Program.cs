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
        static string FirstLetter(string word)
        {
            var letters = new List<string> {
                "fh", "vh", "dz", "ph", "ts", "ch", "ng", "sh", "th", "dh", "kh", "rkh", "rl",
                "i", "y", "u", "o", "e", "a",
                "p", "f", "t", "c", "x", "k", "q", "h", "r", "z", "m", "n", "r", "l", "j", "w", "b", "v", "d", "s", "g", };
            foreach (var letter in letters)
            {
                if (word.Replace("-", string.Empty).Replace("'", string.Empty).StartsWith(letter))
                {
                    return letter;
                }
            }
            return word;
        }

        static string LastLetter(string word)
        {
            var letters = new List<string> {
                "fh", "vh", "dz", "ph", "ts", "ch", "ng", "sh", "th", "dh", "kh", "rkh", "rl",
                "i", "y", "u", "o", "e", "a",
                "p", "f", "t", "c", "x", "k", "q", "h", "r", "z", "m", "n", "r", "l", "j", "w", "b", "v", "d", "s", "g", };
            foreach (var letter in letters)
            {
                if (word.Replace("-", string.Empty).Replace("'", string.Empty).EndsWith(letter))
                {
                    return letter;
                }
            }
            return word;
        }

        static int CountString(string text, string word) => (text.Length - text.Replace(word, string.Empty).Length) / word.Length;

        class BufferData
        {
            public int All { get; set; }
            public int Buffer { get; set; }
            public List<Example> Examples { get; set; }
        }

        class Example
        {
            public string Word { get; set; }
            public List<string> Decomposition { get; set; }
            public int FastLetterIndex { get; set; }
            public int LastLetterIndex { get; set; }
        }

        static void Main(string[] args)
        {
            var dictionary =
                from word in OneToManyJsonSerializer.Deserialize(File.ReadAllText(@"dictionary.json")).Words
                select word.Entry.Form;
            var table = new Dictionary<Tuple<string, string>, BufferData>();
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
                    var decomposition = decomposer.Decompose(word).FirstOrDefault() ?? new List<string>();
                    if (word == string.Join(string.Empty, decomposition).Replace("-", string.Empty))
                    {
                        var count = CountString(text, word);
                        if (decomposition.Count == 2)
                        {
                            var tuple = new Tuple<string, string>(LastLetter(decomposition[0]), FirstLetter(decomposition[1]));
                            if (table.ContainsKey(tuple))
                            {
                                table[tuple].All += count;
                                table[tuple].Examples.Add(
                                    new Example
                                    {
                                        Word = word,
                                        Decomposition = decomposition,
                                        FastLetterIndex = 0,
                                        LastLetterIndex = 1,
                                    });
                            }
                            else
                            {
                                table.Add(tuple, new BufferData
                                {
                                    All = count,
                                    Buffer = 0,
                                    Examples = new List<Example>
                                    {
                                        new Example {
                                            Word = word,
                                            Decomposition = decomposition,
                                            FastLetterIndex = 0,
                                            LastLetterIndex = 1,
                                        },
                                    }
                                });
                            }
                        }
                        else
                        {
                            for (int i = 1; i < decomposition.Count - 1; i++)
                            {
                                var buffer = new List<string> { "-a-", "-e-", "-i-", "-l-", "-m-", "-rg-", "-u-", "-v-", "eu-", "-eu", };
                                if (buffer.Contains(decomposition[i]))
                                {
                                    var tuple = new Tuple<string, string>(LastLetter(decomposition[i - 1]), FirstLetter(decomposition[i + 1]));
                                    if (table.ContainsKey(tuple))
                                    {
                                        table[tuple].All += count;
                                        table[tuple].Buffer += count;
                                        table[tuple].Examples.Add(
                                            new Example
                                            {
                                                Word = word,
                                                Decomposition = decomposition,
                                                FastLetterIndex = i - 1,
                                                LastLetterIndex = i + 1,
                                            });
                                    }
                                    else
                                    {
                                        table.Add(tuple, new BufferData
                                        {
                                            All = count,
                                            Buffer = count,
                                            Examples = new List<Example>
                                            {
                                                new Example {
                                                    Word = word,
                                                    Decomposition = decomposition,
                                                    FastLetterIndex = i - 1,
                                                    LastLetterIndex = i + 1,
                                               },
                                            },
                                        });
                                    }
                                }
                                else
                                {
                                    var tuple = new Tuple<string, string>(LastLetter(decomposition[i - 1]), FirstLetter(decomposition[i]));
                                    if (table.ContainsKey(tuple))
                                    {
                                        table[tuple].All += count;
                                        table[tuple].Examples.Add(
                                            new Example
                                            {
                                                Word = word,
                                                Decomposition = decomposition,
                                                FastLetterIndex = i - 1,
                                                LastLetterIndex = i,
                                            });
                                    }
                                    else
                                    {
                                        table.Add(tuple, new BufferData
                                        {
                                            All = count,
                                            Buffer = 0,
                                            Examples = new List<Example>
                                            {
                                                new Example {
                                                    Word = word,
                                                    Decomposition = decomposition,
                                                    FastLetterIndex = i - 1,
                                                    LastLetterIndex = i,
                                                },
                                            },
                                        });
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        decomposition.Add("(ERROR)");
                    }
                    Console.WriteLine(word + " => " + string.Join(" ", decomposition));
                    sw.WriteLine(word + "\t" + string.Join(" ", decomposition));
                }
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
