namespace Wdsf.Api.Examples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;
    using Wdsf.Api.Client.Interfaces;

    class Program
    {
        private static IClient apiClient;

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

            Link participantsReference = competition.Links.First(l => l.Rel.StartsWith(ResourceRelation.CompetitionParticipants));

            switch (participantsReference.Rel.Replace(ResourceRelation.CompetitionParticipants, string.Empty))
            {
                case ".team":
                    {
                        ListTeams(participantsReference);
                        break;
                    }
                case ".couple":
                    {
                        ListCouples(participantsReference);
                        break;
                    }
                case ".single":
                    {
                        ListSingles(participantsReference);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Unkown participant type received");
                        break;
                    }
            }

            Console.ReadKey();
        }

        private static void ListSingles(Link participantsReference)
        {
            List<ParticipantSingle> participants = apiClient.Get<ListOfSingleParticipant>(new Uri(participantsReference.HRef));

            foreach (ParticipantSingle participant in participants)
            {
                ParticipantSingleDetail single = apiClient.Get<ParticipantSingleDetail>(new Uri(participant.Link.First(l => l.Rel == ResourceRelation.Self).HRef));
                Console.Write("{0} {1} {2}\n", single.StartNumber, single.Name, single.Country);
            }
        }

        private static void ListCouples(Link participantsReference)
        {
            List<ParticipantCouple> participants = apiClient.Get<ListOfCoupleParticipant>(new Uri(participantsReference.HRef));

            foreach (ParticipantCouple participant in participants)
            {
                ParticipantCoupleDetail couple = apiClient.Get<ParticipantCoupleDetail>(new Uri(participant.Link.First(l => l.Rel == ResourceRelation.Self).HRef));
                Console.Write("{0} {1} {2}\n", couple.StartNumber, couple.Name, couple.Country);
            }
        }

        private static void ListTeams(Link participantsReference)
        {
            List<ParticipantTeam> participants = apiClient.Get<ListOfTeamParticipant>(new Uri(participantsReference.HRef));

            foreach (ParticipantTeam participant in participants)
            {
                ParticipantTeamDetail team = apiClient.Get<ParticipantTeamDetail>(new Uri(participant.Link.First(l => l.Rel == ResourceRelation.Self).HRef));
                Console.Write("{0} {1} {2}\n", team.StartNumber, team.Team, team.Country);
            }
        }
    }
}
