namespace Uploader.Handler
{
    using Breaking_Uploader.Handler;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using Uploader.Model;
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Models;

    internal class Trivium : BaseHandler, IClientHandler
    {
        private Model.Trivium data;
        private readonly Client client;
        private readonly int competitionId;

        public Trivium(Model.Trivium data, Client client, int competitionId)
        {
            this.data = data;
            this.client = client;
            this.competitionId = competitionId;
        }

        public void Upload()
        {
            foreach (var phase in data.Phases)
            {
                if (phase.Rankings != null)
                {
                    foreach (var rank in phase.Rankings)
                    {
                        rank.Min = rank.Min.Trim();
                    }
                }
                if (phase.RankingForEachGroup != null)
                {
                    foreach (var group in phase.RankingForEachGroup)
                    {
                        foreach (var rank in group)
                        {
                            rank.Min = rank.Min.Trim();
                        }
                    }
                }

                foreach (var battle in phase.Battles)
                {
                    if (battle.RedPlayer != null) battle.RedPlayer.Min = battle.RedPlayer.Min.Trim();
                    if (battle.BluePlayer != null) battle.BluePlayer.Min = battle.BluePlayer.Min.Trim();
                    if (battle.SoloPlayer != null) battle.SoloPlayer.Min = battle.SoloPlayer.Min.Trim();
                }
            }

            LoadBaseData(client, competitionId);

            var allMIN = data.Phases.Where(p=> p.Rankings != null).SelectMany(p => p.Rankings.Select(r => r.Min)).Distinct();
            LoadMissingAthleteMIN(client, competitionId, allMIN);

            foreach(var p in Participants)
            {
                p.Rank = "0";
            }

            foreach (var r in data.FinalRankings)
            {
                var p = Participants.First(p => p.PersonId == r.Min.Trim());
                if (r.IsQuit)
                {
                    p.Status = "Excused";
                }
                else
                {
                    p.Status = "Present";
                    p.Rank = r.RankIndex.ToString();
                }
            }

            int round = 1;
            foreach (var phase in data.Phases)
            {
                switch (phase.PhaseName)
                {
                    case "Preselection": LoadPreselection(phase.Battles); break;
                    case "Pre-qualifier Top 128": LoadPreQualifier(phase.Battles, round++); break;
                    case "Pre-qualifier Top 64": LoadPreQualifier(phase.Battles, round++); break;
                    case "Pre-qualifier Top 32": LoadPreQualifier(phase.Battles, round++); break;
                    case "Round Robin": LoadRoundRobin(phase); break;
                    case "Knockout": LoadKnockout(phase.Battles); break;
                }
            }

            UploadToAPI(client);
        }

        private void LoadRanks(Model.Ranking[] rankings)
        {
            foreach (var rank in rankings)
            {
                var p = this.Participants.FirstOrDefault(p => p.PersonId == rank.Min.Trim() && p.Rank == string.Empty);
                if (p != null)
                {
                    p.Rank = rank.RankIndex.ToString();
                }
            }
        }


        private void LoadPreselection(Battle[] battles)
        {
            var scoresByJudge = new Dictionary<string, List<decimal>>();
            var judges = battles.SelectMany( b=> b.SoloPlayer.ScoreDetails.Select(d=> d.Judge.Min)).Distinct();
            foreach (var judge in judges)
            {
                var s = new List<decimal>();
                foreach (var b in battles)
                {
                    if (!b.SoloPlayer.ScoreDetails.Any(s => s.Judge.Min == judge)) continue;
                    s.Add(b.SoloPlayer.ScoreDetails.First(sd => sd.Judge.Min == judge).Score);
                }
                scoresByJudge.Add(judge, s);
            }


            foreach (var battle in battles)
            {
                var dancer = Participants.FirstOrDefault(p => p.PersonId == battle.SoloPlayer.Min);
                if (dancer == null)
                {
                    Console.Error.WriteLine(battle.SoloPlayer.Min + ": no dancer => " + battle.SoloPlayer.Name);
                    continue;
                }

                var dance = FindDance(dancer, "PreSeed");
                foreach (var score in battle.SoloPlayer.ScoreDetails) {

                    var official = Officials.FirstOrDefault(p => p.Min.ToString() == score.Judge.Min);
                    if (official == null)
                    {
                        Console.Error.WriteLine(score.Judge.Min + ": no official => " + score.Judge.Name);
                        continue;
                    }

                    dance.Scores.Add(new BreakingSeedScore()
                    {
                        OfficialId = official.Id,
                        Rank = scoresByJudge[score.Judge.Min].Count(s => s > score.Score) + 1
                    });
                }
            }
        }

