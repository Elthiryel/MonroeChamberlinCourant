using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class BruteForceWithFlowMonroe : AbstractAlgorithm
    {
        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var subsets = AlgorithmUtils.GetSubsets(preferences.Candidates.Keys.ToList(), winnersCount);
            var bestScore = 0;
            Results results = null;
            foreach (var subset in subsets)
            {
                var winners = AlgorithmUtils.AssignBestForMonroe(new List<int>(subset), preferences.VotersPreferences, satisfactionFunction);
                var tempResults = new Results(preferences, winners, satisfactionFunction, RuleType.Monroe);
                var score = ScoreCalculator.CalculateScore(tempResults);
                if (score > bestScore)
                {
                    bestScore = score;
                    results = tempResults;
                }
            }
            return results;
        }
    }
}
