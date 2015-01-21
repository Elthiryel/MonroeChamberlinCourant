using System.Collections.Generic;
using System.Linq;

namespace MonroeChamberlinCourant.Framework.Model
{
    public enum RuleType
    {
        Monroe, ChamberlinCourant
    }

    public class Results
    {
        public Preferences Preferences { get; private set; }
        public IEnumerable<int> Winners { get; private set; }
        public RuleType Type { get; private set; }

        public Results(Preferences preferences, IEnumerable<int> winners, RuleType type)
        {
            Preferences = preferences;
            Winners = winners;
            Type = type;
        }

        public List<int> WinnersSet()
        {
            return Winners.Distinct().ToList();
        }

        public List<Candidate> WinnersAsCandidates()
        {
            return Winners.Select(winner => Preferences.Candidates[winner]).ToList();
        }

        public List<Candidate> WinnersSetAsCandidates()
        {
            return Winners.Distinct().Select(winner => Preferences.Candidates[winner]).ToList();
        } 
    }
}
