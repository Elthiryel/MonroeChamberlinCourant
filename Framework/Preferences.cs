using System.Collections.Generic;
using System.Text;

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

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var candidate in Candidates.Values)
            {
                builder.Append(candidate.Id).Append(": ").Append(candidate.Name).AppendLine();
            }
            foreach (var voterPreferences in VotersPreferences)
            {
                foreach (var id in voterPreferences)
                {
                    builder.Append(id).Append(" ");
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
