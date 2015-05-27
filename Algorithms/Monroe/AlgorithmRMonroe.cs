using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class AlgorithmRMonroe : AbstractAlgorithm
    {
        private readonly int _samplingSteps;
        private readonly Random _random;

        public AlgorithmRMonroe(int samplingSteps)
        {
            _samplingSteps = samplingSteps;
            _random = new Random();
        }

        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var potentialWinners = new List<IList<int>>(_samplingSteps);
            for (var i = 0; i < _samplingSteps; ++i)
            {
                potentialWinners.Add(AlgorithmUtils.GetRandomAlternatives(preferences.Candidates.Keys.ToList(), winnersCount, _random));
            }

            var bestScore = -1;
            Results bestResults = null;
            foreach (var candidates in potentialWinners)
            {
                var winners = AlgorithmUtils.AssignBestForMonroe(candidates, preferences.VotersPreferences, satisfactionFunction);
                var results = new Results(preferences, winners, satisfactionFunction, RuleType.Monroe);
                var score = ScoreCalculator.CalculateScore(results);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestResults = results;
                }
            }

            return bestResults;
        }
    }
}
