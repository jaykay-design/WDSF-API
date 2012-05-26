namespace Update_results
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;
    
    /// <summary>
    /// This example show how to update results of a competition
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

            Console.WriteLine("Loading officials.");
            List<Official> adjudicators = GetAllAdjudicators(competition);
            if (adjudicators.Count == 0)
            {
                Console.WriteLine("No officials found. Can not add results!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Loading participants.");
            List<ParticipantCouple> participants = GetAllParticipants(competition);
            if (participants.Count == 0)
            {
                Console.WriteLine("No participants found. Can not add results!");
                Console.ReadKey();
                return;
            }


            // one couple did not show up so we set it to "Noshow"
            ParticipantCouple missingParticipant = participants.First();
            Console.WriteLine(string.Format("Setting 'No-Show' for participant: {0}", missingParticipant.Couple));
            ParticipantCoupleDetail missing = apiClient.GetCoupleParticipant(missingParticipant.Id);
            missing.Status = "Noshow";
            try
            {
                Console.WriteLine("Saving participant.");
                if (!apiClient.UpdateCoupleParticipant(missing))
                {
                    Console.WriteLine(string.Format("Could not update participant: {0}", apiClient.LastApiMessage));
                }
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                Console.ReadKey();
                return;
            }

            // we just simulate the ranking here. Your application would follow the scating rules to determine the ranking
            int rank = 1;
            foreach (ParticipantCouple participant in participants.Skip(1))
            {
                Console.WriteLine(string.Format("Setting scores for participant: {0}", participant.Couple));
                ParticipantCoupleDetail coupleParticipant = apiClient.GetCoupleParticipant(participant.Id);
                FillResults(coupleParticipant, adjudicators);
                coupleParticipant.Rank = rank.ToString();

                try
                {
                    Console.WriteLine("Saving participant.");
                    if (!apiClient.UpdateCoupleParticipant(coupleParticipant))
                    {
                        Console.WriteLine(string.Format("Could not update participant: {0}", apiClient.LastApiMessage));
                    }
                }
                catch (ApiException ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    Console.ReadKey();
                    return;
                }

                rank++;
            }

            Console.WriteLine("Setting competition status to processing.");
            competition.Status = "Processing";
            apiClient.UpdateCompetition(competition);

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static List<ParticipantCouple> GetAllParticipants(CompetitionDetail competition)
        {
            // get the url for a list of participants of the competition.

            Uri officialsUri = competition.Links
                .Where(l => l.Rel.StartsWith(ResourceRelation.CompetitionParticipants))
                .Select(l => new Uri(l.HRef))
                .FirstOrDefault();

            ListOfCoupleParticpant allParticipants = apiClient.Get<ListOfCoupleParticpant>(officialsUri);

            return new List<ParticipantCouple>(allParticipants);
        }

        private static List<Official> GetAllAdjudicators(CompetitionDetail competition)
        {
            // get the url for the list of adjudicators of the competition.

            Uri officialsUri = competition.Links
                .Where(l => l.Rel == ResourceRelation.CompetitionOfficials)
                .Select(l => new Uri(l.HRef))
                .FirstOrDefault();

            ListOfOfficial allOfficials = apiClient.Get<ListOfOfficial>(officialsUri);

            return new List<Official>(allOfficials);
        }

        private static void FillResults(ParticipantCoupleDetail participant, List<Official> adjudicators)
        {
            Random random = new Random();

            // as we have loeded the participant from the API it may contain results from previous rounds.
            // always upload the entire mark/score set!

            participant.Rounds.Clear();

            string[] danceNames = new string[] { "SAMBA", "CHA CHA CHA", "RUMBA", "PASO DOBLE", "JIVE" };

            // results for round 1
            // couple has got a star, this means a mark from every adjudicator for every dance with IsSet = true
            Round round1 = new Round() { Name = "1" };
            participant.Rounds.Add(round1);

            foreach (string danceName in danceNames)
            {
                Dance dance = new Dance() { Name = danceName };
                round1.Dances.Add(dance);

                foreach (Official adjudicator in adjudicators)
                {
                    MarkScore score = new MarkScore()
                    {
                        IsSet = true,
                        OfficialId = adjudicator.Id
                    };
                    dance.Scores.Add(score);
                }
            }

            // results for round 2
            Round round2 = new Round() { Name = "2" };
            participant.Rounds.Add(round2);

            foreach (string danceName in danceNames)
            {
                Dance dance = new Dance() { Name = danceName };
                round2.Dances.Add(dance);

                foreach (Official adjudicator in adjudicators)
                {
                    // we use a random number to decide if the couple got a mark or not
                    // in real life this would be taken from the adjudicator's decision
                    if (random.Next(0, 10) > 5)
                    {
                        continue;
                    }

                    MarkScore score = new MarkScore()
                    {
                        OfficialId = adjudicator.Id
                    };
                    dance.Scores.Add(score);
                }
            }

            // results for final round
            Round roundF = new Round() { Name = "F" };
            participant.Rounds.Add(roundF);

            foreach (string danceName in danceNames)
            {
                Dance dance = new Dance() { Name = danceName };
                roundF.Dances.Add(dance);

                foreach (Official adjudicator in adjudicators)
                {
                    // we use a random number to determine the rank in a final round
                    // in real life this would be taken from the adjudicator's decision
                    int rank = random.Next(1, 6);

                    FinalScore score = new FinalScore()
                    {
                        Rank = rank,
                        OfficialId = adjudicator.Id
                    };
                    dance.Scores.Add(score);
                }
            }
        }
    }
}
