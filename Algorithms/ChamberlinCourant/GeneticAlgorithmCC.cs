using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Algorithms.ChamberlinCourant
{
    public class GeneticAlgorithmCC : AbstractAlgorithm
    {
        private readonly int _numberOfIterations;
        private readonly int _numberOfCreatures;
        private readonly Random _random;

        private Preferences _preferences;
        private int _winnersCount;
        private IList<int> _satisfactionFunction; 

        public GeneticAlgorithmCC(int numberOfIterations, int numberOfCreatures)
        {
            _numberOfIterations = numberOfIterations;
            _numberOfCreatures = numberOfCreatures;
            _random = new Random();
        }

        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            _preferences = preferences;
            _winnersCount = winnersCount;
            _satisfactionFunction = satisfactionFunction;

            var available = preferences.Candidates.Keys.ToList();

            var creatures = GetRandomCreatures(available);
            var bestCreature = creatures.OrderByDescending(c => c.Score).First();
            var bestScore = bestCreature.Score;

            for (var i = 0; i < _numberOfIterations; ++i)
            {
                Results iterationBestCreature;
                creatures = PerformIteration(creatures, available, out iterationBestCreature);
                if (iterationBestCreature.Score > bestScore)
                {
                    bestScore = iterationBestCreature.Score;
                    bestCreature = iterationBestCreature;
                }
            }

            return bestCreature;
        }

        private IList<Results> GetRandomCreatures(IList<int> available)
        {
            var creatures = new List<Results>(_numberOfCreatures);
            for (var i = 0; i < _numberOfCreatures; ++i)
            {
                var distinctWinners = AlgorithmUtils.GetRandomAlternatives(available, _winnersCount, _random);
                var results = GetResults(distinctWinners);
                creatures.Add(results);
            }
            return creatures;
        } 

        private IList<Results> PerformIteration(IEnumerable<Results> solutions, IList<int> available, out Results bestSolution)
        {
            var newSolutions = new List<Results>(_numberOfCreatures);

            var half = _numberOfCreatures / 2;

            var sorted = solutions.OrderByDescending(s => s.Score);
            var better = sorted.Take(half).ToList();

            var bestScore = -1;
            bestSolution = null;

            foreach (var solution in better)
            {
                var distinctWinners = solution.Winners.Distinct().ToList();
                var newDistinctWinners = Mutation(distinctWinners, available, 1, _random);
                var results = GetResults(newDistinctWinners);
                if (results.Score > bestScore)
                {
                    bestScore = results.Score != null ? (int) results.Score : -1;
                    bestSolution = results;
                }
                newSolutions.Add(results);
            }

            for (var i = half; i < _numberOfCreatures; ++i)
            {
                var first = _random.Next(half);
                var second = _random.Next(half);
                var newDistinctWinners = Crossover(better[first].Winners.Distinct(), better[second].Winners.Distinct(), _winnersCount, _random);
                var results = GetResults(newDistinctWinners);
                if (results.Score > bestScore)
                {
                    bestScore = results.Score != null ? (int)results.Score : -1;
                    bestSolution = results;
                }
                newSolutions.Add(results);
            }

            return newSolutions;
        }

        private Results GetResults(IList<int> distinctWinners)
        {
            while (distinctWinners.Count < _winnersCount)
                distinctWinners.Add(_preferences.Candidates.Keys.First());
            var winners = Enumerable.Repeat(distinctWinners[0], _preferences.NumberOfVoters).ToList();
            for (var i = 1; i < _winnersCount; ++i)
            {
                for (var j = 0; j < _preferences.NumberOfVoters; ++j)
                {
                    var voter = _preferences.VotersPreferences[j];
                    if (voter.IndexOf(distinctWinners[i]) < voter.IndexOf(winners[j]))
                        winners[j] = distinctWinners[i];
                }
            }
            var results = new Results(_preferences, winners, _satisfactionFunction, RuleType.ChamberlinCourant);
            results.Score = ScoreCalculator.CalculateScore(results);
            return results;
        }

        private IList<int> Crossover(IEnumerable<int> first, IEnumerable<int> second, int size, Random random)
        {
            var allElements = first.Union(second).ToList();
            return AlgorithmUtils.GetRandomAlternatives(allElements, size, random);
        }

        private IList<int> Mutation(IList<int> creature, IList<int> available, int randomization, Random random)
        {
            var newCreature = new List<int>(creature);
            for (var i = 0; i < randomization; ++i)
            {
                var missing = available.Except(creature).ToList();
                var indexFrom = random.Next(newCreature.Count);
                var indexTo = random.Next(missing.Count);
                newCreature[indexFrom] = missing[indexTo];
            }
            return newCreature;
        }
    }
}
