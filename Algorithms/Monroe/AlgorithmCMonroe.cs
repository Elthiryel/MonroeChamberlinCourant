using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class AlgorithmCMonroe : AbstractAlgorithm
    {
        public AlgorithmCMonroe(int d)
        {
            _d = d;
        }

        private readonly int _d;

        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var ratio = (double) preferences.NumberOfVoters / winnersCount;
            var upperBound = (int) Math.Ceiling(ratio);
            var lowerBound = (int) Math.Floor(ratio);
            var upperBoundUseCount = (int) Math.Round((1 - (upperBound - ratio)) * winnersCount);

            var par = new List<AssignmentInfo>(1) {new AssignmentInfo(preferences, true)};

            for (var i = 1; i <= winnersCount; ++i)
            {
                var newPar = new List<AssignmentInfo>(par[0].RemainingAlternatives.Count);
                foreach (var assignment in par)
                {
                    var bests = new Dictionary<int, IList<int>>();
                    foreach (var alternative in assignment.RemainingAlternatives)
                    {
                        assignment.RemainingAgents.Sort(new AgentComparer(alternative, preferences.VotersPreferences));
                        bests[alternative] = assignment.RemainingAgents.Take(i <= upperBoundUseCount ? upperBound : lowerBound).ToList();
                        var newAssignment = AssignmentInfo.CopyFrom(assignment);
                        foreach (var agent in bests[alternative])
                        {
                            newAssignment.Assignment[agent] = alternative;
                            newAssignment.RemainingAgents.Remove(agent);
                        }
                        newAssignment.RemainingAlternatives.Remove(alternative);
                        newPar.Add(newAssignment);
                    }
                }
                newPar = AssignScoresAndSort(newPar, preferences, satisfactionFunction);
                par = newPar.Take(_d).ToList();
            }

            foreach (var assignmentInfo in par)
            {
                assignmentInfo.Assignment = AlgorithmUtils.AssignBestForMonroe(assignmentInfo.Assignment.Distinct().ToList(), preferences.VotersPreferences, satisfactionFunction);
            }

            var finalPar = AssignScoresAndSort(par, preferences, satisfactionFunction);

            return new Results(preferences, finalPar[0].Assignment, satisfactionFunction, RuleType.Monroe);
        }

        private static List<AssignmentInfo> AssignScoresAndSort(List<AssignmentInfo> par, Preferences preferences, IList<int> satisfactionFunction)
        {
            foreach (var assignmentInfo in par)
            {
                var score = 0;
                for (var i = 0; i < assignmentInfo.Assignment.Count; ++i)
                {
                    var alternative = assignmentInfo.Assignment[i];
                    if (alternative >= 0)
                    {
                        var position = preferences.VotersPreferences[i].IndexOf(alternative);
                        score += satisfactionFunction[position];
                    }
                }
                assignmentInfo.Score = score;
            }
            return par.OrderByDescending(assignmentInfo => assignmentInfo.Score).ToList();
        }
    }
}
