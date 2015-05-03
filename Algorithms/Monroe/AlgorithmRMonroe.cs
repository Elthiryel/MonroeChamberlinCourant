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
                potentialWinners[i] = GetRandomAlternatives(preferences.Candidates.Keys.ToList(), winnersCount);
            }

            // TODO use network-flow-based approach to assign elements of potentialWinners

            throw new NotImplementedException();
        }

        private IList<int> GetRandomAlternatives(IList<int> alternatives, int numberNeeded)
        {
            var randomAlternatives = new List<int>(numberNeeded);
            var alternativesCount = alternatives.Count;
            for (var i = 0; i < alternativesCount; ++i)
            {
                var numberLeft = alternativesCount - i;
                var probability = numberNeeded / (double) numberLeft;
                var randomValue = _random.NextDouble();
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
