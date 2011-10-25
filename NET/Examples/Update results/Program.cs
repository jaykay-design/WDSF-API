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

            // the participant ID used here is an example. 
            // create a participant ID by using the "Register Participant" example.
            Console.WriteLine("Loading participant.");
            ParticipantCoupleDetail participant = apiClient.GetCoupleParticipant(1373892);

            // the competition must have adjudicators registered for this to work
            // register adjudicators as in the "Register Officials" example
            Console.WriteLine("Loading officials.");
            List<Official> adjudicators = GetAllAdjudicators(participant);

            FillResults(participant, adjudicators);

            try
            {
                Console.WriteLine("Saving participant.");
                bool isSuccess = apiClient.UpdateCoupleParticipant(participant);
                Console.WriteLine("Done.");
            }
            catch (ApiException ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }

            Console.ReadKey();
        }

        private static List<Official> GetAllAdjudicators(ParticipantCoupleDetail participant)
        {
            // get a list of adjudicators for the competition this participant is part of.

            Uri competitionUri = participant.Link
                .Where(l => l.Rel == ResourceRelation.ParticipantCompetition)
                .Select(l => new Uri(l.HRef))
                .FirstOrDefault();

            CompetitionDetail competition = apiClient.Get<CompetitionDetail>(competitionUri);

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
            // clear them depending on how you intend to update results.

            participant.Rounds.Clear();

            string[] danceNames = new string[] { "SAMBA", "CHA CHA CHA", "RUMBA", "PASO DOBLE", "JIVE" };

            // results for round 1
            // couple has got a star, this means a mark from every adjudicator for every dance.
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
