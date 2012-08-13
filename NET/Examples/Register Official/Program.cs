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
                "Registering for competition {0} {1} {2} in {3}-{4} on {5}",
                competition.CompetitionType,
                competition.AgeClass,
                competition.Discipline,
                competition.Location,
                competition.Country,
                competition.Date);

            // prepare the person filter
            Dictionary<string, string> personFilter = new Dictionary<string, string>()
            {
                { FilterNames.Person.NameOrMin, string.Empty} ,
                { FilterNames.Person.Type, "Adjudicator,Chairman"  }
            };

            do
            {
                // ask for an official name and list all possible matches
                ListPersons(personFilter);

                // choose one officials from the list
                PersonDetail selectedPerson = ChoosePerson();

                // and save the person as a new official
                Uri newOfficialUri = SavePerson(competition, selectedPerson);

                if (newOfficialUri != null)
                {
                    // get the newly created official to display it
                    OfficialDetail official = apiClient.Get<OfficialDetail>(newOfficialUri);

                    Console.WriteLine("Added official '{0}' to competition with ID:{1}.", official.Person, official.Id);
                }
                
                Console.Write("Add more? [Y]es/[N]o :");

            } while (Console.ReadKey().Key == ConsoleKey.Y);

            // show all registered officials of this competition
            ListAllOfficials(competition);

            Console.ReadKey();
        }

        private static void ListPersons(Dictionary<string, string> personFilter)
        {
            Console.WriteLine();

            IList<Person> candidates;
            do
            {
                Console.Write("Enter name or MIN of official: ");
                string personName = Console.ReadLine();
                personFilter[FilterNames.Person.NameOrMin] = personName;

                candidates = apiClient.GetPersons(personFilter);

                if (candidates.Count == 0)
                {
                    Console.WriteLine("No person with the name or MIN '{0}' found.");
                }
            } while (candidates.Count == 0);

            // show matches
            Console.WriteLine("Found possible persons:");
            foreach (Person person in candidates)
            {
                Console.WriteLine("{0,10} {1,-40} - {2}", person.Min, person.Name, person.Country);
            }

            Console.WriteLine();
        }

        private static PersonDetail ChoosePerson()
        {
            // pick one person
            PersonDetail selectedPerson = null;
            do
            {
                Console.Write("Enter person MIN: ");
                string min = Console.ReadLine();

                try
                {
                    selectedPerson = apiClient.GetPerson(int.Parse(min));
                }
                catch (ApiException ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            } while (selectedPerson == null);

            Console.WriteLine();

            return selectedPerson;
        }

        private static Uri SavePerson(CompetitionDetail competition, PersonDetail selectedPerson)
        {
            Uri newOfficialUri = null;
            do
            {
                Console.Write("Enter adjudicator char or [nothing] for Chairman: ");
                string adjudicatorChar = Console.ReadLine();

                OfficialDetail official = new OfficialDetail()
                {
                    CompetitionId = competition.Id,
                    Min = int.Parse(selectedPerson.Min)
                };

                if (adjudicatorChar == string.Empty ||
                    adjudicatorChar.Contains("nothing"))
                {
                    official.Task = "Chairman";
                }
                else
                {
                    official.Task = "Adjudicator";
                    official.AdjudicatorChar = adjudicatorChar;
                }

                try
                {
                    newOfficialUri = apiClient.SaveOfficial(official);
                }
                catch (ApiException ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    return null;
                }

            } while (false);

            return newOfficialUri;
        }

        private static void ListAllOfficials(CompetitionDetail competition)
        {
            IList<Official> allOfficials = apiClient.GetOfficials(competition.Id);
            if (allOfficials.Count != 0)
            {
                Console.WriteLine();
                Console.WriteLine("Officials registered so far:");
                foreach (Official official in allOfficials)
                {
                    Console.WriteLine("{0}: {1}", official.Name, official.Nationality);
                }
            }
        }
    }
}
