namespace Uploader.Handler
{
    using Breaking_Uploader.Handler;
    using System;
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Models;

    internal class Preseed : BaseHandler, IClientHandler
    {
        private Model.Trivium data;
        private readonly Client client;
        private readonly int competitionId;

        public Preseed(Model.Trivium data, Client client, int competitionId)
        {
            this.data = data;
            this.client = client;
            this.competitionId = competitionId;
        }

        public void Upload()
        {
            LoadBaseData(client, competitionId);

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

                var dancer = Participants.FirstOrDefault(p => p.PersonId == fields[4]);

                if (dancer == null)
                {
                    Console.WriteLine(fields[4] + ": no dancer");
                    continue;
                }

                AddSeed(fields, dancer);
            }

            UploadToAPI(client);
        }

        private void AddSeed(string[] fields, ParticipantSingleDetail dancer)
        {
            var danceDancer = FindDance(dancer, "Seed");

            danceDancer.Scores.Add(new BreakingSeedScore()
            {
                Score = int.Parse(fields[5])
            });
        }
    }
}
