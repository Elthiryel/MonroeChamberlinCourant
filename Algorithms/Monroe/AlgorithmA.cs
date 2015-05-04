using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class AlgorithmA : AbstractAlgorithm
    {
        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var ratio = (double) preferences.NumberOfVoters / winnersCount;
            var upperBound = (int) Math.Ceiling(ratio);
            var lowerBound = (int) Math.Floor(ratio);
            var upperBoundUseCount = (int) Math.Round((1 - (upperBound - ratio)) * winnersCount);

            var winners = Enumerable.Repeat(-1, preferences.NumberOfVoters).ToList();
            var remainingAlternatives = new List<int>(preferences.Candidates.Keys);
            var remainingAgents = Enumerable.Range(0, preferences.NumberOfVoters).ToList();

            for (var i = 1; i <= winnersCount; ++i)
            {
                var score = new Dictionary<int, int>();
                var bests = new Dictionary<int, IList<int>>();
                foreach (var alternative in remainingAlternatives)
                {
                    remainingAgents.Sort(new AgentComparer(alternative, preferences.VotersPreferences));
                    bests[alternative] = remainingAgents.Take(i <= upperBoundUseCount ? upperBound : lowerBound).ToList();
                    score[alternative] = bests[alternative].Sum(agent => satisfactionFunction[preferences.VotersPreferences[agent].IndexOf(alternative)]);
                }
                var bestScore = 0;
                var bestAlternative = -1;
                foreach (var pair in score)
                {
                    if (pair.Value >= bestScore)
                    {
                        bestScore = pair.Value;
                        bestAlternative = pair.Key;
                    }
                }
                foreach (var agent in bests[bestAlternative])
                {
                    winners[agent] = bestAlternative;
                    remainingAgents.Remove(agent);
                }
                remainingAlternatives.Remove(bestAlternative);
            }

            var results = new Results(preferences, winners, satisfactionFunction, RuleType.Monroe);
            return results;
        }
    }
}
