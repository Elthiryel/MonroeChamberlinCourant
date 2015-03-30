using System.Collections.Generic;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms
{
    public delegate int SatisfactionFunction(int position);

    public interface IAlgorithm
    {
        Results Run(Preferences preferences, IList<int> satisfactionFunction);
        Results Run(Preferences preferences, SatisfactionFunction satisfactionFunction);
    }
}
