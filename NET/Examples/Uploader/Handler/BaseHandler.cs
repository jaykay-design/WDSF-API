using Wdsf.Api.Client;
using Wdsf.Api.Client.Models;

namespace Breaking_Uploader.Handler
{
    internal class BaseHandler
    {
        protected IList<ParticipantSingleDetail> Participants { get;  set; }
        protected IList<OfficialDetail> Officials { get; set; }

        protected void LoadBaseData(Client client, int competitionId, string removeScores)
        {
            Console.Write("Loading participants ");
            var participants = client.GetSingleParticipants(competitionId);
            var participantDetails = new List<ParticipantSingleDetail>();
            foreach (var p in participants)
            {
                var sp = client.GetSingleParticipant(p.Id);
                // remove all scores we want to add but leave the others
                foreach (var r in sp.Rounds)
                {
                    foreach (var d in r.Dances)
                    {
                        d.Scores = d.Scores.Where(s => s.Kind != removeScores).ToList();
                    }
                }

                participantDetails.Add(sp);
                Console.Write(".");
            }
            Console.WriteLine();

            Console.Write("Loading officials ");
            var officials = client.GetOfficials(competitionId);
            var officiaDetails = new List<OfficialDetail>();
            foreach (var o in officials)
            {
                officiaDetails.Add(client.GetOfficial(o.Id));
                Console.Write(".");
            }
            Console.WriteLine();


            this.Participants = participantDetails;
            this.Officials = officiaDetails;
        }

        protected void UploadToAPI(Client client)
        {
            foreach (var p in Participants)
            {
                try
                {
                    Console.Write("Updating participant: " + p.Name);
                    client.UpdateSingleParticipant(p);
                    Console.WriteLine(" => " + client.LastApiMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.Error.WriteLine("Error updating participant. " + client.LastApiMessage);
                    Console.Error.WriteLine(ex.InnerException?.Message ?? ex.Message);
                }
            }
        }

        protected Dance FindDance(ParticipantSingleDetail dancer, string roundName)
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
