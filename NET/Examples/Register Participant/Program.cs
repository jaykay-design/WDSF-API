namespace Wdsf.Api.Examples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;
    
    /// <summary>
    /// Shows how to register couples to a competition.
    /// Exception handling is reduced to a minimum for better readability.
    /// </summary>
    class Program
    {
        private static Client apiClient;

        static void Main(string[] args)
        {
            // prepare the client for access
            apiClient = new Client("guest", "guest", WdsfEndpoint.Sandbox);

            // the competition ID used here is an example. 
            // Find further possible competition IDs by going to : https://sandbox.worlddancesport.org/api/1/competition?status=PreRegistration
            CompetitionDetail competition = apiClient.GetCompetition(41077);

            Console.WriteLine(
                "Registering for competition {0} {1} {2} in {3}-{4} on {5}",
                competition.CompetitionType,
                competition.AgeClass,
                competition.Discipline,
                competition.Location,
                competition.Country,
                competition.Date);

            // we want to list only couples that are allowed to participate in this competition
            string allAllowedAgeGroups = GetAllowedAgeGroups(competition);

            // prepare the couple filter
            Dictionary<string,string> coupleFilter = new Dictionary<string,string>()
            {
                { "name", string.Empty} ,
                { "division", competition.Division },
                { "ageGroup", allAllowedAgeGroups }
            };

            do
            {
                // ask for an athlete name and list all possible couples
                ListCouples(coupleFilter);

                // choose one couple id from the list
                CoupleDetail selectedCouple = ChooseCouple();
                
                // and save the couple as a new participant
                Uri newParticipantUri = SaveParticipant(competition, selectedCouple);

                // get the newly created participant to display it
                ParticipantCoupleDetail participant = apiClient.Get<ParticipantCoupleDetail>(newParticipantUri);

                Console.WriteLine("Added couple '{0}' to competition with ID:{1}.", participant.Name, participant.Id);
                Console.Write("An more? [Y]es/[N]o :");

            } while (Console.ReadKey().Key == ConsoleKey.Y);

            // show all registered participants of this competition
            ListAllParticipants(competition);

            Console.ReadKey();
        }

        private static string GetAllowedAgeGroups(CompetitionDetail competition)
        {
            // find out what age classes are allowed to dance in this competition
            string[] allAllowedAgeGroups = apiClient
                .GetAges()
                .Where(a => a.AllowedToDanceIn.Contains(competition.AgeClass))
                .Select(a => a.Name)
                .ToArray();

            return string.Join(",", allAllowedAgeGroups);
        }

        private static void ListCouples(Dictionary<string, string> coupleFilter)
        {
            Console.WriteLine();

            IList<Couple> candidates;
            do
            {
                Console.Write("Enter name or MIN of athlete: ");
                string athelteName = Console.ReadLine();
                coupleFilter["name"] = athelteName;

                candidates = apiClient.GetCouples(coupleFilter);

                if (candidates.Count == 0)
                {
                    Console.WriteLine("No couple with the name or MIN '{0}' found.");
                }
            } while (candidates.Count == 0);

            // show matches
            Console.WriteLine("Found possible couples:");
            foreach (Couple couple in candidates)
            {
                Console.WriteLine("{0,10} {1,-40} - {2}", couple.Id, couple.Name, couple.Country);
            }

            Console.WriteLine();
        }

        private static CoupleDetail ChooseCouple()
        {
            // pick one couple
            CoupleDetail selectedCouple = null;
            do
            {
                Console.Write("Enter couple ID: ");
                string coupleId = Console.ReadLine();

                try
                {
                    selectedCouple = apiClient.GetCouple(coupleId);
                }
                catch (ApiException ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            } while (selectedCouple == null);

            Console.WriteLine();

            return selectedCouple;
        }

        private static Uri SaveParticipant(CompetitionDetail competition, CoupleDetail selectedCouple)
        {
            Uri newParticipantUri = null;
            do
            {
                // choose start number
                Console.Write("Enter start number: ");
                string startNumber = Console.ReadLine();
                int startNumberValue;
                int.TryParse(startNumber, out startNumberValue);

                ParticipantCoupleDetail participant = new ParticipantCoupleDetail()
                {
                    CompetitionId = competition.Id,
                    CoupleId = selectedCouple.Id,
                    StartNumber = startNumberValue
                };

                try
                {
                    newParticipantUri = apiClient.SaveCoupleParticipant(participant);
                }
                catch (ApiException ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    continue;
                }
            } while (false);

            return newParticipantUri;
        }

        private static void ListAllParticipants(CompetitionDetail competition)
        {
            // show list of all registered participants
            Uri allParticipantsUri = competition.Links
                .Where(l => l.Rel == ResourceRelation.CompetitionParticipants)
                .Select(l => new Uri(l.HRef))
                .FirstOrDefault();

            ListOfCoupleParticpant allParticipants = apiClient.Get(allParticipantsUri) as ListOfCoupleParticpant;
            if (allParticipants.Count != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Couples registered so far:");
                foreach (ParticipantCouple participant in allParticipants)
                {
                    Console.WriteLine("{0, 5}: {1,-40} - {2}", participant.StartNumber, participant.Couple, participant.Country);
                }
            }
        }
    }
}
