using Uploader;
using Wdsf.Api.Client;

var allowedModelTypeNames = new[] { "trivium" };
if (args.Length < 5)
{
    Console.WriteLine("Missing arguments. Required arguments are: username, password, competitionID, results type, path to results file");
    return;
}

if (!int.TryParse(args[2], out int competitionId))
{
    Console.WriteLine($"{args[2]} is not a valid competititon ID.");
    return;
}

var modelTypeName = args[3];
if (!allowedModelTypeNames.Contains(modelTypeName))
{
    Console.WriteLine($"Not an allowed model type name. Allowed are {string.Join(", ", allowedModelTypeNames)}.");
    return;
}

StreamReader resultsFile;
try
{
    resultsFile = File.OpenText(args[4]);
}
catch (Exception ex)
{
    Console.WriteLine($"Could not open results file {args[4]}: {ex}");
    return;
}

var resultContents = resultsFile.ReadToEnd().Split(Environment.NewLine).Skip(1);
resultsFile.Dispose();

var client = new Client(args[0], args[1], WdsfEndpoint.Services);

IClientHandler handler;
switch (modelTypeName)
{
    case "trivium": handler = new Trivium(); break;
    default: return;
}

handler.Upload(client, competitionId, resultContents);