namespace Uploader
{
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;

    internal class Trivium: IClientHandler
    {
        public void Upload(Client client, int competitionId, IEnumerable<string> data)
        {

            var participants = client.GetSingleParticipants(competitionId);
            var participantDetails = new List<ParticipantSingleDetail>();
            foreach (var p in participants)
            {
                participantDetails.Add(client.GetSingleParticipant(p.Id));
            }

            foreach (var line in data)
            {
                var fields = line.Split(';');
                if(fields.Length < 5)
                {
                    continue;
                }

                var existing = participantDetails.FirstOrDefault(p => p.PersonId == fields[4]);
                if (existing != null)
                {
                    existing.Rank = fields[0];
                    existing.Status = "Present";
                    try
                    {
                        client.UpdateSingleParticipant(existing);
                    }
                    catch (ApiException ex)
                    {
                        Console.WriteLine(fields[1] +": "+ ex.InnerException?.Message ?? ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        client.SaveSingleParticipant(new ParticipantSingleDetail()
                        {
                            CompetitionId = competitionId,
                            PersonId = fields[4],
                            Rank = fields[0],
                            Status = "Present"
                        });
                    }
                    catch (ApiException ex)
                    {
                        Console.WriteLine(fields[1] + ": " + ex.InnerException?.Message ?? ex.Message);
                    }
                }
            }
        }
    }
}
