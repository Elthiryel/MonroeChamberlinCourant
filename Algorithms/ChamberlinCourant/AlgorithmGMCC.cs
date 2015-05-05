using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Algorithms.ChamberlinCourant
{
    public class AlgorithmGMCC : AbstractAlgorithm
    {
        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var assignment = Enumerable.Repeat(-1, preferences.NumberOfVoters).ToList();
            var used = new List<int>(winnersCount);
            var remaining = new List<int>(preferences.Candidates.Keys.ToList());
            for (var i = 1; i <= winnersCount; ++i)
            {
                assignment = GetBestAssignment(used, remaining, assignment, preferences, satisfactionFunction);
            }
            return new Results(preferences, assignment, satisfactionFunction, RuleType.ChamberlinCourant);
        }

        private List<int> GetBestAssignment(ICollection<int> used, ICollection<int> remaining, IReadOnlyList<int> assignment, 
            Preferences preferences, IList<int> satisfactionFunction)
        {
            var bestScore = -1;
            var bestAlternative = -1;
            var bestAssignment = new List<int>();
            foreach (var alternative in remaining)
            {
                var newAssignment = new List<int>(assignment);
                for (var i = 0; i < assignment.Count; ++i)
                {
                    var voter = preferences.VotersPreferences[i];
                    if (assignment[i] == -1 || (voter.IndexOf(assignment[i]) > voter.IndexOf(alternative)))
                        newAssignment[i] = alternative;
                }
                var results = new Results(preferences, newAssignment, satisfactionFunction, RuleType.ChamberlinCourant);
                var score = ScoreCalculator.CalculateScore(results);
                if (score > bestScore)
                {
                    bestScore = score;
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
