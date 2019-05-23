using System;
using System.Collections.Generic;
using System.Text;

namespace Aderant.Overlap
{
    class Program
    {
        static void Main(string[] args)
        {
            var fragments = new List<string> {
                "all is well",
                "ell that en",
                "hat end",
                "t ends well"
            };

            Overlap overlap = new Overlap(fragments);

            Console.WriteLine("Merge overlap of strings");
            Console.WriteLine("========================");
            Console.WriteLine("Input:");
            for(int i=0; i<fragments.Count; i++)
            {
                Console.WriteLine(fragments[i]);
            }
            Console.WriteLine("========================");
            Console.WriteLine("Result:");
            Console.WriteLine(overlap.MergeOverlap());
            Console.ReadKey();

            overlap.Dispose();
        }
    }
}
