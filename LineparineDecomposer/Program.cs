using System;
using System.Collections.Generic;

namespace LineparineDecomposer
{
    class Program
    {
        static void Main(string[] args)
        {
            var ld = new LineparineDecomposer();
            var ans = ld.Decompose("misse's");
            Console.WriteLine(string.Join(" ", ans[0]));
        }
    }
}
