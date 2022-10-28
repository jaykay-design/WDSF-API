namespace Uploader.Handler
{
    using Breaking_Uploader.Handler;
    using System;
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Models;

    internal class Threefold : BaseHandler, IClientHandler
    {
        private Model.Trivium data;
        private readonly Client client;
        private readonly int competitionId;

        public Threefold()
        {
            this.data = data;
            this.client = client;
            this.competitionId = competitionId;
        }

        public void Upload(Client client, int competitionId, IEnumerable<string> data)
        {
            LoadBaseData(client, competitionId, "threefold");

            foreach (var line in data)
            {
                var fields = line.Split(';',',','\t').Select(d => d.Replace("\"", string.Empty)).ToArray();
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

            danceDancer1.Scores.Add(new ThreeFoldScore()
            {
                Mode = "TopX",
                Branch = fields[6] == "3rd" ? 2 : int.Parse(fields[6]),
                SubRound = int.Parse(fields[7]),
                OfficialId = judge.Id,
                Artistic = fields[10].StartsWith('-') ? int.Parse(fields[10]) * -1 : 0,
                Interpretation = fields[11].StartsWith('-') ? int.Parse(fields[11]) * -1 : 0,
                Physical = fields[12].StartsWith('-') ? int.Parse(fields[12]) * -1 : 0,
                IsRed = true,
            });
            danceDancer2.Scores.Add(new ThreeFoldScore()
            {
                Mode = "TopX",
                Branch = fields[6] == "3rd" ? 2 : int.Parse(fields[6]),
                SubRound = int.Parse(fields[7]),
                OfficialId = judge.Id,
                Artistic = fields[10].StartsWith('-') ? int.Parse(fields[10]) * -1 : 0,
                Interpretation = fields[11].StartsWith('-') ? int.Parse(fields[11]) * -1 : 0,
                Physical = fields[12].StartsWith('-') ? int.Parse(fields[12]) * -1 : 0,
            });
        }

        private void AddRoundRobin(string[] fields, OfficialDetail judge, ParticipantSingleDetail dancer1, ParticipantSingleDetail dancer2)
        {
            var danceDancer1 = FindDance(dancer1, fields[5]);
            var danceDancer2 = FindDance(dancer2, fields[5]);

            danceDancer1.Scores.Add(new ThreeFoldScore()
            {
                Mode = "RoundRobin",
                Branch = int.Parse(fields[6]),
                SubRound = int.Parse(fields[7]),
                OfficialId = judge.Id,
                Artistic = fields[8].StartsWith('-') ? int.Parse(fields[10]) * -1 : 0,
                Interpretation = fields[10].StartsWith('-') ? int.Parse(fields[11]) * -1 : 0,
                Physical = fields[12].StartsWith('-') ? int.Parse(fields[12]) * -1 : 0,
                IsRed = true
            });
            danceDancer2.Scores.Add(new ThreeFoldScore()
            {
                Mode = "RoundRobin",
                Branch = int.Parse(fields[6]),
                SubRound = int.Parse(fields[7]),
                OfficialId = judge.Id,
                Artistic = fields[9].StartsWith('-') ? int.Parse(fields[10]) * -1 : 0,
                Interpretation = fields[11].StartsWith('-') ? int.Parse(fields[11]) * -1 : 0,
                Physical = fields[13].StartsWith('-') ? int.Parse(fields[12]) * -1 : 0,
            });
        }

    }
}
