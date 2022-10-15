namespace Uploader
{
    using Wdsf.Api.Client;

    internal interface IClientHandler
    {
        void Upload(Client client, int competitionId, IEnumerable<string> data);
    }
}
