using System.Collections.Generic;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Framework.Generation
{
    public interface IPreferencesGenerator
    {
        Preferences Generate(IDictionary<int, Candidate> candidates, int numberOfVoters);
    }
}
