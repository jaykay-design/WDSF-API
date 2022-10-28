using Wdsf.Api.Client;
using Wdsf.Api.Client.Exceptions;
using Wdsf.Api.Client.Models;

namespace Breaking_Uploader.Handler
{
    using System.Collections.Concurrent;
    internal class BaseHandler
    {
        protected IList<ParticipantSingleDetail> Participants { get;  set; }
        protected IList<OfficialDetail> Officials { get; set; }

        protected void LoadBaseData(Client client, int competitionId)
        {
            Console.Write("Loading participants ");
            var participants = client.GetSingleParticipants(competitionId);
            var participantDetails = new ConcurrentBag<ParticipantSingleDetail>();
            Parallel.ForEach(participants, new ParallelOptions() { MaxDegreeOfParallelism = 5 }, p =>
            {
                var sp = client.GetSingleParticipant(p.Id);
                sp.Rounds = new List<Round>();
                participantDetails.Add(sp);
                Console.Write(".");
            });

            Console.WriteLine();

            Console.Write("Loading officials ");
            var officials = client.GetOfficials(competitionId);
            var officiaDetails = new ConcurrentBag<OfficialDetail>();
            Parallel.ForEach(officials, new ParallelOptions() { MaxDegreeOfParallelism = 5 }, o =>
            {
                officiaDetails.Add(client.GetOfficial(o.Id));
                Console.Write(".");
            });
            Console.WriteLine();


            this.Participants = participantDetails.ToList();
            this.Officials = officiaDetails.ToList();
        }

        protected void LoadMissingAthleteMIN(Client client, int competitionId, IEnumerable<string> allMIN)
        {
            var minToLoad = allMIN.Except(Participants.Select(p => p.PersonId));
            if (minToLoad.Count() == 0)
            {
                return;
            }

            Console.Write("Adding participants ");
            foreach (var min in minToLoad)
            {
                var p = new ParticipantSingleDetail()
                {
                    CompetitionId = competitionId,
                    PersonId = min,
                    Status = "Present"
                };
                try
                {
                    var pUri = client.SaveSingleParticipant(p);
                    Participants.Add(client.Get<ParticipantSingleDetail>(pUri));
                }
                catch (ApiException ex)
                {
                    Console.Write(p.PersonId + ": " + ex.InnerException.Message);
                }
                Console.Write(".");
            }
            Console.WriteLine();
        }

        protected void UploadToAPI(Client client)
        {
            var results = Participants.Select(p => new Result()
            {
                ParticipantId = p.Id,
                Rank = p.Rank,
                Rounds = p.Rounds.ToArray()
            });

            try
            {
                Console.WriteLine("Updating participants");
                if (!client.UploadResults(results, Participants.First().CompetitionId))
                {
                    Console.WriteLine("ok");
                }
                else
                {
                    Console.WriteLine(client.LastApiMessage);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error updating participant. " + client.LastApiMessage);
                Console.Error.WriteLine(ex.InnerException?.Message ?? ex.Message);
            }

            //foreach (var p in Participants)
            //{
            //    try
            //    {
            //        Console.Write("Updating participant: " + p.Name);
            //        if (client.UpdateSingleParticipant(p))
            //        {
            //            Console.WriteLine(" => ok");
            //        }
            //        else
            //        {
            //            Console.WriteLine(" => " + client.LastApiMessage);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine();
            //        Console.Error.WriteLine("Error updating participant. " + client.LastApiMessage);
            //        Console.Error.WriteLine(ex.InnerException?.Message ?? ex.Message);
            //    }
            //}
        }

        protected Dance FindDance(ParticipantSingleDetail dancer, string roundName)
        {
            dancer.Rounds ??= new List<Round>();
            var round = dancer.Rounds.FirstOrDefault(d => d.Name == roundName);
            if (round == null)
            {
                round = new Round() { Name = roundName };
                dancer.Rounds.Add(round);
                round.Dances.Add(new Dance() { Name = "BREAKING", Scores = new List<Score>() });
            }

            return round.Dances.First();
        }

    }
}
