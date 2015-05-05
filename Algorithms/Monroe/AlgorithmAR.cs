using System;
using System.Collections.Generic;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class AlgorithmAR : AbstractAlgorithm
    {
        private readonly double _epsilon;
        private readonly double _lambda;

        public AlgorithmAR(double epsilon, double lambda)
        {
            _epsilon = epsilon;
            _lambda = lambda;
        }

        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            if (preferences.NumberOfCandidates <= 1.0 + (2.0/_epsilon))
            {
                var bruteForceAlgorithm = new BruteForceMonroe();
                return bruteForceAlgorithm.Run(preferences, winnersCount, satisfactionFunction);
            }

            var algorithmA = new AlgorithmA();
            var resultsA = algorithmA.Run(preferences, winnersCount, satisfactionFunction);
            var scoreA = ScoreCalculator.CalculateScore(resultsA);

            var samplingSteps = (int) Math.Ceiling(- Math.Log(1.0 - _lambda) * (2.0 + _epsilon) / _epsilon);
            var algorithmR = new AlgorithmRMonroe(samplingSteps);
            var resultsR = algorithmR.Run(preferences, winnersCount, satisfactionFunction);
            var scoreR = ScoreCalculator.CalculateScore(resultsR);

            return scoreA > scoreR ? resultsA : resultsR;
        }
    }
}
