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
    /// An example on how to celar a participants results.
    /// </summary>
    class Program
    {
        private static Client apiClient;

        static void Main(string[] args)
        {
            // prepare the client for access
            apiClient = // your API key: a token from My WDSF, My API Tokens
            new Client(Environment.GetEnvironmentVariable("WDSF_API_KEY"), WdsfEndpoint.Sandbox);

            // the participant ID used here is an example. 
            // create a participant ID by using the "Register Participant" example.
            Console.WriteLine("Loading participant.");
            ParticipantCoupleDetail participant = apiClient.GetCoupleParticipant(1404026);

            participant.Rounds.Clear();

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
    }
}
