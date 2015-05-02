using System;
using System.Collections.Generic;
using MonroeChamberlinCourant.Algorithms.ChamberlinCourant;
using MonroeChamberlinCourant.Algorithms.Monroe;
using MonroeChamberlinCourant.Framework.Generation;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

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

//            var results = new Results(preferences, new[] {8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8},
//                new[] {4, 3, 2, 1, 0}, RuleType.ChamberlinCourant);
//            var score = ScoreCalculator.CalculateScore(results);
//            Console.WriteLine(score);

//            var algorithm = new BruteForceCC();
//            var algorithm = new BruteForceMonroe();
            var algorithm = new AlgorithmA();

            var results = algorithm.Run(preferences, 3, p => -p + 4);

            foreach (var winner in results.Winners)
            {
                Console.Write(winner);
                Console.Write(" ");
            }
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
