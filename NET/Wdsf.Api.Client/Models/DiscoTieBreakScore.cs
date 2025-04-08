namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class DiscoTieBreakScore : Score
    {
        public const string SerializerTypeName = "discoTieBreak";

        [XmlAttribute("rank")]
        [JsonProperty("rank")]
        public int Rank { get; set; }
    }
}
