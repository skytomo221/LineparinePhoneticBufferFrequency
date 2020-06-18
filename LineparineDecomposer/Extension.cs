using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace LineparineDecomposer
{
    static class Extension
    {
        static string LastLetter(this string word)
        {
            var letters = new string[] {
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

        static string LastVowelLetter(this string letter) => Regex.Replace(letter, "[^iyuoea]", string.Empty).LastLetter();

        static int PhoneticBufferClass(this string word)
            => (string.IsNullOrEmpty(word.LastVowelLetter())) ?
            0 :
            (new Dictionary<string, int>
            {
                ["a"] = 1,
                ["o"] = 1,
                ["e"] = 2,
                ["i"] = 2,
                ["u"] = 3,
                ["y"] = 4,
            })[word.LastVowelLetter()];

        public static bool IsMatchToClass(this string left, string right)
            => (new Dictionary<int, string[]>
            {
                [0] = new string[] { },
                [1] = new string[] { "-a-", "-v-", },
                [2] = new string[] { "-e-", "-rg-", },
                [3] = new string[] { "-u-", "-m-", },
                [4] = new string[] { "-i-", "-l-", },
            })[left.PhoneticBufferClass()].Contains(right);
    }
}
