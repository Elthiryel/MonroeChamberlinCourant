using System;
using System.Collections.Generic;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Framework.Generation
{
    public class PolyaStrictGenerator : IPreferencesGenerator
    {
        public Preferences Generate(IDictionary<int, Candidate> candidates, int numberOfVoters)
        {
            var random = new Random();

            var newCandidates = new List<Candidate>(candidates.Values);
            var votersPreferences = new List<IList<int>>(numberOfVoters);

            for (var i = 0; i < numberOfVoters; i++)
            {
                List<Candidate>[] newCandidatesCopy = {new List<Candidate>(newCandidates)};
                var result = new List<int>(newCandidatesCopy[0].Count);
                foreach (var c in candidates.Select(t => newCandidatesCopy[0][random.Next(newCandidatesCopy[0].Count)]))
                {
                    result.Add(c.Id);
                    var tmp = newCandidatesCopy[0].Where(x => x != c).ToList();
                    newCandidatesCopy[0] = tmp;
                    for (var k = 0; k < candidates.Count - result.Count; ++k)
                        newCandidates.Add(c);
                }
                votersPreferences.Add(result);
            }

            var preferences = new Preferences(candidates.Count, candidates, numberOfVoters, votersPreferences);
            return preferences;
        }
    }
}
