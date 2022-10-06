namespace Uploader.Handler
{
    using System;
    using System.Text;
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;

    internal class Threefold : IClientHandler
    {
        public void Upload(Client client, int competitionId, IEnumerable<string> data)
        {
            Console.WriteLine("Loading participants");
            var participants = client.GetSingleParticipants(competitionId);
            var participantDetails = new List<ParticipantSingleDetail>();
            foreach (var p in participants)
            {
                var sp = client.GetSingleParticipant(p.Id);
                // remove all threefold scores but leave others like PreSeed
                foreach(var r in sp.Rounds)
                {
                    foreach(var d in r.Dances)
                    {
                        d.Scores = d.Scores.Where(s => s.Kind != "threefold").ToList();
                    }
                }

                participantDetails.Add(sp);
            }

            Console.WriteLine("Loading official");
            var officials = client.GetOfficials(competitionId);
            var officiaDetails = new List<OfficialDetail>();
            foreach (var o in officials)
            {
                officiaDetails.Add(client.GetOfficial(o.Id));
            }

            foreach (var line in data)
            {
                var fields = line.Split(';',',','\t').Select(d => d.Replace("\"", string.Empty)).ToArray();
                if (fields.Length < 5)
                {
                    continue;
                }


                if (string.IsNullOrEmpty(fields[4]))
                {
                    Console.WriteLine("No MIN for " + fields[1]);
                    continue;
                }

                var judge = officiaDetails.FirstOrDefault(p => p.Min.ToString() == fields[4]);
                var dancer1 = participantDetails.FirstOrDefault(p => p.PersonId == fields[8]);
                var dancer2 = participantDetails.FirstOrDefault(p => p.PersonId == fields[9]);

                if (judge == null)
                {
                    Console.WriteLine(fields[4] + ": no judge");
                    continue;
                }
                if (dancer1 == null)
                {
                    Console.WriteLine(fields[8] + ": no dancer 1");
                    continue;
                }
                if (dancer2 == null)
                {
                    Console.WriteLine(fields[9] + ": no dancer 2");
                    continue;
                }

                switch (fields[3])
                {
                    case "RoundRobin": AddRoundRobin(fields, judge, dancer1, dancer2); break;
                    case "TOP4": AddTopX(fields, judge, dancer1, dancer2); break;
                    case "TOP8": AddTopX(fields, judge, dancer1, dancer2); break;
                    case "TOP16": AddTopX(fields, judge, dancer1, dancer2); break;
                    default: Console.WriteLine("Unknown mode: " + fields[3]); break;
                }

            }

            foreach (var p in participantDetails)
            {
                try
                {
                    client.UpdateSingleParticipant(p);
                }
                catch (ApiException ex)
                {
                    Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
                }
            }
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

        private Dance FindDance(ParticipantSingleDetail dancer, string roundName)
        {
            dancer.Rounds ??= new List<Round>();
            var round = dancer.Rounds.FirstOrDefault(d => d.Name == roundName);
            if (round == null)
            {
                round = new Round() { Name = roundName, Dances = new List<Dance>() };
                dancer.Rounds.Add(round);
                round.Dances.Add(new Dance() { Name = "BREAKING", Scores = new List<Score>() });
            }

            return round.Dances.First();
        }
    }
}
