namespace Remove_results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;

    /// <summary>
    /// An example on how to close a competition and retreive the WRL information.
    /// </summary>
    class Program
    {
        private static Client apiClient;

        static void Main(string[] args)
        {
            // prepare the client for access
            apiClient = new Client("guest", "guest", WdsfEndpoint.Sandbox);

            Console.Write("Enter competition ID:");
            int competitionId;
            if (!int.TryParse(Console.ReadLine(), out competitionId))
            {
                Console.WriteLine("Not a valid competition ID");
                Console.ReadKey();
                return;
            }

            CompetitionDetail competition = apiClient.GetCompetition(competitionId);

            Console.WriteLine(
                "Closing competition {0} {1} {2} in {3}-{4} on {5}",
                competition.CompetitionType,
                competition.AgeClass,
                competition.Discipline,
                competition.Location,
                competition.Country,
                competition.Date);

            competition.Status = "Closed";

            try
            {
                if (!apiClient.UpdateCompetition(competition))
                {
                    Console.WriteLine(string.Format("Failed to close the competition. {0}", apiClient.LastApiMessage));
                    Console.ReadKey();
                    return;
                }

                // we have to get the competition again to retreive the WRL coefficient
                competition = apiClient.GetCompetition(competitionId);
                Console.WriteLine(string.Format("The competitions coefficient is: {0}", competition.Coefficient));

                // find the URL that contains all participants
                Uri allParticipantsUri = competition.Links
                    .Where(l => l.Rel.StartsWith(ResourceRelation.CompetitionParticipants))
                    .Select(l => new Uri(l.HRef))
                    .FirstOrDefault();

                Console.WriteLine("Results:");
                Console.WriteLine(string.Format("{0,3} {1,4} {2,6} {3,-40} {4}", "Rank", "#", "Points", "Name", "Country"));

                ListOfCoupleParticipant participants = apiClient.Get<ListOfCoupleParticipant>(allParticipantsUri);
                foreach (ParticipantCouple participant in participants)
                {
                    ParticipantCoupleDetail p = apiClient.GetCoupleParticipant(participant.Id);
                    Console.WriteLine(string.Format("{0,3}. {1,4} {2,6} {3,-40} {4}", p.Rank, p.StartNumber, p.Points, p.Name, p.Country));
                }

            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }

            Console.ReadKey();
        }
    }
}
