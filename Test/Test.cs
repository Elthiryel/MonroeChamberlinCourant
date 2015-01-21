using System;
using System.Collections.Generic;
using MonroeChamberlinCourant.Framework.Generation;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Test
{
    public class Test
    {
        public static void Main(string[] args)
        {
//            var preferences = DataLoader.LoadPreferences("example_prefs.txt");
//            var a = 1;
            var candidates = new Dictionary<int, Candidate>(5)
            {
                {8, new Candidate(8, "AAA")},
                {23, new Candidate(23, "BBB")},
                {45, new Candidate(45, "CCC")},
                {22, new Candidate(22, "DDD")},
                {9, new Candidate(9, "EEE")}
            };
            var generator = new PolyaStrictGenerator();
            var preferences = generator.Generate(candidates, 20);
            Console.Write(preferences);
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
