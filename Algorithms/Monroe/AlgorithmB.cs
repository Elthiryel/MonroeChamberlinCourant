using System.Collections.Generic;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class AlgorithmB : AbstractAlgorithm
    {
        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var algorithmA = new AlgorithmA();
            var initialResults = algorithmA.Run(preferences, winnersCount, satisfactionFunction);

            // TODO use network-flow-based approach to reassign initialResults

            return initialResults;
        }
    }
}
