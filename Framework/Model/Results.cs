using System.Collections.Generic;
using System.Linq;

namespace MonroeChamberlinCourant.Framework.Model
{
    public enum RuleType
    {
        Monroe, ChamberlinCourant
    }

    public delegate int SatisfactionFunction(int position);

    public class Results
    {
        public Preferences Preferences { get; private set; }
        public IList<int> Winners { get; private set; }
        public IList<int> SatisfactionFunction { get; private set; }
        public RuleType Type { get; private set; }
        public int? Score { get; set; }

        public Results(Preferences preferences, IList<int> winners, IList<int> satisfactionFunction, RuleType type)
        {
            Preferences = preferences;
            Winners = winners;
            SatisfactionFunction = satisfactionFunction;
            Type = type;
            Score = null;
        }

        public IList<int> WinnersSet()
        {
            return Winners.Distinct().ToList();
        }

        public IList<Candidate> WinnersAsCandidates()
        {
            return Winners.Select(winner => Preferences.Candidates[winner]).ToList();
        }

        public IList<Candidate> WinnersSetAsCandidates()
        {
            return Winners.Distinct().Select(winner => Preferences.Candidates[winner]).ToList();
        } 
    }
}
