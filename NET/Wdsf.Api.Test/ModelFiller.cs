using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wdsf.Api.Client.Models;

namespace Wdsf.Api.Test
{
    internal static class ModelFiller
    {
        static string[] danceNames = new string[] { "SAMBA", "RUMBA", "CHA CHA CHA", "PASO DOBLE", "TANGO" };
        internal static void Fill(ParticipantCoupleDetail couple, string scoreType, int rounds, int maxRounds, IEnumerable<OfficialWrapper> officials)
        {
            if (string.IsNullOrEmpty(scoreType))
            {
                return;
            }

            var adjudicators = officials.Where(o=> o.official.Task == "Adjudicator");
            var chairman = officials.FirstOrDefault(o=> o.official.Task == "Chairman");
            Random rand = new Random();
            if (scoreType == "Scating")
            {
                for (int roundIndex = 1; roundIndex <= rounds; roundIndex++)
                {
                    Round round = new Round() { Name = roundIndex == maxRounds ? "F" : roundIndex.ToString() };
                    couple.Rounds.Add(round);
                    for (int danceIndex = 0; danceIndex < 5; danceIndex++)
                    {
                        Dance dance = new Dance(){ Name = danceNames[danceIndex]};
                        round.Dances.Add(dance);
                        switch (scoreType)
                        {
                            case "Scating":
                                {
                                    if (roundIndex == maxRounds)
                                    {
                                        foreach (var adj in adjudicators)
                                        {
                                            dance.Scores.Add(new FinalScore()
                                            {
                                                OfficialId = adj.official.Id,
                                                Rank = rand.Next(1,6)
                                            });
                                        }
                                    }
                                    else
                                    {
                                        foreach (var adj in adjudicators)
                                        {
                                            if (rand.Next(0, 100) > 50)
                                            {
                                                dance.Scores.Add(new MarkScore()
                                                {
                                                    OfficialId = adj.official.Id
                                                });
                                            }
                                        }
                                    }

                                    break;
                                }
                            case "OnScale 1":
                                {

                                    break;
                                }
                            case "OnScale 2":
                                {

                                    break;
                                }
                        }
                    }
                }
            }

        }
    }
}
