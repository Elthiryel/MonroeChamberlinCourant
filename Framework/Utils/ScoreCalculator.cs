using System.Linq;
using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Framework.Utils
{
    public class ScoreCalculator
    {
        public static int CalculateScore(Results results)
        {
            return results.Winners.Select((winner, i) => results.Preferences.VotersPreferences[i].IndexOf(winner))
                .Sum(position => results.SatisfactionFunction[position]);
        }
    }
}
