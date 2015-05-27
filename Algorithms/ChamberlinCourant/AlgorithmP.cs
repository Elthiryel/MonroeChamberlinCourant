using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms.ChamberlinCourant
{
    public class AlgorithmP : AbstractAlgorithm
    {
        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var winners = Enumerable.Repeat(-1, preferences.NumberOfVoters).ToList();
            var x = GetX(preferences.NumberOfCandidates, winnersCount);
            var remainingAlternatives = preferences.Candidates.Keys.ToList();

            for (var i = 1; i <= winnersCount; ++i)
            {
                var bestCount = -1;
                var bestAlternative = -1;
                foreach (var alternative in remainingAlternatives)
                {
                    var count = 0;
                    for (var j = 0; j < preferences.NumberOfVoters; ++j)
                    {
                        if (winners[j] == -1 && preferences.VotersPreferences[j].IndexOf(alternative) < x)
                            ++count;
                    }
                    if (count > bestCount)
                    {
                        bestCount = count;
                        bestAlternative = alternative;
                    }
                }
                for (var j = 0; j < preferences.NumberOfVoters; ++j)
                {
                    var voter = preferences.VotersPreferences[j];
                    if (winners[j] == -1 || voter.IndexOf(bestAlternative) < voter.IndexOf(winners[j]))
                    {
                        winners[j] = bestAlternative;
                    }
                }
                remainingAlternatives.Remove(bestAlternative);
            }

            return new Results(preferences, winners, satisfactionFunction, RuleType.ChamberlinCourant);
        }

        private static int GetX(int m, int k)
        {
            var w = AlgorithmUtils.LambertsFunction(k);
            var x = (int) Math.Ceiling(m * w / k);
            return x;
        }
    }
}
