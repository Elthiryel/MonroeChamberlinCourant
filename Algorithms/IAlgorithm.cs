using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Algorithms
{
    public interface IAlgorithm
    {
        Results Run(Preferences preferences);
    }
}
