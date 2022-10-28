using CommandLine;
using Newtonsoft.Json;
using Uploader.Handler;
using Wdsf.Api.Client;

namespace Uploader
{
    class Program
    {
        public record Options
        {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            [Option('u', "username", Required = true, HelpText = "Your WDSF API username.")]
            public string Username { get; init; }
            [Option('p', "password", Required = true, HelpText = "Your WDSF API password.")]
            public string Password { get; init; }
            [Option('i', "competitionId", Required = true, HelpText = "The WDSF competition ID.")]
            public int CompetitionId { get; init; }
            [Option('t', "resultType", Required = true, HelpText = "Trivium or ThreeFold.")]
            public string DataKind { get; init; }
            [Option('d', "dataPath", Required = true, HelpText = "The path to the JSON data.")]
            public string Path { get; init; }
            [Option('s', "useSandbox", Required = false, HelpText = "Send data to the WDSF API Sandbox")]
            public bool UseSandbox { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        }

        static void Main(string[] args)
        {

            Parser.Default.ParseArguments<Options>(args)
                               .WithParsed(o =>
                               {
                                   Console.WriteLine("Breaking data uploader");
                                   Console.WriteLine($"Current Arguments: -u {o.Username} -p {o.Password} -i    {o.CompetitionId} -t {o.DataKind} -p {o.Path}");

                                   Run(o);
                               });
        }

        private static void Run(Options o)
        {
            var allowedModelTypeNames = new[] { "trivium", "threefold" };
            var modelTypeName = o.DataKind.ToLower();
            if (!allowedModelTypeNames.Contains(modelTypeName))
            {
                Console.WriteLine($"Not an allowed model type name. Allowed are {string.Join(", ", allowedModelTypeNames)}.");
                return;
            }

            string resultContents;
            try
            {
                using var resultsFile = File.OpenText(o.Path);
                resultContents = resultsFile.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not open results file {o.Path}: {ex}");
                return;
            }

            //            var client = new Client(o.Username, o.Password, "http://localhost/PublicServices/API/1/");
            var client = new Client(
                o.Username,
                o.Password,
                o.UseSandbox ? WdsfEndpoint.Sandbox : WdsfEndpoint.Services);

            IClientHandler handler;
            switch (modelTypeName)
            {
                case "trivium":
                    {
                        var data = JsonConvert.DeserializeObject<Model.Trivium>(resultContents);
                        handler = new Trivium(data, client, o.CompetitionId); 
                        break;
                    }
                case "threefold": throw new NotImplementedException("No implementation for threefold");
                default: return;
            }

            //try
            //{
                handler.Upload();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
            //}
        }
    }
}