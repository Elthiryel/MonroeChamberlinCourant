using System.Collections.Generic;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms
{
    public abstract class AbstractAlgorithm : IAlgorithm
    {
        public abstract Results Run(Preferences preferences, IList<int> satisfactionFunction);

        public Results Run(Preferences preferences, SatisfactionFunction satisfactionFunction)
        {
            var numberOfCandidates = preferences.NumberOfCandidates;
            var satisfactionArray = new int[numberOfCandidates];
            for (var i = 0; i < numberOfCandidates; ++i)
                satisfactionArray[i] = satisfactionFunction(i);
            return Run(preferences, satisfactionArray);
        }
    }
}
