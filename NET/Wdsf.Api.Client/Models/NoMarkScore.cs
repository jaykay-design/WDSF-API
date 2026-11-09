namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// The marker the API returns for a dance recorded without marks (JSON only).
    /// </summary>
    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class NoMarkScore : Score
    {
        public const string SerializerTypeName = "nomark";
    }
}
