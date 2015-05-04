using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms.ChamberlinCourant
{
    public class AlgorithmCCC : AbstractAlgorithm
    {
        public AlgorithmCCC(int d)
        {
            _d = d;
        }

        private readonly int _d;

        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var par = new List<AssignmentInfo>(1) {new AssignmentInfo(preferences, false)};

            for (var i = 1; i <= winnersCount; ++i)
            {
                var newPar = new List<AssignmentInfo>(par[0].RemainingAlternatives.Count);
                foreach (var assignment in par)
                {
                    foreach (var alternative in assignment.RemainingAlternatives)
                    {
                        var newAssignment = AssignmentInfo.CopyFrom(assignment);
                        for (var j = 0; j < preferences.NumberOfVoters; ++j)
                        {
                            if (newAssignment.Assignment[j] == -1)
                                newAssignment.Assignment[j] = alternative;
                            else if (preferences.VotersPreferences[j].IndexOf(alternative) < preferences.VotersPreferences[j].IndexOf(newAssignment.Assignment[j]))
                                newAssignment.Assignment[j] = alternative;
                        }
                        newAssignment.RemainingAlternatives.Remove(alternative);
                        newPar.Add(newAssignment);
                    }
                }
                newPar = AssignScoresAndSort(newPar, preferences, satisfactionFunction);
                par = newPar.Take(_d).ToList();
            }

            var finalPar = AssignScoresAndSort(par, preferences, satisfactionFunction);

            return new Results(preferences, finalPar[0].Assignment, satisfactionFunction, RuleType.ChamberlinCourant);
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
