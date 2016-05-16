using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Algorithms.Monroe
{
    public class SimulatedAnnealingMonroe : AbstractAlgorithm
    {
        private readonly double _coolingRate;
        private readonly Random _random;

        private double _temperature;

        public SimulatedAnnealingMonroe(double temperature, double coolingRate)
        {
            _temperature = temperature;
            _coolingRate = coolingRate;
            _random = new Random();
        }

        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var maxEnergyValue = (UInt64)satisfactionFunction[0] * (UInt64)preferences.NumberOfVoters;
            var alternatives = new List<int>(preferences.Candidates.Keys);

            var currentSolution = AlgorithmUtils.GetRandomAlternatives(alternatives, winnersCount, _random);
            var currentSolutionWinners = AlgorithmUtils.AssignBestForCC(currentSolution, preferences.VotersPreferences);
            var currentEnergy = GetEnergy(currentSolutionWinners, preferences, satisfactionFunction, maxEnergyValue);

            var bestSolutionWinners = currentSolutionWinners;
            var bestEnergy = maxEnergyValue;

            while (_temperature > 1)
            {
                var newSolution = PerformRandomSwap(alternatives, currentSolution);
                var newSolutionWinners = AlgorithmUtils.AssignBestForMonroe(newSolution, preferences.VotersPreferences, satisfactionFunction);
                var newEnergy = GetEnergy(newSolutionWinners, preferences, satisfactionFunction, maxEnergyValue);
                if (AlgorithmUtils.AcceptanceProbability(currentEnergy, newEnergy, _temperature) > _random.NextDouble())
                {
                    currentSolution = newSolution;
                    currentSolutionWinners = newSolutionWinners;
                    currentEnergy = newEnergy;
                }
                if (currentEnergy < bestEnergy)
                {
                    bestSolutionWinners = currentSolutionWinners;
                    bestEnergy = currentEnergy;
                }
                _temperature *= 1 - _coolingRate;
            }

            var results = new Results(preferences, bestSolutionWinners, satisfactionFunction, RuleType.Monroe);
            return results;
        }

        private IList<int> PerformRandomSwap(IEnumerable<int> alternatives, ICollection<int> solution)
        {
            var toChoose = alternatives.Except(solution).ToList();
            var randomOldAlternative = _random.Next(solution.Count);
            var randomNewAlternative = _random.Next(toChoose.Count);
            var newSolution = new List<int>(solution);
            newSolution[randomOldAlternative] = toChoose[randomNewAlternative];
            return newSolution;
        }

        private UInt64 GetEnergy(IList<int> winners, Preferences preferences, IList<int> satisfactionFunction, UInt64 maxEnergy)
        {
            var results = new Results(preferences, winners, satisfactionFunction, RuleType.Monroe);
            var score = ScoreCalculator.CalculateScore(results);
            return maxEnergy - (UInt64)score;
        }
    }
}
