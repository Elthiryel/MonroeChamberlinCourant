using MonroeChamberlinCourant.Framework.Model;

namespace MonroeChamberlinCourant.Framework.Utils
{
    public class ScoreCalculator
    {
        public static int CalculateScore(Results results)
        {
            var result = 0;
            for (var i = 0; i < results.Winners.Count; ++i)
            {
                var winner = results.Winners[i];
                if (winner >= 0)
                {
                    result += results.SatisfactionFunction[results.Preferences.VotersPreferences[i].IndexOf(winner)];
                }
            }
            return result;
        }
    }
}
