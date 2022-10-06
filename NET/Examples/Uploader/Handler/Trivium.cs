namespace Uploader.Handler
{
    using Breaking_Uploader.Handler;
    using System;
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Models;

    internal class Trivium : BaseHandler, IClientHandler
    {
        public void Upload(Client client, int competitionId, IEnumerable<string> data)
        {
            LoadBaseData(client, competitionId, "trivium");

            foreach (var line in data)
            {
                var fields = line.Split(';', ',', '\t').Select(d => d.Replace("\"", string.Empty)).ToArray();
                if (fields.Length < 5)
                {
                    continue;
                }


                if (string.IsNullOrEmpty(fields[4]))
                {
                    Console.Error.WriteLine("No MIN for " + fields[1]);
                    continue;
                }

                var judge = Officials.FirstOrDefault(p => p.Min.ToString() == fields[4]);
                var dancer1 = Participants.FirstOrDefault(p => p.PersonId == fields[8]);
                var dancer2 = Participants.FirstOrDefault(p => p.PersonId == fields[9]);

                if (judge == null)
                {
                    Console.Error.WriteLine(fields[4] + ": no judge");
                    continue;
                }
                if (dancer1 == null)
                {
                    Console.Error.WriteLine(fields[8] + ": no dancer 1");
                    continue;
                }
                if (dancer2 == null)
                {
                    Console.Error.WriteLine(fields[9] + ": no dancer 2");
                    continue;
                }

                switch (fields[3])
                {
                    case "RoundRobin": AddRoundRobin(fields, judge, dancer1, dancer2); break;
                    case "TOP4": AddTopX(fields, judge, dancer1, dancer2); break;
                    case "TOP8": AddTopX(fields, judge, dancer1, dancer2); break;
                    case "TOP16": AddTopX(fields, judge, dancer1, dancer2); break;
                    default: Console.Error.WriteLine("Unknown mode: " + fields[3]); break;
                }

            }

            UploadToAPI(client);
        }

        private void AddTopX(string[] fields, OfficialDetail judge, ParticipantSingleDetail dancer1, ParticipantSingleDetail dancer2)
        {
            var round = 3 - int.Parse(fields[5]);
            var danceDancer1 = FindDance(dancer1, round.ToString());
            var danceDancer2 = FindDance(dancer2, round.ToString());

            danceDancer1.Scores.Add(new TriviumScore()
            {
                Mode = "TopX",
                Branch = fields[6] == "3rd" ? 2 : int.Parse(fields[6]),
                SubRound = int.Parse(fields[7]),
                OfficialId = judge.Id,
                Technique = fields[10].StartsWith('-') ? int.Parse(fields[10]) * -1 : 0,
                Variety = fields[11].StartsWith('-') ? int.Parse(fields[11]) * -1 : 0,
                Performativity = fields[12].StartsWith('-') ? int.Parse(fields[12]) * -1 : 0,
                Musicality = fields[13].StartsWith('-') ? int.Parse(fields[13]) * -1 : 0,
                Creativity = fields[14].StartsWith('-') ? int.Parse(fields[14]) * -1 : 0,
                Personality = fields[15].StartsWith('-') ? int.Parse(fields[15]) * -1 : 0,
                Crash1 = int.Parse(fields[16]),
                Crash2 = int.Parse(fields[18]),
                Crash3 = int.Parse(fields[20]),
                Misbehaviour = int.Parse(fields[22]),
                Repeat = int.Parse(fields[24]),
                Bite = int.Parse(fields[25]),
                Spontaneity = int.Parse(fields[28]),
                Confidence = int.Parse(fields[30]),
                Execution = int.Parse(fields[32]),
                Form = int.Parse(fields[34]),
                IsRed = true,
            });
            danceDancer2.Scores.Add(new TriviumScore()
            {
                Mode = "TopX",
                Branch = fields[6] == "3rd" ? 2 : int.Parse(fields[6]),
                SubRound = int.Parse(fields[7]),
                OfficialId = judge.Id,
                Technique = fields[10].StartsWith('-') ? 0 : int.Parse(fields[10]),
                Variety = fields[11].StartsWith('-') ? 0 : int.Parse(fields[11]),
                Performativity = fields[12].StartsWith('-') ? 0 : int.Parse(fields[12]),
                Musicality = fields[13].StartsWith('-') ? 0 : int.Parse(fields[13]),
                Creativity = fields[14].StartsWith('-') ? 0 : int.Parse(fields[14]),
                Personality = fields[15].StartsWith('-') ? 0 : int.Parse(fields[15]),
                Crash1 = int.Parse(fields[17]),
                Crash2 = int.Parse(fields[19]),
                Crash3 = int.Parse(fields[21]),
                Misbehaviour = int.Parse(fields[23]),
                Repeat = int.Parse(fields[25]),
                Bite = int.Parse(fields[27]),
                Spontaneity = int.Parse(fields[29]),
                Confidence = int.Parse(fields[31]),
                Execution = int.Parse(fields[33]),
                Form = int.Parse(fields[35])
            });
        }

        private void AddRoundRobin(string[] fields, OfficialDetail judge, ParticipantSingleDetail dancer1, ParticipantSingleDetail dancer2)
        {
            var danceDancer1 = FindDance(dancer1, fields[5]);
            var danceDancer2 = FindDance(dancer2, fields[5]);

            danceDancer1.Scores.Add(new TriviumScore()
            {
                Mode = "RoundRobin",
                Branch = int.Parse(fields[6]),
                SubRound = int.Parse(fields[7]),
                OfficialId = judge.Id,
                Technique = fields[10].StartsWith('-') ? int.Parse(fields[10]) * -1 : 0,
                Variety = fields[11].StartsWith('-') ? int.Parse(fields[11]) * -1 : 0,
                Performativity = fields[12].StartsWith('-') ? int.Parse(fields[12]) * -1 : 0,
                Musicality = fields[13].StartsWith('-') ? int.Parse(fields[13]) * -1 : 0,
                Creativity = fields[14].StartsWith('-') ? int.Parse(fields[14]) * -1 : 0,
                Personality = fields[15].StartsWith('-') ? int.Parse(fields[15]) * -1 : 0,
                Crash1 = int.Parse(fields[16]),
                Crash2 = int.Parse(fields[18]),
                Crash3 = int.Parse(fields[20]),
                Misbehaviour = int.Parse(fields[22]),
                Repeat = int.Parse(fields[24]),
                Bite = int.Parse(fields[25]),
                Spontaneity = int.Parse(fields[28]),
                Confidence = int.Parse(fields[30]),
                Execution = int.Parse(fields[32]),
                Form = int.Parse(fields[34]),
                IsRed = true
            });
            danceDancer2.Scores.Add(new TriviumScore()
            {
                Mode = "RoundRobin",
                Branch = int.Parse(fields[6]),
                SubRound = int.Parse(fields[7]),
                OfficialId = judge.Id,
                Technique = fields[10].StartsWith('-') ? 0 : int.Parse(fields[10]),
                Variety = fields[11].StartsWith('-') ? 0 : int.Parse(fields[11]),
                Performativity = fields[12].StartsWith('-') ? 0 : int.Parse(fields[12]),
                Musicality = fields[13].StartsWith('-') ? 0 : int.Parse(fields[13]),
                Creativity = fields[14].StartsWith('-') ? 0 : int.Parse(fields[14]),
                Personality = fields[15].StartsWith('-') ? 0 : int.Parse(fields[15]),
                Crash1 = int.Parse(fields[17]),
                Crash2 = int.Parse(fields[19]),
                Crash3 = int.Parse(fields[21]),
                Misbehaviour = int.Parse(fields[23]),
                Repeat = int.Parse(fields[25]),
                Bite = int.Parse(fields[27]),
                Spontaneity = int.Parse(fields[29]),
                Confidence = int.Parse(fields[31]),
                Execution = int.Parse(fields[33]),
                Form = int.Parse(fields[35])
            });
        }

    }
}
