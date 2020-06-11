using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LineparineWordsFrequency
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader sr = new StreamReader("input.txt"))
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                sw.Write(
                    Regex.Split(sr.ReadToEnd().ToLower(), @"\s")
                    .Where(word => Regex.IsMatch(word, @"^[\x20-\x7F]+$"))
                    .Select(word => Regex.Replace(word, @"[\x21-\x26\x28-\x2f\x3a-\x40\x5b-\x60\x7b-\x7e]", string.Empty))
                    .Where(word => !string.IsNullOrEmpty(word))
                    .OrderBy(word => word)
                    .Aggregate((now, next) => now + "\n" + next));
            }
        }
    }
}
