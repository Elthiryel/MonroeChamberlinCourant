using System;
using System.IO;
using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Framework.Data
{
    public class DataPersister
    {
        public static void SavePreferences(Preferences preferences, string filename)
        {
            using (var preferencesFile = new StreamWriter(filename))
            {
                preferencesFile.WriteLine(preferences.NumberOfCandidates);
                foreach (var candidate in preferences.Candidates)
                {
                    preferencesFile.WriteLine("{0},{1}", candidate.Key, candidate.Value);
                }
                preferencesFile.WriteLine("{0},{1},{2}", preferences.NumberOfVoters, preferences.NumberOfVoters, preferences.NumberOfVoters);
                foreach (var singlePreferences in preferences.VotersPreferences)
                {
                    var singlePreferencesString = singlePreferences.
                        Aggregate(String.Empty, (current, preference) => current + (preference + ",")).TrimEnd(',');
                    preferencesFile.WriteLine("{0},{1}", 1, singlePreferencesString);
                }
                preferencesFile.WriteLine();
            }
        }
    }
}
