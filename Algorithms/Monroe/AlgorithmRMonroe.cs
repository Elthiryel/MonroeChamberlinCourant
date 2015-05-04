using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class AlgorithmRMonroe : AbstractAlgorithm
    {
        private readonly int _samplingSteps;
        private readonly Random _random;

        public AlgorithmRMonroe(int samplingSteps)
        {
            _samplingSteps = samplingSteps;
            _random = new Random();
        }

        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var potentialWinners = new List<IList<int>>(_samplingSteps);
            for (var i = 0; i < _samplingSteps; ++i)
            {
                potentialWinners[i] = AlgorithmUtils.GetRandomAlternatives(preferences.Candidates.Keys.ToList(), winnersCount, _random);
            }

            // TODO use network-flow-based approach to assign elements of potentialWinners

            throw new NotImplementedException();
        }


    }
}
