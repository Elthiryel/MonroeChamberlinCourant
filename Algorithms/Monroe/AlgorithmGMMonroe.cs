using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class AlgorithmGMMonroe : AbstractAlgorithm
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
                // TODO use network-flow-based approach to assign alternatives to agents (partial K-assignment)
            }
            throw new NotImplementedException();
        }
    }
}
