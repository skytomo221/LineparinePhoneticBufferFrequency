using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineparinePhoneticBufferFrequency
{
    public class LineparinePhoneticBufferFrequencyAnalyzer
    {
        public string Text { get; set; }
        public IEnumerable<string> Words { get; set; }

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

        public Dictionary<Tuple<string, string>, BufferData> Analyze()
        {
            var table = new Dictionary<Tuple<string, string>, BufferData>();           
            var decomposer = new LineparineDecomposer.LineparineDecomposer();
            foreach (var word in Words)
            {
                var decomposition = decomposer.Decompose(word).FirstOrDefault() ?? new List<string>();
                if (word == string.Join(string.Empty, decomposition).Replace("-", string.Empty))
                {
                    var count = CountString(Text, word);
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
            }
            return table;
        }
    }
}
