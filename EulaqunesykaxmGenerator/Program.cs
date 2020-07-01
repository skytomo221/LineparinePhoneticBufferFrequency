using LineparinePhoneticBufferFrequency;
using Otamajakushi;
using System;
using System.Collections.Generic;
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
            using (StreamReader srData = new StreamReader("../../../Data.tsx"))
            using (StreamWriter sw = new StreamWriter("output.tsx"))
            {
                string data = srData.ReadToEnd();
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
                    "n", "l", "j", "w", "b",
                    "vh", "v", "d", "s", "g", "dz",
                    "ph", "ts", "ch", "ng", "sh",
                    "th", "dh", "kh", "rkh", "rl",
                };
                sw.WriteLine(data.Trim());
                sw.WriteLine("export const rates : {[key: string]: number | null} = {");
                foreach (var left in alphabet)
                {
                    foreach (var right in alphabet)
                    {
                        var tuple = new Tuple<string, string>(left, right);
                        if (table.ContainsKey(tuple))
                        {
                            var rate = ((double)(table[tuple].Buffer + 1) / (table[tuple].All + 2));
                            sw.WriteLine($"['{left}+{right}'] : {((table[tuple].All == 0) ? "null" : rate.ToString())},");
                        }
                        else
                        {
                            sw.WriteLine($"['{left}+{right}'] : null,");
                        }
                    }
                }
                sw.WriteLine("}");
                sw.WriteLine("export const examples : { [key: string]: Array<Example> } = {");
                foreach (var left in alphabet)
                {
                    foreach (var right in alphabet)
                    {
                        var tuple = new Tuple<string, string>(left, right);
                        if (table.ContainsKey(tuple))
                        {
                            var example = table[tuple].Examples
                                .Select(w => $"{{\nword : \"{w.Word}\",\nparts : [{string.Join(", ", w.Decomposition.Select(d => $"\"{d}\""))}],\nleftIndex : {w.FastLetterIndex},\nrightIndex : {w.LastLetterIndex}\n}}")
                                .Aggregate((now, next) => $"{now},\n{next}");
                            sw.WriteLine($"['{left}+{right}'] : [\n{example}\n],");
                        }
                    }
                }
                sw.WriteLine("}");
            }
        }
    }
}
