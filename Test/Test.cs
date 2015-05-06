using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MonroeChamberlinCourant.Algorithms;
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
            var votersPreferences = new List<IList<int>>
            {
                new List<int> {23, 8, 22, 9, 45},
                new List<int> {45, 22, 8, 23, 9},
                new List<int> {8, 22, 9, 23, 45},
                new List<int> {23, 8, 9, 22, 45},
                new List<int> {22, 9, 23, 45, 8},
                new List<int> {8, 22, 9, 23, 45},
                new List<int> {9, 22, 45, 8, 23},
                new List<int> {23, 8, 22, 45, 9},
                new List<int> {8, 45, 22, 23, 9},
                new List<int> {23, 8, 9, 22, 45},
                new List<int> {9, 8, 22, 45, 23},
                new List<int> {45, 23, 9, 8, 22},
                new List<int> {23, 22, 9, 8, 45},
                new List<int> {45, 8, 23, 22, 9},
                new List<int> {9, 8, 22, 23, 45},
                new List<int> {8, 23, 22, 45, 9},
                new List<int> {23, 22, 8, 9, 45},
                new List<int> {45, 23, 22, 8, 9},
                new List<int> {9, 8, 22, 23, 45},
                new List<int> {8, 22, 23, 9, 45}
            };

//            var preferences = new Preferences(5, candidates, 20, votersPreferences);
            Console.Write(preferences);
            Console.WriteLine();

//            var results = new Results(preferences, new[] {8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8},
//                new[] {4, 3, 2, 1, 0}, RuleType.ChamberlinCourant);
//            var score = ScoreCalculator.CalculateScore(results);
//            Console.WriteLine(score);

//            var algorithm = new BruteForceCC();
//            var algorithm = new BruteForceMonroe();
//            var algorithm = new AlgorithmA();


//            for (var i = 0; i < 1000; ++i)
//            {
//                var results = new AlgorithmA().Run(preferences, 3, p => -p + 4);
//                Console.Write(ScoreCalculator.CalculateScore(results));
//                Console.WriteLine();
//            }
//            RunAlgorithm(new AlgorithmA(), preferences, "A Monroe");
//            RunAlgorithm(new AlgorithmCMonroe(3), preferences, "C Monroe (3)");
//            RunAlgorithm(new AlgorithmCCC(3), preferences, "C Cha-Cou (3))");
//            RunAlgorithm(new AlgorithmRCC(10), preferences, "R Cha-Cou (10)");
//            RunAlgorithm(new AlgorithmGMCC(), preferences, "GM Cha-Cou");
            RunAlgorithm(new AlgorithmP(), preferences, "P Cha-Cou");
            RunAlgorithm(new BruteForceCC(), preferences, "BruteForce Cha-Cou");
//            RunAlgorithm(new AlgorithmCMonroe(2), preferences);
//            RunAlgorithm(new AlgorithmCMonroe(3), preferences);

            Console.ReadKey();
        }

        private static void RunAlgorithm(IAlgorithm algorithm, Preferences preferences, string label)
        {
            Console.WriteLine(label);
            var results = algorithm.Run(preferences, 3, p => -p + 4);
            foreach (var winner in results.Winners)
            {
                Console.Write(winner);
                Console.Write(" ");
            }
            Console.WriteLine();
            var score = ScoreCalculator.CalculateScore(results);
            Console.Write(score);
            Console.WriteLine();
        }
    }
}
