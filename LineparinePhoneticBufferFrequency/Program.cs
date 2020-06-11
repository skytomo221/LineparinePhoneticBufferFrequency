using Otamajakushi;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                sw.Write(
                    sr
                    .ReadToEnd()
                    .Split('\n')
                    .Distinct()
                    .Where(word => !dictionary.Contains(word))
                    .Where(word => !(Regex.IsMatch(word, @"^.+'s$") && dictionary.Contains(word.Replace("'s", string.Empty))))
                    .Where(word => !(Regex.IsMatch(word, @"^.+'i$") && dictionary.Contains(word.Replace("'i", string.Empty))))
                    .Where(word => !(Regex.IsMatch(word, @"^.+'c$") && dictionary.Contains(word.Replace("'c", string.Empty))))
                    .Where(word => !(Regex.IsMatch(word, @"^.+'d$") && dictionary.Contains(word.Replace("'d", string.Empty))))
                    .Where(word => !(Regex.IsMatch(word, @"^.+'dy$") && dictionary.Contains(word.Replace("'dy", string.Empty))))
                    .Where(word => !(Regex.IsMatch(word, @"^.+'l$") && dictionary.Contains(word.Replace("'l", string.Empty))))
                    .Where(word => !(Regex.IsMatch(word, @"^.+sti$") && dictionary.Contains(word.Replace("sti", string.Empty))))
                    .Where(word => !(Regex.IsMatch(word, @"^.+'tj$") && dictionary.Contains(word.Replace("'tj", string.Empty))))
                    .Where(word => !(Regex.IsMatch(word, @"^.+'sci$") && dictionary.Contains(word.Replace("'sci", string.Empty))))
                    .Where(word => !Regex.IsMatch(word, @"^\d+$"))
                    .Aggregate((now, next) => now + "\n" + next));
            }
            using (StreamReader sr = new StreamReader("input.txt"))
            using (StreamWriter sw = new StreamWriter("output2.txt"))
            {
                sw.Write(
                    sr
                    .ReadToEnd()
                    .Split('\n')
                    .Distinct()
                    .Where(word => !dictionary.Contains(word))
                    .Where(word => (Regex.IsMatch(word, @"^.+[^aeiouyr]'s$") && dictionary.Contains(word.Replace("'s", string.Empty))))
                    .Where(word => (Regex.IsMatch(word, @"^.+[^aeiouyr]'i$") && dictionary.Contains(word.Replace("'i", string.Empty))))
                    .Where(word => (Regex.IsMatch(word, @"^.+[^aeiouyr]'c$") && dictionary.Contains(word.Replace("'c", string.Empty))))
                    .Where(word => (Regex.IsMatch(word, @"^.+[^aeiouyr]'d$") && dictionary.Contains(word.Replace("'d", string.Empty))))
                    .Where(word => (Regex.IsMatch(word, @"^.+[^aeiouyr]'dy$") && dictionary.Contains(word.Replace("'dy", string.Empty))))
                    .Where(word => (Regex.IsMatch(word, @"^.+[^aeiouyr]'l$") && dictionary.Contains(word.Replace("'l", string.Empty))))
                    .Where(word => (Regex.IsMatch(word, @"^.+[^aeiouyr]sti$") && dictionary.Contains(word.Replace("sti", string.Empty))))
                    .Where(word => (Regex.IsMatch(word, @"^.+[^aeiouyr]'tj$") && dictionary.Contains(word.Replace("'tj", string.Empty))))
                    .Where(word => (Regex.IsMatch(word, @"^.+[^aeiouyr]'sci$") && dictionary.Contains(word.Replace("'sci", string.Empty))))
                    .Where(word => !Regex.IsMatch(word, @"^\d+$")).Count());
//                    .Aggregate((now, next) => now + "\n" + next));
            }
        }
    }
}
