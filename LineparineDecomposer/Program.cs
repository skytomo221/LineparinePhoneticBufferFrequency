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
            //foreach (var q in new string[] { "misse's", "elmenerfe", "limufhu'i", "axelinielmejten", "evistreharkinsen" })
            foreach (var q in new string[] { "evistreharkinsen" })
            {
                var ans = ld.Decompose(q);
                Console.WriteLine(string.Join("\n", ans.Select(words => string.Join(" ", words))));
                Console.WriteLine();
            }
        }
    }
}
