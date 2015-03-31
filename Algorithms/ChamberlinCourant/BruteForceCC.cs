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
            var subsets = GetSubsets(preferences.Candidates.Keys.ToList(), winnersCount);
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

        private static IEnumerable<ISet<int>> GetSubsets(IList<int> superSet, int k)
        {
            var res = new List<ISet<int>>();
            GetSubsets(superSet, k, 0, new HashSet<int>(), res);
            return res;
        } 

        private static void GetSubsets(IList<int> superSet, int k, int idx, ISet<int> current, IList<ISet<int>> solution)
        {
            // successful stop clause
            if (current.Count == k)
            {
                solution.Add(new HashSet<int>(current));
                return;
            }

            // unsuccessful stop clause
            if (idx == superSet.Count)
                return;
            var x = superSet[idx];
            current.Add(x);

            // "guess" x is in the subset
            GetSubsets(superSet, k, idx + 1, current, solution);
            current.Remove(x);

            // "guess" x is not in the subset
            GetSubsets(superSet, k, idx + 1, current, solution);
        }
    }
}
