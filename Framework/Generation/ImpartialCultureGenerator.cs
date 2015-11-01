using System.Collections.Generic;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace MonroeChamberlinCourant.Framework.Generation
{
    public class ImpartialCultureGenerator : IPreferencesGenerator
    {
        public Preferences Generate(IDictionary<int, Candidate> candidates, int numberOfVoters)
        {
            var votersPreferences = new List<IList<int>>(numberOfVoters);

            for (var i = 0; i < numberOfVoters; i++)
            {
                var currentVotersPreferences = new List<int>(candidates.Keys);
                currentVotersPreferences.Shuffle();
                votersPreferences.Add(currentVotersPreferences);
            }

            var preferences = new Preferences(candidates.Count, candidates, numberOfVoters, votersPreferences);
            return preferences;
        }

    }
}
