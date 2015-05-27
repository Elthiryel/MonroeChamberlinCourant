using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class AlgorithmGMMonroe : AbstractAlgorithm
    {
        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            IList<int> assignment = Enumerable.Repeat(-1, preferences.NumberOfVoters).ToList();
            var used = new List<int>(winnersCount);
            var remaining = new List<int>(preferences.Candidates.Keys.ToList());
            for (var i = 1; i <= winnersCount; ++i)
            {
                assignment = GetBestAssignment(used, remaining, preferences, satisfactionFunction, winnersCount);
            }
            return new Results(preferences, assignment, satisfactionFunction, RuleType.Monroe);
        }

        private IList<int> GetBestAssignment(ICollection<int> used, ICollection<int> remaining, Preferences preferences, IList<int> satisfactionFunction, int winnersCount)
        {
            var bestScore = -1;
            var bestAlternative = -1;
            IList<int> bestAssignment = new List<int>();
            foreach (var alternative in remaining)
            {
                var currentAlternatives = new List<int>(used.Count + 1);
                currentAlternatives.AddRange(used);
                currentAlternatives.Add(alternative);
                var newAssignment = AlgorithmUtils.AssignBestForMonroe(currentAlternatives, preferences.VotersPreferences, satisfactionFunction, winnersCount);
                var newResults = new Results(preferences, newAssignment, satisfactionFunction, RuleType.Monroe);
                var newScore = ScoreCalculator.CalculateScore(newResults);
                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestAlternative = alternative;
                    bestAssignment = newAssignment;
                }
            }
            used.Add(bestAlternative);
            remaining.Remove(bestAlternative);
            return bestAssignment;
        }
    }
}
