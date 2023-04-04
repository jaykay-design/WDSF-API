namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class BreakingSeedScore : Score
    {
        public const string SerializerTypeName = "breakingseed";

        [XmlIgnore]
        [JsonProperty("kind")]
        public override string Kind => SerializerTypeName;

        [XmlAttribute("rank")]
        [JsonProperty("rank")]
        public int Rank { get; set; }
    }
}
