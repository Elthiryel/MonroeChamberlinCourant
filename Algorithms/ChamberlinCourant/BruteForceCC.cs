using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Algorithms.ChamberlinCourant
{
    public class BruteForceCC : AbstractAlgorithm
    {
        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var subsets = AlgorithmUtils.GetSubsets(preferences.Candidates.Keys.ToList(), winnersCount);
            var bestScore = 0;
            Results results = null;
            foreach (var subset in subsets)
            {
                var winners = new List<int>(preferences.NumberOfVoters);
                foreach (var singleVoterPreferences in preferences.VotersPreferences)
                {
                    var position = 0;
                    while (true)
                    {
                        if (subset.Contains(singleVoterPreferences[position]))
                        {
                            winners.Add(singleVoterPreferences[position]);
                            break;
                        }
                        ++position;
                    }
                }
                var tempResults = new Results(preferences, winners, satisfactionFunction, RuleType.ChamberlinCourant);
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
