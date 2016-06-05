using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using MonroeChamberlinCourant.Algorithms;
using MonroeChamberlinCourant.Framework.Data;
using MonroeChamberlinCourant.Framework.Model;
using MonroeChamberlinCourant.Framework.Utils;

namespace Execution
{
    public class Executor
    {
        public static void RunAlgorithms(IEnumerable<IAlgorithm> algorithms, string dataFilePattern, int first, int last, 
            IList<int> satisfactionFunction, int winnersCount, string logFilePath, string labelPart, int timeout)
        {
            using (var logFile = new StreamWriter(logFilePath))
            {
                foreach (var algorithm in algorithms)
                {
                    var label = string.Format("{0} - {1}", labelPart, algorithm.GetType());
                    RunAlgorithm(algorithm, dataFilePattern, first, last, satisfactionFunction, winnersCount, logFile,
                        label, timeout);
                }
            }
        }

        private static void RunAlgorithm(IAlgorithm algorithm, string dataFilePattern, int first, int last, IList<int> satisfactionFunction, 
            int winnersCount, TextWriter logFile, string labelPart, int timeout)
        {
            for (var i = first; i <= last; ++i)
            {
                var filename = string.Format(dataFilePattern, i);
                var preferences = DataLoader.LoadPreferences(filename);
                var label = string.Format("{0} - {1} - {2}", labelPart, filename, i);
                RunAlgorithm(algorithm, preferences, satisfactionFunction, winnersCount, logFile, label, timeout);
            }
        }

        private static void RunAlgorithm(IAlgorithm algorithm, Preferences preferences, IList<int> satisfactionFunction, int winnersCount, TextWriter logFile, string label, int timeout)
        {
            var timestamp = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            logFile.WriteLine(timestamp);
            logFile.WriteLine(label);
            Results results = null;
            Stopwatch stopwatch = null;
            var task = Task.Run(() =>
            {
                stopwatch = Stopwatch.StartNew();
                results = algorithm.Run(preferences, winnersCount, satisfactionFunction);
            });
            if (task.Wait(TimeSpan.FromSeconds(timeout)))
            {
                stopwatch.Stop();
                var score = ScoreCalculator.CalculateScore(results);
                logFile.WriteLine("Time taken: {0}ms", stopwatch.Elapsed.TotalMilliseconds);
                logFile.WriteLine("Score: {0}", score);
                logFile.WriteLine();
            }
            else
            {
                logFile.WriteLine("Timeout (limit = {0}s)", timeout);
            }
        }
    }
}
