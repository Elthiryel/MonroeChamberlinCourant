using System.Collections.Generic;
using MonroeChamberlinCourant.Framework.Data;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Framework.Generation
{
    public class GenerationHelper
    {
        public static Preferences GenerateData(IPreferencesGenerator generator, int numberOfCandidates, int numberOfVoters)
        {
            var candidates = new Dictionary<int, Candidate>();
            for (var i = 0; i < numberOfCandidates; ++i)
            {
                var candidate = new Candidate(i);
                candidates[i] = candidate;
            }
            return generator.Generate(candidates, numberOfVoters);
        }

        public static Preferences GenerateAndPersistData(IPreferencesGenerator generator, int numberOfCandidates,
            int numberOfVoters, string filename)
        {
            var preferences = GenerateData(generator, numberOfCandidates, numberOfVoters);
            DataPersister.SavePreferences(preferences, filename);
            return preferences;
        }
    }
}
