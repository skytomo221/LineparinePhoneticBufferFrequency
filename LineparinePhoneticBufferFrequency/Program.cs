using Otamajakushi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LineparinePhoneticBufferFrequency
{
    class Program
    {
        static char FirstLetter(string word) => word.Replace("'", string.Empty)[0];
        static char LastLetter(string word) => word.Replace("'", string.Empty).Reverse().First();
        static int CountString(string text, string word) => (text.Length - text.Replace(word, string.Empty).Length) / word.Length;

        static void Main(string[] args)
        {
            var dictionary =
                from word in OneToManyJsonSerializer.Deserialize(File.ReadAllText(@"dictionary.json")).Words
                select word.Entry.Form;
            var allTable = new Dictionary<Tuple<char, char>, int>();
            var bufferTable = new Dictionary<Tuple<char, char>, int>();
            var exampleTable = new Dictionary<Tuple<char, char>, List<string>>();
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
                dictionary =
                    from word in OneToManyJsonSerializer.Deserialize(File.ReadAllText(@"dictionary.json")).Words
                    select word.Entry.Form.Replace("-", string.Empty);
                foreach (var word in words)
                {
                    int start = 0;
                    int length = word.Length;
                    var subword = string.Empty;
                    var subwords = new List<string>();
                    while (length - start >= 0)
                    {
                        subword = word.Substring(start, length - start);
                        if (dictionary.Contains(subword))
                        {
                            subwords.Add(subword);
                            start += subword.Length;
                            length = word.Length;
                        }
                        else
                        {
                            length--;
                        }
                    }
                    if (word == string.Join(string.Empty, subwords))
                    {
                        var buffer = new List<string> { "v", "rg", "m", "l", "eu", "a", "e", "u", "i" };
                        if (subwords.Count == 2)
                        {
                            var count = CountString(text, word);
                            var tuple = new Tuple<char, char>(LastLetter(subwords[0]), FirstLetter(subwords[1]));
                            if (allTable.ContainsKey(tuple))
                            {
                                allTable[tuple] += count;
                                exampleTable[tuple].Add(word + "\t" + string.Join(" ", subwords) + "\t" + count);
                            }
                            else
                            {
                                allTable.Add(tuple, count);
                                exampleTable.Add(tuple, new List<string> { word + "\t" + string.Join(" ", subwords) + "\t" + count });
                            }
                        }
                        else
                        {
                            for (int i = 1; i < subwords.Count - 1; i++)
                            {
                                if (buffer.Contains(subwords[i]))
                                {
                                    var count = CountString(text, word);
                                    var tuple = new Tuple<char, char>(LastLetter(subwords[i - 1]), FirstLetter(subwords[i + 1]));
                                    if (bufferTable.ContainsKey(tuple))
                                    {
                                        bufferTable[tuple] += count;
                                    }
                                    else
                                    {
                                        bufferTable.Add(tuple, count);

                                    }
                                    if (allTable.ContainsKey(tuple))
                                    {
                                        allTable[tuple] += count;
                                        exampleTable[tuple].Add(word + "\t" + string.Join(" ", subwords) + "\t" + count);
                                    }
                                    else
                                    {
                                        allTable.Add(tuple, count);
                                        exampleTable.Add(tuple, new List<string> { word + "\t" + string.Join(" ", subwords) + "\t" + count });
                                    }
                                }
                                else
                                {
                                    var count = CountString(text, word);
                                    var tuple = new Tuple<char, char>(LastLetter(subwords[i - 1]), FirstLetter(subwords[i]));
                                    if (allTable.ContainsKey(tuple))
                                    {
                                        allTable[tuple] += count;
                                        exampleTable[tuple].Add(word + "\t" + string.Join(" ", subwords) + "\t" + count);
                                    }
                                    else
                                    {
                                        allTable.Add(tuple, count);
                                        exampleTable.Add(tuple, new List<string> { word + "\t" + string.Join(" ", subwords) + "\t" + count });
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
                        if (bufferTable.ContainsKey(tuple))
                        {
                            swBuffer.Write(bufferTable[tuple] + "\t");
                        }
                        else
                        {
                            swBuffer.Write("0\t");
                        }
                        if (allTable.ContainsKey(tuple))
                        {
                            swAll.Write(allTable[tuple] + "\t");
                            swExample.WriteLine($"{first.ToString()} + {last.ToString()}\n{exampleTable[tuple].Aggregate((now, next) => now + "\n" + next)}");
                            swExample.WriteLine();
                        }
                        else
                        {
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
