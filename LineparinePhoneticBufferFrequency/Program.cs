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
        static void Main(string[] args)
        {
            var dictionary = 
                from word in OneToManyJsonSerializer.Deserialize(File.ReadAllText(@"dictionary.json")).Words
                select word.Entry.Form;
            using (StreamReader sr = new StreamReader("input.txt"))
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                var words =
                   from word in sr.ReadToEnd().Split('\n')
                   .Distinct()
                   where !dictionary.Contains(word)
                   select word;
                foreach (var word in words)
                {
                    var subword = string.Empty;
                    int start = 0;
                    for (int end = word.Length - 1; end >= 0 ; end--)
                    {
                        subword = subword.Substring(start, end - start);
                        var subwords = new List<string>();
                        if (dictionary.Contains(subword))
                        {
                            subwords.Add(subword);
                        }
                    }
                }
                //dictionary.Words =
                //    from word in dictionary.Words
                //    where word.Translations.Find(x => x.Title == )
            }
        }
    }
}
