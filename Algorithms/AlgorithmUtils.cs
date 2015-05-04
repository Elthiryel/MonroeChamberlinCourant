using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms
{
    public class AgentComparer : IComparer<int>
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

    public class AssignmentInfo
    {
        private AssignmentInfo()
        {
        }

        public AssignmentInfo(Preferences preferences, bool hasAgents)
        {
            Assignment = Enumerable.Repeat(-1, preferences.NumberOfVoters).ToList();
            RemainingAlternatives = new List<int>(preferences.Candidates.Keys);
            RemainingAgents = hasAgents ? Enumerable.Range(0, preferences.NumberOfVoters).ToList() : null;
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
                RemainingAgents = assignmentInfo.RemainingAgents != null ? new List<int>(assignmentInfo.RemainingAgents) : null,
                Score = assignmentInfo.Score
            };
            return newAssignmentInfo;
        }
    }

    public class AlgorithmUtils
    {
        public static IList<int> GetRandomAlternatives(IList<int> alternatives, int numberNeeded, Random random)
        {
            var randomAlternatives = new List<int>(numberNeeded);
            var alternativesCount = alternatives.Count;
            for (var i = 0; i < alternativesCount; ++i)
            {
                var numberLeft = alternativesCount - i;
                var probability = numberNeeded / (double)numberLeft;
                var randomValue = random.NextDouble();
                if (randomValue <= probability)
                {
                    randomAlternatives.Add(alternatives[i]);
                    --numberNeeded;
                    if (numberNeeded == 0)
                        break;
                }
            }
            return randomAlternatives;
        } 
    }
}