        private void LoadPreQualifier(Battle[] battles, int round)
        {
            int branch = 1;
            foreach (var battle in battles)
            {
                var dancer1 = Participants.FirstOrDefault(p => p.PersonId == battle.RedPlayer.Min);
                if (dancer1 == null)
                {
                    Console.Error.WriteLine(battle.RedPlayer.Min + ": no dancer => " + battle.RedPlayer.Name);
                    continue;
                }
                var dancer2 = Participants.FirstOrDefault(p => p.PersonId == battle.BluePlayer.Min);
                if (dancer2 == null)
                {
                    Console.Error.WriteLine(battle.BluePlayer.Min + ": no dancer => " + battle.BluePlayer.Name);
                    continue;
                }

                foreach (var dancer in new[] { dancer1, dancer2 })
                {
                    var dance = FindDance(dancer, "PreQ" + round);
                    var scores = dancer.PersonId == battle.RedPlayer.Min
                        ? battle.RedPlayer.ScoreDetails
                        : battle.BluePlayer.ScoreDetails;
                    AddTriviumScores(scores, dance, dancer, battle, "Preliminary", branch);
                }
                branch++;
            }
        }

        private void LoadRoundRobin(Phase phase)
        {
            var groupNames = new[] { "A", "B", "C", "D" };
            var battlesInGroups = new Dictionary<string, IEnumerable<Battle>>();
            char round = 'A';

            for (var groupIndex = 0; groupIndex < groupNames.Length; groupIndex++)
            {
                var minsInThisGroup = phase.RankingForEachGroup[groupIndex].Select(r => r.Min).ToList();
                battlesInGroups.Add(
                    round.ToString(),
                    phase.Battles.Where(b =>
                    minsInThisGroup.Contains(b.RedPlayer.Min)
                    || minsInThisGroup.Contains(b.BluePlayer.Min))
                    );
                round++;
            }

            foreach (var group in battlesInGroups.Keys)
            {
                var battles = battlesInGroups[group];

                int branch = 1;
                foreach (var battle in battles)
                {
                    var dancer1 = Participants.FirstOrDefault(p => p.PersonId == battle.RedPlayer.Min);

                    if (dancer1 == null)
                    {
                        Console.Error.WriteLine(battle.RedPlayer.Min + ": no dancer => " + battle.RedPlayer.Name);
                        continue;
                    }
                    var dancer2 = Participants.FirstOrDefault(p => p.PersonId == battle.BluePlayer.Min);
                    if (dancer2 == null)
                    {
                        Console.Error.WriteLine(battle.BluePlayer.Min + ": no dancer => " + battle.BluePlayer.Name);
                        continue;
                    }

                    foreach (var dancer in new[] { dancer1, dancer2 })
                    {
                        var dance = FindDance(dancer, group);
                        var scores = dancer.PersonId == battle.RedPlayer.Min
                            ? battle.RedPlayer.ScoreDetails
                            : battle.BluePlayer.ScoreDetails;

                        AddTriviumScores(scores, dance, dancer, battle, "RoundRobin", branch);
                    }
                    branch++;
                }

            }
        }

