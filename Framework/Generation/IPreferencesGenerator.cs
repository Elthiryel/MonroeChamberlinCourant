using System.Collections.Generic;

namespace MonroeChamberlinCourant.Framework.Generation
{
    public interface IPreferencesGenerator
    {
        Preferences Generate(IDictionary<int, Candidate> candidates, int numberOfVoters);
    }
}
