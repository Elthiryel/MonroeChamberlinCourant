using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class AlgorithmCMonroe : AbstractAlgorithm
    {
        private class AgentComparer : IComparer<int>
        {
            private readonly int _alternativeId;
            private readonly IList<IList<int>> _agentsPreferences;

            public AgentComparer(int alternativeId, IList<IList<int>> agentsPreferences)
            {
                _alternativeId = alternativeId;
                _agentsPreferences = agentsPreferences;
            }

            public int Compare(int x, int y)
            {
                var xPosition = _agentsPreferences[x].IndexOf(_alternativeId);
                var yPosition = _agentsPreferences[y].IndexOf(_alternativeId);
                return xPosition - yPosition;
            }
        }

        private class AssignmentInfo
        {
            private AssignmentInfo()
            {
            }

            public AssignmentInfo(Preferences preferences)
            {
                Assignment = Enumerable.Repeat(-1, preferences.NumberOfVoters).ToList();
                RemainingAlternatives = new List<int>(preferences.Candidates.Keys);
                RemainingAgents = Enumerable.Range(0, preferences.NumberOfVoters).ToList();
                Score = 0;
            }

            public List<int> Assignment { get; private set; } 
            public List<int> RemainingAlternatives { get; private set; }
            public List<int> RemainingAgents { get; private set; }
            public int Score { get; set; }

            public static AssignmentInfo CopyFrom(AssignmentInfo assignmentInfo)
            {
                var newAssignmentInfo = new AssignmentInfo
                {
                    Assignment = new List<int>(assignmentInfo.Assignment),
                    RemainingAlternatives = new List<int>(assignmentInfo.RemainingAlternatives),
                    RemainingAgents = new List<int>(assignmentInfo.RemainingAgents),
                    Score = assignmentInfo.Score
                };
                return newAssignmentInfo;
            }
        }

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

            var par = new List<AssignmentInfo>(1) {new AssignmentInfo(preferences)};

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

            // TODO use network-flow-based approach to reassign elements of par

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
