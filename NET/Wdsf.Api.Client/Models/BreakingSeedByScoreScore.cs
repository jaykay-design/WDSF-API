namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class BreakingSeedByScoreScore : Score
    {
        public const string SerializerTypeName = "breakingseedbyscore";

        [XmlIgnore]
        [JsonProperty("kind")]
        public override string Kind => SerializerTypeName;

        [XmlAttribute("score")]
        [JsonProperty("score")]
        public decimal Score { get; set; }

        [XmlAttribute("isIgnored")]
        [JsonProperty("isIgnored")]
        public bool IsIgnored { get; set; }

        [XmlAttribute("isTieBreak")]
        [JsonProperty("isTieBreak")]
        public bool IsTieBreak { get; set; }
    }
}
