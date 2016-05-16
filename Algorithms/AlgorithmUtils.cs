using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms
{
    public class MinCostMaxFlow
    {
        private const int Inf = Int32.MaxValue / 2 - 1;

        private bool[] _found;
        private int _n;
        private double[,] _cap;
        private double[,] _flow;
        private double[,] _cost;
        private int[] _dad;
        private double[] _dist;
        private double[] _pi;

        private bool Search(int source, int sink)
        {
            for (var i = 0; i < _found.Length; ++i)
                _found[i] = false;
            for (var i = 0; i < _dist.Length; ++i)
                _dist[i] = Inf;
            _dist[source] = 0;

            while (source != _n)
            {
                var best = _n;
                _found[source] = true;
                for (var k = 0; k < _n; ++k)
                {
                    if (_found[k])
                        continue;
                    if (_flow[k, source] != 0)
                    {
                        var val = _dist[source] + _pi[source] - _pi[k] - _cost[k, source];
                        if (_dist[k] > val)
                        {
                            _dist[k] = val;
                            _dad[k] = source;
                        }
                    }
                    if (_flow[source, k] < _cap[source, k])
                    {
                        var val = _dist[source] + _pi[source] - _pi[k] + _cost[source, k];
                        if (_dist[k] > val)
                        {
                            _dist[k] = val;
                            _dad[k] = source;
                        }
                    }

                    if (_dist[k] < _dist[best])
                        best = k;
                }
                source = best;
            }
            for (var k = 0; k < _n; ++k)
            {
                var sum = _pi[k] + _dist[k];
                _pi[k] = sum < Inf ? sum : Inf;
            }
            return _found[sink];
        }

        public double[,] GetMaxFlow(double[,] cap, double[,] cost, int source, int sink)
        {
            _cap = cap;
            _cost = cost;

            _n = _cap.GetLength(0);
            _found = new bool[_n];
            _flow = new double[_n, _n];
            _dist = new double[_n + 1];
            _dad = new int[_n];
            _pi = new double[_n];

            while (Search(source, sink))
            {
                double amt = Inf;
                for (var x = sink; x != source; x = _dad[x])
                {
                    var value = _flow[x, _dad[x]] != 0 ? _flow[x, _dad[x]] : _cap[_dad[x], x] - _flow[_dad[x], x];
                    amt = amt < value ? amt : value;
                }
                for (var x = sink; x != source; x = _dad[x])
                {
                    if (_flow[x, _dad[x]] != 0)
                        _flow[x, _dad[x]] -= amt;
                    else
                        _flow[_dad[x], x] += amt;
                }
            }

            return _flow;
        }
    }

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

        public IList<int> Assignment { get; set; }
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
        private const double Diff = 0.00000000000001d;

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

        public static IList<int> AssignBestForCC(IList<int> candidates, IList<IList<int>> votersPreferences)
        {
            var votersCount = votersPreferences.Count;
            var candidatesCount = candidates.Count;
            var winners = Enumerable.Repeat(candidates[0], votersCount).ToList();
            for (var i = 1; i < candidatesCount; ++i)
            {
                for (var j = 0; j < votersCount; ++j)
                {
                    if (votersPreferences[j].IndexOf(candidates[i]) < votersPreferences[j].IndexOf(winners[j]))
                        winners[j] = candidates[i];
                }
            }
            return winners;
        } 

        public static IList<int> AssignBestForMonroe(IList<int> candidates, IList<IList<int>> votersPreferences, IList<int> satisfactionFunction, int winnersCount = -1)
        {
            var candidatesCount = candidates.Count;
            var votersCount = votersPreferences.Count;
            var source = candidatesCount + votersCount;
            var sink = source + 1;
            var totalCount = sink + 1;
            var ratio = (double) votersCount / (winnersCount > 0 ? winnersCount : candidatesCount);

            var cap = new double[totalCount, totalCount];
            var cost = new double[totalCount, totalCount];
            
            for (var i = 0; i < candidatesCount; ++i)
            {
                // source -> candidates
                cap[source, i] = ratio;
                for (var j = candidatesCount; j < candidatesCount + votersCount; ++j)
                {
                    // candidates -> voters
                    cap[i, j] = 1;
                    cost[i, j] = satisfactionFunction[0] - satisfactionFunction[votersPreferences[j - candidatesCount].IndexOf(candidates[i])];
                }
            }

            for (var j = candidatesCount; j < candidatesCount + votersCount; ++j)
            {
                // voters -> sink
                cap[j, sink] = 1;
            }

            var minCostMaxFlow = new MinCostMaxFlow();
            var maxFlow = minCostMaxFlow.GetMaxFlow(cap, cost, source, sink);

            var foundCounter = 0;
            var winners = new List<int>(votersCount);
            var values = new List<Tuple<int, int>>(); // candidate - voter
            for (var j = candidatesCount; j < candidatesCount + votersCount; ++j)
            {
                var index = j - candidatesCount;
                var found = false;
                for (var i = 0; i < candidatesCount; ++i)
                {
                    var value = maxFlow[i, j];
                    if (value > 1.0 - Diff)
                    {
                        ++foundCounter;
                        found = true;
                        winners.Add(candidates[i]);
                        break;
                    }
                    if (value > Diff)
                        values.Add(new Tuple<int, int>(i, index));
                }
                if (!found)
                    winners.Add(-1);
            }
            if (values.Count != 0)
            {
                var subsets = GetSubsets(values, votersCount - foundCounter);
                var pairs = GetBestPairs(subsets, candidates, votersPreferences, satisfactionFunction);
                foreach (var pair in pairs)
                {
                    winners[pair.Item2] = candidates[pair.Item1];
                }
            }
            return winners;
        }

        private static IEnumerable<Tuple<int, int>> GetBestPairs(IEnumerable<ISet<Tuple<int, int>>> subsets, IList<int> candidates, IList<IList<int>> votersPreferences, IList<int> satisfactionFunction)
        {
            var bestScore = -1;
            ISet<Tuple<int, int>> bestSubset = null;
            foreach (var subset in subsets)
            {
                var existingCandidates = new LinkedList<int>();
                var existingVoters = new LinkedList<int>();
                var recurring = false;
                var score = 0;
                foreach (var pair in subset)
                {
                    var candidate = pair.Item1;
                    var voter = pair.Item2;
                    if (existingCandidates.Contains(candidate) || existingVoters.Contains(voter))
                    {
                        recurring = true;
                        break;
                    }
                    existingCandidates.AddLast(candidate);
                    existingVoters.AddLast(voter);
                    score += satisfactionFunction[votersPreferences[voter].IndexOf(candidates[candidate])];
                }
                if (recurring)
                    continue;
                if (score > bestScore)
                {
                    bestScore = score;
                    bestSubset = subset;
                }
            }
            return bestSubset;
        }

        public static IEnumerable<ISet<T>> GetSubsets<T>(IList<T> superSet, int k)
        {
            var res = new List<ISet<T>>();
            GetSubsets(superSet, k, 0, new HashSet<T>(), res);
            return res;
        }

        private static void GetSubsets<T>(IList<T> superSet, int k, int idx, ISet<T> current, IList<ISet<T>> solution)
        {
            // successful stop clause
            if (current.Count == k)
            {
                solution.Add(new HashSet<T>(current));
                return;
            }

            // unsuccessful stop clause
            if (idx == superSet.Count)
                return;
            var x = superSet[idx];
            current.Add(x);

            // "guess" x is in the subset
            GetSubsets(superSet, k, idx + 1, current, solution);
            current.Remove(x);

            // "guess" x is not in the subset
            GetSubsets(superSet, k, idx + 1, current, solution);
        }

        public static double LambertsFunction(double x)
        {
            var lower = 0.0d;
            var upper = 5.0d;
            while (true)
            {
                var mid = (lower + upper) / 2;
                bool greater;
                var result = ValidateLambertsFunction(x, mid, out greater);
                if (result)
                    return mid;
                if (greater)
                    lower = mid;
                else
                    upper = mid;
            }
        }

        private static bool ValidateLambertsFunction(double x, double w, out bool greater)
        {
            const double diff = 0.05d;
            var computedValue = w * Math.Pow(Math.E, w);
            greater = x > computedValue;
            return Math.Abs(computedValue - x) < diff;
        }

        public static double AcceptanceProbability(UInt64 energy, UInt64 newEnergy, double temperature)
        {
            return newEnergy < energy ? 1.0d : Math.Exp((energy - newEnergy) / temperature);
        }
    }
}
