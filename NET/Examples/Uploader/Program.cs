using CommandLine;
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
            [Option('t', "resultType", Required = true, HelpText = "Trivium, ThreeFold or PreSeed.")]
            public string DataKind { get; init; }
            [Option('d', "dataPath", Required = true, HelpText = "The path to the CSV data (can also be delimited by ;).")]
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
            var allowedModelTypeNames = new[] { "trivium", "threefold", "preseed" };
            var modelTypeName = o.DataKind.ToLower();
            if (!allowedModelTypeNames.Contains(modelTypeName))
            {
                Console.WriteLine($"Not an allowed model type name. Allowed are {string.Join(", ", allowedModelTypeNames)}.");
                return;
            }

            IEnumerable<string> resultContents;
            try
            {
                using var resultsFile = File.OpenText(o.Path);
                resultContents = resultsFile
                    .ReadToEnd()
                    .Split(Environment.NewLine)
                    .Skip(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not open results file {o.Path}: {ex}");
                return;
            }

            //            var client = new Client(o.Username, o.Password, "http://localhost:50350/API/1/");
            var client = new Client(
                o.Username,
                o.Password,
                o.UseSandbox ? WdsfEndpoint.Sandbox : WdsfEndpoint.Services);

            IClientHandler handler;
            switch (modelTypeName)
            {
                case "trivium": handler = new Trivium(); break;
                case "threefold": handler = new Threefold(); break;
                case "preseed": handler = new Preseed(); break;
                default: return;
            }

            try
            {
                handler.Upload(client, o.CompetitionId, resultContents);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}