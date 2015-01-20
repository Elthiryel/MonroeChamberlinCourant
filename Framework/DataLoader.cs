using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonroeChamberlinCourant.Framework.Exceptions;

namespace MonroeChamberlinCourant.Framework
{
    public class DataLoader
    {
        public static Preferences LoadPreferences(string filename)
        {
            using (var preferencesFile = new StreamReader(filename))
            {
                var numberOfCandidatesLine = preferencesFile.ReadLine();
                if (numberOfCandidatesLine == null)
                    throw new InvalidPreferencesFormatException(Literals.CannotParseNumberOfCandidates);
                var numberOfCandidates = Int32.Parse(numberOfCandidatesLine);
                var candidates = new Dictionary<int, Candidate>(numberOfCandidates);
                for (var i = 0; i < numberOfCandidates; i++)
                {
                    var candidateLine = preferencesFile.ReadLine();
                    if (candidateLine == null)
                        throw new InvalidPreferencesFormatException(String.Format(Literals.CannotParseCandidate, i));
                    var candidateLineSplitted = candidateLine.Split(',');
                    if (candidateLineSplitted.Count() < 2)
                        throw new InvalidPreferencesFormatException(String.Format(Literals.CannotParseCandidate, i));
                    var candidateId = Int32.Parse(candidateLineSplitted[0]);
                    var candidate = new Candidate(candidateId, candidateLineSplitted[1]);
                    if (candidates.ContainsKey(candidateId))
                        throw new InvalidPreferencesFormatException(Literals.DuplicateCandidateId);
                    candidates[candidateId] = candidate;
                }
                var numberOfVotersLine = preferencesFile.ReadLine();
                if (numberOfVotersLine == null)
                    throw new InvalidPreferencesFormatException(Literals.CannotParseNumberOfVoters);
                var numberOfVotersLineSplitted = numberOfVotersLine.Split(',');
                if (numberOfVotersLineSplitted.Count() < 3)
                    throw new InvalidPreferencesFormatException(Literals.CannotParseNumberOfVoters);
                var numberOfVoters = Int32.Parse(numberOfVotersLineSplitted[0]);
                var numberOfPreferences = Int32.Parse(numberOfVotersLineSplitted[2]);
                var votersPreferences = new List<IList<int>>(numberOfVoters);
                for (var i = 0; i < numberOfPreferences; i++)
                {
                    var voterPreferencesLine = preferencesFile.ReadLine();
                    if (voterPreferencesLine == null)
                        throw new InvalidPreferencesFormatException(String.Format(Literals.CannotParseVoterPreferences, i));
                    var voterPreferencesLineSplitted = voterPreferencesLine.Split(',');
                    if (voterPreferencesLineSplitted.Count() != numberOfCandidates + 1)
                        throw new InvalidPreferencesFormatException(String.Format(Literals.CannotParseVoterPreferences, i));
                    var voterPreferences = new List<int>(numberOfCandidates);
                    var count = Int32.Parse(voterPreferencesLineSplitted[0]);
                    for (var j = 1; j < voterPreferencesLineSplitted.Count(); j++)
                    {
                        voterPreferences.Add(Int32.Parse(voterPreferencesLineSplitted[j]));
                    }
                    for (var k = 0; k < count; k++)
                    {
                        votersPreferences.Add(voterPreferences);
                    }
                }
                if (votersPreferences.Count != numberOfVoters)
                    throw new InvalidPreferencesFormatException(Literals.InvalidVotersNumber);
                var preferences = new Preferences(numberOfCandidates, candidates, numberOfVoters, votersPreferences);
                return preferences;
            }
        }
    }
}
