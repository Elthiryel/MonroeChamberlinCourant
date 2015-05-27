using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using MonroeChamberlinCourant.Algorithms;
using MonroeChamberlinCourant.Algorithms.ChamberlinCourant;
using MonroeChamberlinCourant.Algorithms.Monroe;
using MonroeChamberlinCourant.Framework.Data;
using MonroeChamberlinCourant.Framework.Generation;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Test
{
    public class Test
    {
        public static void Main(string[] args)
        {
            var preferences = GenerationHelper.GenerateAndPersistData(new PolyaStrictGenerator(), 10, 10, "test.txt");
//
////            var preferences = DataLoader.LoadPreferences("test.txt");
//            
//            // MONROE
//            Console.WriteLine("MONROE");
//            RunAlgorithm(new AlgorithmA(), preferences, "Algorithm A");
//            RunAlgorithm(new AlgorithmCMonroe(3), preferences, "Algorithm C (3)");
//            
            // CHAMBERLIN-COURANT
            Console.WriteLine("CHAMBERLIN-COURANT");
            RunAlgorithm(new AlgorithmCCC(3), preferences, "Algorithm C (3)");
            RunAlgorithm(new AlgorithmGMCC(), preferences, "Algorithm GM");
            RunAlgorithm(new AlgorithmP(), preferences, "Algorithm P");
            RunAlgorithm(new AlgorithmRCC(5), preferences, "Algorithm R (5)");
            RunAlgorithm(new GeneticAlgorithmCC(10, 10), preferences, "Genetic Algorithm (10, 10)");
            RunAlgorithm(new BruteForceCC(), preferences, "Brute Force");

            // MONROE
            Console.WriteLine("MONROE");
            RunAlgorithm(new AlgorithmA(), preferences, "Algorithm A");
            RunAlgorithm(new AlgorithmB(), preferences, "Algorithm B");
            RunAlgorithm(new AlgorithmCMonroe(3), preferences, "Algorithm C (3)");
            RunAlgorithm(new AlgorithmRMonroe(5), preferences, "Algorithm R (5)");
            RunAlgorithm(new AlgorithmAR(0.015, 0.75), preferences, "Algorithm AR (0.014, 0.75");
            RunAlgorithm(new AlgorithmGMMonroe(), preferences, "Algorithm GM");
            RunAlgorithm(new BruteForceWithFlowMonroe(), preferences, "Brute Force with Flow");

//
//            Console.WriteLine("DONE...");
//            Console.ReadKey();
//            RunAlgorithm(new BruteForceWithFlowMonroe(), preferences, "Brute-force with flow (Monroe)");
//            RunAlgorithm(new BruteForceMonroe(), preferences, "Brute-force (Monroe)");
//            var x = AlgorithmUtils.LambertsFunction(100);
//            Console.WriteLine(x);
            Console.ReadLine();

            //TestMinCost();

        }

        private static void RunAlgorithm(IAlgorithm algorithm, Preferences preferences, string label)
        {
            Console.WriteLine(label);
            var stopwatch = Stopwatch.StartNew();
            //var results = algorithm.Run(preferences, 5, new[] {262144, 131072, 65536, 32768, 16384, 8192, 4096, 2048, 1024, 512, 256, 128, 64, 32, 16, 8, 4, 2, 1, 0});
            var results = algorithm.Run(preferences, 2, new[] {9, 8, 7, 6, 5, 4, 3, 2, 1, 0});
            stopwatch.Stop();
//            Console.WriteLine();
//            foreach (var winner in results.Winners)
//            {
//                Console.Write(winner);
//                Console.Write(" ");
//            }
//            Console.WriteLine();
            Console.WriteLine("Time taken: {0}ms", stopwatch.Elapsed.TotalMilliseconds);
            var score = ScoreCalculator.CalculateScore(results);
            Console.Write("Score: {0}", score);
            Console.WriteLine();
        }

        private static void TestMinCost()
        {
//            var flow = new MinCostMaxFlow();
//
//            double[,] cap = {{0, 3, 4, 5, 0},
//                       {0, 0, 2, 0, 0},
//                       {0, 0, 0, 4, 1},
//                       {0, 0, 0, 0, 10},
//                       {0, 0, 0, 0, 0}};
//
//            double[,] cost1 = {{0, 1, 0, 0, 0},
//                         {0, 0, 0, 0, 0},
//                         {0, 0, 0, 0, 0},
//                         {0, 0, 0, 0, 0},
//                         {0, 0, 0, 0, 0}};
//
//            double[,] cost2 = {{0, 0, 1, 0, 0},
//                         {0, 0, 0, 0, 0},
//                         {0, 0, 0, 0, 0},
//                         {0, 0, 0, 0, 0},
//                         {0, 0, 0, 0, 0}};
//
//            double[] ret1 = flow.GetMaxFlow(cap, cost1, 0, 4);
//            double[] ret2 = flow.GetMaxFlow(cap, cost2, 0, 4);
//
//            Console.WriteLine("{0} {1}", ret1[0], ret1[1]);
//            Console.WriteLine("{0} {1}", ret2[0], ret2[1]);
//
//            Console.ReadLine();
        }
    }
}
