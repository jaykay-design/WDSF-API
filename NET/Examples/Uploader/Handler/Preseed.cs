namespace Uploader.Handler
{
    using System;
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;

    internal class Preseed : IClientHandler
    {
        public void Upload(Client client, int competitionId, IEnumerable<string> data)
        {
            Console.WriteLine("Loading participants");
            var participants = client.GetSingleParticipants(competitionId);
            var participantDetails = new List<ParticipantSingleDetail>();
            foreach (var p in participants)
            {
                var sp = client.GetSingleParticipant(p.Id);
                // remove all preseed scores but leave others like Trivium
                foreach (var r in sp.Rounds)
                {
                    foreach (var d in r.Dances)
                    {
                        d.Scores = d.Scores.Where(s => s.Kind != "breakingseed").ToList();
                    }
                }
                participantDetails.Add(sp);
            }

            foreach (var line in data)
            {
                var fields = line.Split(';').Select(d => d.Replace("\"", string.Empty)).ToArray();
                if (fields.Length < 5)
                {
                    continue;
                }


                if (string.IsNullOrEmpty(fields[4]))
                {
                    Console.WriteLine("No MIN for " + fields[4]);
                    continue;
                }

                var dancer = participantDetails.FirstOrDefault(p => p.PersonId == fields[4]);

                if (dancer == null)
                {
                    Console.WriteLine(fields[4] + ": no dancer");
                    continue;
                }

                AddSeed(fields, dancer);
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

        private void AddSeed(string[] fields,ParticipantSingleDetail dancer)
        {
            var danceDancer = FindDance(dancer, "Seed");

            danceDancer.Scores.Add(new BreakingSeedScore()
            {
                Score = int.Parse(fields[5])
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