        private void LoadKnockout(Battle[] battles)
        {
            var allLayers = battles.Select(b => b.LayerIndex).Distinct().OrderByDescending(l => l);
            var roundMap = new Dictionary<int, int>();

            var roundIndex = 1;
            foreach (var layer in allLayers) roundMap.Add(layer, roundIndex++);

            foreach (var battle in battles)
            {
                var dancer1 = Participants.FirstOrDefault(p => p.PersonId == battle.RedPlayer.Min);
                if (dancer1 == null)
                {
                    Console.Error.WriteLine(battle.RedPlayer.Min + ": no dancer => " + battle.RedPlayer.Name);
                    continue;
                }
                var dancer2 = Participants.FirstOrDefault(p => p.PersonId == battle.BluePlayer.Min);
                if (dancer2 == null)
                {
                    Console.Error.WriteLine(battle.BluePlayer.Min + ": no dancer => " + battle.BluePlayer.Name);
                    continue;
                }

                var layerIndex = battle.LayerIndex == 1 ? 2 : battle.LayerIndex;
                var branch = layerIndex / 2 * battle.BatchIndex + battle.EntranceIndex + 1;
                foreach (var dancer in new[] { dancer1, dancer2 })
                {
                    // increase round for each battle section
                    var dance = FindDance(dancer, roundMap[battle.LayerIndex].ToString());
                    var scores = dancer.PersonId == battle.RedPlayer.Min
                        ? battle.RedPlayer.ScoreDetails
                        : battle.BluePlayer.ScoreDetails;
                    AddTriviumScores(scores, dance, dancer, battle, "TopX", branch);
                }

            }
        }

        private List<TriviumScore> AddTriviumScores(ScoredetailTrivium[] scores, Dance dance, ParticipantSingleDetail dancer, Battle battle, string mode, int branch)
        {
            var added = new List<TriviumScore>();
            foreach (var score in scores)
            {
                foreach (var rating in score.Ratings)
                {
                    var official = Officials.FirstOrDefault(p => p.Min.ToString() == rating.Judge.Min);
                    if (official == null)
                    {
                        Console.Error.WriteLine(rating.Judge.Min + ": no official => " + rating.Judge.Name);
                        continue;
                    }
                    var s = new TriviumScore()
                    {
                        Mode = mode,
                        SubRound = score.RoundIndex + 1,
                        Branch = branch,
                        IsRed = dancer.PersonId == battle.RedPlayer.Min,
                        OfficialId = official.Id,
                        Technique = GetScore("technique", rating.Scores),
                        Variety = GetScore("variety", rating.Scores),
                        Creativity = GetScore("creativity", rating.Scores),
                        Personality = GetScore("personality", rating.Scores),
                        Performativity = GetScore("performance", rating.Scores),
                        Musicality = GetScore("musicality", rating.Scores),
                        Crash1 = GetCount("Slip", rating.Scores),
                        Crash2 = GetCount("Crash", rating.Scores),
                        Crash3 = GetCount("Whipeout", rating.Scores),
                        Bite = Math.Abs(GetCount("Bite", rating.Scores)),
                        Misbehaviour = Math.Abs(GetCount("Misbehavior", rating.Scores)),
                        Spontaneity = GetCount("Spontaneity", rating.Scores),
                        Confidence = GetCount("Confidence", rating.Scores),
                        Execution = GetCount("Execution", rating.Scores),
                        Form = GetCount("Form", rating.Scores),
                        Repeat = GetCount("Repeat", rating.Scores),
                    };
                    dance.Scores.Add(s) ;
                    added.Add(s);
                }
            }

            return added;
        }

        private static int GetCount(string name, Model.Score[] scores)
        {
            return (int)(scores.FirstOrDefault(r => r.Name == name)?.Value ?? 0);
        }

        private static int GetScore(string name, Model.Score[] scores)
        {
            var factors = new Dictionary<string, decimal>()
                {
                    {"technique", 5m },
                    {"variety", 7.5m },
                    {"performance", 5m },
                    {"musicality", 7.5m },
                    {"creativity", 5m },
                    {"personality", 7.5m },
                };
            return (int)(scores.First(r => r.Name == name).Value * factors[name]);
        }


    }
}
