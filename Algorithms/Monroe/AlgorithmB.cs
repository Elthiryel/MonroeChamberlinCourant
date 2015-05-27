using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class AlgorithmB : AbstractAlgorithm
    {
        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var algorithmA = new AlgorithmA();
            var initialResults = algorithmA.Run(preferences, winnersCount, satisfactionFunction);
            var finalWinners = AlgorithmUtils.AssignBestForMonroe(initialResults.Winners.Distinct().ToList(), preferences.VotersPreferences, satisfactionFunction);
            return new Results(preferences, finalWinners, satisfactionFunction, RuleType.Monroe);
        }
    }
}
