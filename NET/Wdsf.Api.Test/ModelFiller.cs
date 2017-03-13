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

            var adjudicators = officials.Where(o => o.official.Task == "Adjudicator");
            var chairman = officials.FirstOrDefault(o => o.official.Task == "Chairman");
            Random rand = new Random();
            for (int roundIndex = 1; roundIndex <= rounds; roundIndex++)
            {
                Round round = new Round() { Name = roundIndex == maxRounds ? "F" : roundIndex.ToString() };
                couple.Rounds.Add(round);
                for (int danceIndex = 0; danceIndex < 5; danceIndex++)
                {
                    Dance dance = new Dance() { Name = danceNames[danceIndex] };
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
                                            Rank = rand.Next(1, 6)
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
                        case "OnScale 3":
                            {
                                if (roundIndex == maxRounds)
                                {
                                    bool isGroupDance = rand.Next(0, 10) >= 5;
                                    dance.IsGroupDance = isGroupDance;

                                    if (isGroupDance)
                                    {
                                        foreach (var adj in adjudicators)
                                        {
                                            int scoreCount = 0;
                                            dance.Scores.Add(new OnScale3Score()
                                            {
                                                OfficialId = adj.official.Id,
                                                CP = rand.Next(0, 10) >= 5 && scoreCount++ == 0 ? rand.Next(5, 10) : 0,
                                                MM = rand.Next(0, 10) >= 5 && scoreCount++ == 0 ? rand.Next(5, 10) : 0,
                                                PS = rand.Next(0, 10) >= 5 && scoreCount++ == 0 ? rand.Next(5, 10) : 0,
                                                TQ = scoreCount == 0 ? rand.Next(5, 10) : 0
                                            });
                                        }
                                    }
                                    else
                                    {
                                        foreach (var adj in adjudicators)
                                        {
                                            int scoreCount = 0;
                                            dance.Scores.Add(new OnScale3Score()
                                            {
                                                OfficialId = adj.official.Id,
                                                CP = rand.Next(0, 10) >= 5 && scoreCount++ < 2 ? rand.Next(5, 10) : 0,
                                                MM = rand.Next(0, 10) >= 5 && scoreCount++ < 2 ? rand.Next(5, 10) : 0,
                                                PS = scoreCount++ < 2 ? rand.Next(5, 10) : 0,
                                                TQ = scoreCount < 2 ? rand.Next(5, 10) : 0
                                            });
                                        }
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
                    }
                }
            }
        }
    }
}
