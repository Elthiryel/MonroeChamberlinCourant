using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class BruteForceMonroe : AbstractAlgorithm
    {
        private int _upperBound;
        private int _lowerBound;
        private int _winnersCount;

        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            _winnersCount = winnersCount;
            var ratio = (double) preferences.NumberOfVoters / winnersCount;
            _upperBound = (int) Math.Ceiling(ratio);
            _lowerBound = (int) Math.Floor(ratio);

            var solutions = new List<IList<int>>();
            GetPossibleWinnersLists(0, preferences.NumberOfVoters, new List<int>(preferences.Candidates.Keys), new List<int>(), solutions);

            Results bestResults = null;
            var bestScore = 0;

            foreach (var solution in solutions)
            {
                var results = new Results(preferences, solution, satisfactionFunction, RuleType.Monroe);
                var score = ScoreCalculator.CalculateScore(results);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestResults = results;
                }
            }
            
            return bestResults;
        }

        private void GetPossibleWinnersLists(int i, int limit, IList<int> candidates, IList<int> currentSolution, IList<IList<int>> solutions)
        {
            if (i == limit)
            {
                if (ValidateSolution(currentSolution))
                    solutions.Add(new List<int>(currentSolution));
                return;
            }

            var currentWinners = currentSolution.Distinct().ToList();
            foreach (var candidate in candidates)
            {
                if (currentWinners.Contains(candidate))
                {
                    if (currentSolution.Count(j => j == candidate) >= _upperBound)
                        continue;
                    currentSolution.Add(candidate);
                    GetPossibleWinnersLists(i + 1, limit, candidates, currentSolution, solutions);
                    currentSolution.RemoveAt(i);
                }
                else if (currentWinners.Count < _winnersCount)
                {
                    currentSolution.Add(candidate);
                    GetPossibleWinnersLists(i + 1, limit, candidates, currentSolution, solutions);
                    currentSolution.RemoveAt(i);
                }
            }
        }

        private bool ValidateSolution(IList<int> solution)
        {
            var groups = solution.GroupBy(i => i).ToList();
            return groups.Count == _winnersCount && groups.Select(@group => @group.Count()).All(count => count >= _lowerBound && count <= _upperBound);
        }
    }
}
