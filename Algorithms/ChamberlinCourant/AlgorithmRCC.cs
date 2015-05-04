using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Algorithms.ChamberlinCourant
{
    public class AlgorithmRCC : AbstractAlgorithm
    {
        private readonly int _samplingSteps;
        private readonly Random _random;

        public AlgorithmRCC(int samplingSteps)
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
            var bestWinners = new List<int>();

            foreach (var alternatives in potentialWinners)
            {
                var winners = new List<int>(preferences.NumberOfVoters);
                foreach (var agent in preferences.VotersPreferences)
                {
                    var bestPosition = preferences.NumberOfCandidates;
                    var bestAlternative = -1;
                    foreach (var alternative in alternatives)
                    {
                        var position = agent.IndexOf(alternative);
                        if (position < bestPosition)
                        {
                            bestPosition = position;
                            bestAlternative = alternative;
                        }
                    }
                    winners.Add(bestAlternative);
                }
                var score = ScoreCalculator.CalculateScore(new Results(preferences, winners, satisfactionFunction, RuleType.ChamberlinCourant));
                if (score > bestScore)
                {
                    bestScore = score;
                    bestWinners = winners;
                }
            }
            
            return new Results(preferences, bestWinners, satisfactionFunction, RuleType.ChamberlinCourant);
        }
    }
}
