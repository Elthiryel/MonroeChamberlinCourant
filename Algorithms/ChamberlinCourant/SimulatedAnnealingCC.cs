using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Algorithms.ChamberlinCourant
{
    public class SimulatedAnnealingCC : AbstractAlgorithm
    {
        private readonly double _coolingRate;
        private readonly Random _random;

        private double _temperature;

        public SimulatedAnnealingCC(double temperature, double coolingRate)
        {
            _temperature = temperature;
            _coolingRate = coolingRate;
            _random = new Random();
        }

        public override Results Run(Preferences preferences, int winnersCount, IList<int> satisfactionFunction)
        {
            var maxEnergyValue = (UInt64) satisfactionFunction[0] * (UInt64) preferences.NumberOfVoters;
            var alternatives = new List<int>(preferences.Candidates.Keys);

            var currentSolution = AlgorithmUtils.GetRandomAlternatives(alternatives, winnersCount, _random);
            var currentSolutionWinners = AlgorithmUtils.AssignBestForCC(currentSolution, preferences.VotersPreferences);
            var currentEnergy = GetEnergy(currentSolutionWinners, preferences, satisfactionFunction, maxEnergyValue);

            var bestSolutionWinners = currentSolutionWinners;
            var bestEnergy = maxEnergyValue;

            var i = 0; // TODO remove iter variable

            while (_temperature > 1)
            {
                ++i; // TODO remove
                if (i % 100 == 1)
                    Console.WriteLine("Simulated annealing iteration {0}, energy = {1}", i, bestEnergy);

                var newSolution = PerformRandomSwap(alternatives, currentSolution);
                var newSolutionWinners = AlgorithmUtils.AssignBestForCC(newSolution, preferences.VotersPreferences);
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

            var results = new Results(preferences, bestSolutionWinners, satisfactionFunction, RuleType.ChamberlinCourant);
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
            var results = new Results(preferences, winners, satisfactionFunction, RuleType.ChamberlinCourant);
            var score = ScoreCalculator.CalculateScore(results);
            return maxEnergy - (UInt64) score;
        }
    }
}
