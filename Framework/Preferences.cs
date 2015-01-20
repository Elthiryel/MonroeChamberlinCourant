using System.Collections.Generic;

namespace MonroeChamberlinCourant.Framework
{
    public class Preferences
    {
        public int NumberOfCandidates { get; private set; }
        public IDictionary<int, Candidate> Candidates { get; private set; }
        public int NumberOfVoters { get; private set; }
        public IList<IList<int>> VotersPreferences { get; private set; }

        public Preferences(int numberOfCandidates, IDictionary<int, Candidate> candidates, int numberOfVoters,
            IList<IList<int>> votersPreferences)
        {
            NumberOfCandidates = numberOfCandidates;
            Candidates = candidates;
            NumberOfVoters = numberOfVoters;
            VotersPreferences = votersPreferences;
        }
    }
}
