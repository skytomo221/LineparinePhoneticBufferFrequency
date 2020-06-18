using System;
using System.Collections.Generic;
using System.Linq;

namespace LineparineDecomposer
{
    class Program
    {
        static void Main(string[] args)
        {
            var ld = new LineparineDecomposer();
            var ans = ld.Decompose("misse's");
            Console.WriteLine(string.Join("\n", ans.Select(words => string.Join(" ", words))));
            Console.WriteLine();
            ans = ld.Decompose("elmenerfe");
            Console.WriteLine(string.Join("\n", ans.Select(words => string.Join(" ", words))));
            Console.WriteLine();
            ans = ld.Decompose("axelinielmejten");
            Console.WriteLine(string.Join("\n", ans.Select(words => string.Join(" ", words))));
        }
    }
}
